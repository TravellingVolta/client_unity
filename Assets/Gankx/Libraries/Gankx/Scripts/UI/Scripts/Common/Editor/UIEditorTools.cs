﻿using UnityEditor;
using UnityEngine;

namespace Gankx
{
    public static class UIEditorTools
    {
        private static bool MyEndHorizontal;

        public static bool DrawPrefixButton(string text)
        {
            return GUILayout.Button(text, "DropDown", GUILayout.Width(76f));
        }

        public static bool DrawPrefixButton(string text, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, "DropDown", options);
        }

        public static int DrawPrefixList(int index, string[] list, params GUILayoutOption[] options)
        {
            return EditorGUILayout.Popup(index, list, "DropDown", options);
        }

        public static int DrawPrefixList(string text, int index, string[] list, params GUILayoutOption[] options)
        {
            return EditorGUILayout.Popup(text, index, list, "DropDown", options);
        }

        public static void SetLabelWidth(float width)
        {
            EditorGUIUtility.labelWidth = width;
        }

        public static void RegisterUndo(string name, params Object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                Undo.RecordObjects(objects, name);

                foreach (var obj in objects)
                {
                    if (obj == null)
                    {
                        continue;
                    }

                    EditorUtility.SetDirty(obj);
                }
            }
        }

        public static bool DrawMinimalisticHeader(string text)
        {
            return DrawHeader(text, text, false, true);
        }

        public static bool DrawHeader(string text)
        {
            return DrawHeader(text, text, false, UIEditorPrefs.minimalisticLook);
        }

        public static bool DrawHeader(string text, string key)
        {
            return DrawHeader(text, key, false, UIEditorPrefs.minimalisticLook);
        }

        public static bool DrawHeader(string text, bool detailed)
        {
            return DrawHeader(text, text, detailed, !detailed);
        }

        public static bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
        {
            var state = EditorPrefs.GetBool(key, true);

            if (!minimalistic)
            {
                GUILayout.Space(3f);
            }

            if (!forceOn && !state)
            {
                GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            }

            GUILayout.BeginHorizontal();
            GUI.changed = false;

            if (minimalistic)
            {
                if (state)
                {
                    text = "\u25BC" + (char) 0x200a + text;
                }
                else
                {
                    text = "\u25BA" + (char) 0x200a + text;
                }

                GUILayout.BeginHorizontal();
                GUI.contentColor = EditorGUIUtility.isProSkin
                    ? new Color(1f, 1f, 1f, 0.7f)
                    : new Color(0f, 0f, 0f, 0.7f);
                if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f)))
                {
                    state = !state;
                }

                GUI.contentColor = Color.white;
                GUILayout.EndHorizontal();
            }
            else
            {
                text = "<b><size=11>" + text + "</size></b>";
                if (state)
                {
                    text = "\u25BC " + text;
                }
                else
                {
                    text = "\u25BA " + text;
                }

                if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f)))
                {
                    state = !state;
                }
            }

            if (GUI.changed)
            {
                EditorPrefs.SetBool(key, state);
            }

            if (!minimalistic)
            {
                GUILayout.Space(2f);
            }

            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state)
            {
                GUILayout.Space(3f);
            }

            return state;
        }

        public static void BeginContents()
        {
            BeginContents(UIEditorPrefs.minimalisticLook);
        }

        public static void BeginContents(bool minimalistic)
        {
            if (!minimalistic)
            {
                MyEndHorizontal = true;
                GUILayout.BeginHorizontal();
                EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
            }
            else
            {
                MyEndHorizontal = false;
                EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
                GUILayout.Space(10f);
            }

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (MyEndHorizontal)
            {
                GUILayout.Space(3f);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(3f);
        }

        public static Object LoadAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return AssetDatabase.LoadMainAssetAtPath(path);
        }

        public static T LoadAsset<T>(string path) where T : Object
        {
            var obj = LoadAsset(path);
            if (obj == null)
            {
                return null;
            }

            var val = obj as T;
            if (val != null)
            {
                return val;
            }

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var go = obj as GameObject;
                if (go != null)
                {
                    return go.GetComponent(typeof(T)) as T;
                }
            }

            return null;
        }
    }
}