using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    [System.Serializable]
    public class PriorityQueue
    {
        private readonly List<Vector2> _items;
        private readonly List<float> _priorities;

        public PriorityQueue()
        {
            _items = new List<Vector2>();
            _priorities = new List<float>();
        }

        public void Insert(Vector2 item, float priority)
        {
            int index;
            for (index = 0; index < _priorities.Count; index++)
            {
                if (priority < _priorities[index])
                {
                    break;
                }
            }
        
            _items.Insert(index, item);
            _priorities.Insert(index, priority);
        }

        public Vector2 Pop()
        {
            var ret = _items[0];
            _items.RemoveAt(0);
            _priorities.RemoveAt(0);
        
            return ret;
        }

        public int Count()
        {
            return _items.Count;
        }

        public bool Contains(Vector2 item)
        {
            return _items.Any(it => item == it);
        }

        public float GetPriority(Vector2 item)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i] == item)
                    return _priorities[i];
            }

            return -1;
        }

        public void Remove(Vector2 item)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                if (item != _items[i]) continue;
                _items.RemoveAt(i);
                _priorities.RemoveAt(i);
            }
        }
    }
}
