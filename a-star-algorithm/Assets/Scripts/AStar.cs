using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

public class AStar : MonoBehaviour
{
    public MapCreator mapCreator;

    private Character _character;
    public PriorityQueue _pQueue;
    public List<Cell> _path;
    public List<Vector2> _closedList;

    private readonly Vector2[] _directions = 
    {
        new Vector2(0, 1), new Vector2(0, -1),
        new Vector2(-1, 0), new Vector2(1, 0)
    };


    private void Awake()
    {
        mapCreator = GameObject.Find("Map Creator").GetComponent<MapCreator>();
        _character = GetComponent<Character>();
        
        _pQueue = new PriorityQueue();
        _path = new List<Cell>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _pQueue = new PriorityQueue();
        _path = new List<Cell>();
        _closedList = new List<Vector2>();
    }

    public void RunAStar()
    {       
        _pQueue.Insert(mapCreator.CellToPos(_character.curCell), 0);
        
        var current = Vector2.zero;
        while (_pQueue.Count() != 0)
        {
            current = _pQueue.Pop();
            if (mapCreator.PosToCell(current).Type == Cell.CellType.EndPoint) break;

            if (!_closedList.Contains(current))
            {
                _closedList.Add(current);
            }

            foreach (var vec2 in _directions)
            {
                var next = current + vec2;
                if(_closedList.Contains(next) | !MapCreator.IsEnable(next) || mapCreator.PosToCell(next).Type == Cell.CellType.Wall) continue;

                if (_pQueue.Contains(next))
                {
                    if (!(F(next, Vector2.zero) < _pQueue.GetPriority(next))) continue;
                    _pQueue.Remove(next);
                    _pQueue.Insert(next, F(next, Vector2.zero));
                    mapCreator.PosToCell(next).parent = mapCreator.PosToCell(current);
                }
                else
                {
                    _pQueue.Insert(next, F(next, Vector2.zero));
                    mapCreator.PosToCell(next).parent = mapCreator.PosToCell(current);
                }
            }  
        }

        var child = mapCreator.PosToCell(current);
        
        while (child.parent != null)
        {
            _path.Add(child);
            child = child.parent;
        }
        
        _path.Reverse();
        _character.MoveByPath(_path);
    }

    private float F(Vector2 start, Vector2 end)
    {
        return G() + Heuristic(start, end);
    }

    private float G()
    {
        return _path.Sum(t => t.Weight);
    }

    private float Heuristic(Vector2 start, Vector2 end)
    {
        var result = float.MaxValue;
        var x = 0;
        var y = 0;

        var isFind = false;
        for (var iX = 0; iX < 15; iX++)
        {
            for (var jY = 0; jY < 15; jY++)
            {
                var endCell = mapCreator.Cells[iX, jY];

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
            weightSum += mapCreator.Cells[iX, y].Weight;
            if (iX < x) iX++;
            else iX--;
        }
        
        for (var iY = (int)start.y; !iY.Equals(y);)
        {
            weightSum += mapCreator.Cells[x, iY].Weight;
            if (iY < y) iY++;
            else iY--;
        }

        result = weightSum;
        
        return result;
    }
}
