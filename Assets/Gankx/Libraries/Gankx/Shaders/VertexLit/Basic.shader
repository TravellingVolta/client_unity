// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Celf/VertexLit/Basic" {
Properties {
	_Color ("Main Color", Color) = (.5, .5, .5, 1)
	_MainTex ("Main Tex (RGB)", 2D) = "white" {}
}

CGINCLUDE
		#include "UnityCG.cginc"
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma target 2.0

		fixed4 _Color;
		sampler2D _MainTex ;

		struct v2f {
			float4  pos : SV_POSITION;
			float2  uv[2] : TEXCOORD0;
		};

		float4 _MainTex_ST;
        
		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos (v.vertex);
			o.uv[0] = TRANSFORM_TEX (v.texcoord, _MainTex);
            o.uv[1] = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			return o;
		}

		fixed4 frag (v2f i) : COLOR
		{
			fixed3 lay1 = tex2D(_MainTex, i.uv[0]).rgb;
			fixed4 c = fixed4(_Color.rgb * 2.0, 1.0);
			c.rgb *= lay1;
           	c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv[1]));
			return c;
		}
ENDCG

SubShader {
	Tags { "Queue" = "Geometry-100"  "RenderType"="Opaque" "DepthMode"="true"}
	Lighting Off
	Cull Back
	Blend Off

    Pass {
		Tags { "LIGHTMODE"="VertexLMRGBM" }
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		ENDCG
    }

	Pass {
		Tags { "LIGHTMODE"="VertexLM" }
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		ENDCG
    }

	Pass {
		Tags { "LIGHTMODE" = "Vertex" }
		Material { Diffuse (1,1,1,1) }
		Lighting On
		SetTexture [_MainTex] {	Combine texture * primary DOUBLE, texture * primary	} 
	}
}

//Fallback "VertexLit"
} 