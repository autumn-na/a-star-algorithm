using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Scripts
{
    public class AStar : MonoBehaviour
    {
        private PriorityQueue _pQueue;
        private List<Cell> _path;
        private List<Vector2> _closedList;

        private readonly Vector2[] _directions = 
        {
            new Vector2(0, 1), new Vector2(0, -1),
            new Vector2(-1, 0), new Vector2(1, 0)
        };

        private void Awake()
        {
            _pQueue = new PriorityQueue();
            _path = new List<Cell>();
        }

        private void Start()
        {
            UICtrl.Instance.AStarEvent += RunAStar;
            MapCreator.Instance.ResetMapEvent += Init;
            
            Init();
        }

        private void Init()
        {
            _pQueue = new PriorityQueue();
            _path = new List<Cell>();
            _closedList = new List<Vector2>();
        }

        private void RunAStar()
        {       
            _pQueue.Insert(MapCreator.Instance.CellToPos(Character.Instance.curCell), 0);
        
            var current = Vector2.zero;
            while (_pQueue.Count() != 0)
            {
                current = _pQueue.Pop();
                if (MapCreator.Instance.PosToCell(current).Type == Cell.CellType.EndPoint) break;

                if (!_closedList.Contains(current))
                {
                    _closedList.Add(current);
                }

                foreach (var vec2 in _directions)
                {
                    var next = current + vec2;
                    if(_closedList.Contains(next) || !MapCreator.IsEnable(next) || MapCreator.Instance.PosToCell(next).Type == Cell.CellType.Wall) continue;

                    if (_pQueue.Contains(next))
                    {
                        if (!(F(next) < _pQueue.GetPriority(next))) continue;
                        _pQueue.Remove(next);
                        _pQueue.Insert(next, F(next));
                        MapCreator.Instance.PosToCell(next).parent = MapCreator.Instance.PosToCell(current);
                    }
                    else
                    {
                        _pQueue.Insert(next, F(next));
                        MapCreator.Instance.PosToCell(next).parent = MapCreator.Instance.PosToCell(current);
                    }
                }  
            }

            var child = MapCreator.Instance.PosToCell(current);
        
            while (child.parent != null)
            {
                _path.Add(child);
                child = child.parent;
            }
        
            _path.Reverse();
            Character.Instance.MoveByPath(_path);
        }

        private float F(Vector2 start)
        {
            return G() + Heuristic(start);
        }

        private float G()
        {
            return _path.Sum(t => t.Weight);
        }

        private static float Heuristic(Vector2 start)
        {
            var x = 0;
            var y = 0;

            var isFind = false;
            for (var iX = 0; iX < 15; iX++)
            {
                for (var jY = 0; jY < 15; jY++)
                {
                    var endCell = MapCreator.Instance.Cells[iX, jY];

                    if (endCell.Type != Cell.CellType.EndPoint) continue;
                    x = iX;
                    y = jY;
                    isFind = true;
                    break;
                }

                if (isFind) break;
            }
        
            float weightSum = 0;
        
            for (var iX = (int)start.x; !iX.Equals(x);)
            {
                weightSum += MapCreator.Instance.Cells[iX, y].Weight;
                if (iX < x) iX++;
                else iX--;
            }
        
            for (var iY = (int)start.y; !iY.Equals(y);)
            {
                weightSum += MapCreator.Instance.Cells[x, iY].Weight;
                if (iY < y) iY++;
                else iY--;
            }

            var result = weightSum;
        
            return result;
        }
    }
}
