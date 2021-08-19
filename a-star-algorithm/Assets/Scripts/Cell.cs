using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Cell : MonoBehaviour
    {
        private static readonly List<Cell> Cells = new List<Cell>();
        private SpriteRenderer _renderer;

        private CellType _type = CellType.Road;
        private static CellType _editType;
        
        private const int WeightStep = 33;
        
        private float _weight;

        public enum CellType
        {
            Road,
            Wall,
            EndPoint,
            Character,
        }
        
        public CellType Type
        {
            get => _type;
            set
            {
                _type = value;

                switch (value)
                {
                    case CellType.Road:
                        _renderer.color = Color.white;
                        Weight = MINWeight;
                        break;
                    case CellType.Wall:
                        _renderer.color = Color.black;
                        Weight = 999;
                        break;
                    case CellType.EndPoint:
                        _renderer.color = Color.green;
                        Weight = MINWeight;
                        break;
                    case CellType.Character:
                        _renderer.color = Color.gray;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public float Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                if (Type != CellType.EndPoint)
                {
                    _renderer.color = new Color(1 - Weight, 1 - Weight, 1 - Weight);
                }
            }
        }
        
        public Cell parent;
        
        public const float MINWeight = 0f;
        public const float MAXWeight = 0.5f;

        private void Awake()
        {
            Cells.Add(this);
            
            _renderer = GetComponent<SpriteRenderer>();
            parent = null;
        }

        private void Start()
        {
            UICtrl.Instance.EditEvent += EditHandler;
        }

        private void OnMouseDown()
        {
            EditMap();
        }

        private void OnMouseEnter()
        {
            if (!Input.GetMouseButton(0)) return;

            EditMap();
        }

        private void OnMouseOver()
        {
            EditWeight();
        }

        private void EditMap()
        {
            if (_editType == CellType.EndPoint)
            {
                foreach (var cell in Cells.Where(cell => cell.Type == CellType.EndPoint))
                {
                    cell.Type = CellType.Road;
                }
            }
            else if (_editType == CellType.Character && Type == CellType.Wall)
            {
                return;
            }
            else if (_editType == CellType.Character)
            {
                Character.Instance.MoveToCellImmediate(this);
                return;
            }

            Type = _editType;
        }

        private void EditWeight()
        {
            if (Type == CellType.Road)
            {
                Weight = Mathf.Min(Mathf.Max(MINWeight, Weight + Input.mouseScrollDelta.y / WeightStep), MAXWeight);
            }
        }

        private static void EditHandler(int cellTypeInt)
        {
            _editType = (CellType)cellTypeInt;
        }
    }
}
