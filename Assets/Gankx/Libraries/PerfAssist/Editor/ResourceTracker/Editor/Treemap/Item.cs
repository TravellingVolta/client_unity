using MemoryProfilerWindow;
using System;
using UnityEditor;
using UnityEngine;

namespace PerfAssist.Editor.Treemap
{
    public class Item : IComparable<Item>, ITreemapRenderable
    {
        public Group _group;
        public Rect _position;
        public int _index;

        public bool _isNewlyAdded = false;

        public ThingInMemory _thingInMemory;

        public int memorySize { get { return _thingInMemory.size; } }
        public string name { get { return _thingInMemory.caption; } }
        public Color color { get { return _group.color; } }

        public Item(ThingInMemory thingInMemory, Group group)
        {
            _thingInMemory = thingInMemory;
            _group = group;
        }

        public int CompareTo(Item other)
        {
            return (int)(_group != other._group ? other._group.totalMemorySize - _group.totalMemorySize : other.memorySize - memorySize);
        }

        public Color GetColor()
        {
            if (_isNewlyAdded)
            {
                return Color.white;
            }

            return _group.color;
        }

        public Rect GetPosition()
        {
            return _position;
        }

        public string GetLabel()
        {
            string row1 = _group._name;
            string row2 = EditorUtility.FormatBytes(memorySize);
            return row1 + "\n" + row2;
        }
    }
}
