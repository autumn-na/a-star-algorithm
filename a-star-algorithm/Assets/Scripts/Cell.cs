using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellType
    {
        Road,
        Wall,
        EndPoint,
        Character,
    }

    private SpriteRenderer _renderer;

    private CellType _type = CellType.Road;

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

    public Cell parent = null;

    private const int WeightStep = 33;
    public const float MINWeight = 0f;
    public const float MAXWeight = 0.5f;
    private float _weight = 0;
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

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        parent = null;
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
        if (GameMng.Instance.editType == CellType.EndPoint)
        {
            GameMng.Instance.RemoveEndPoint();
        }

        if (GameMng.Instance.editType == CellType.Character)
        {
            if (Type == CellType.Wall)
            {
                return;
            }
            
            GameMng.Instance.mapCreator.character.MoveToCellImmediate(this);
            return;
        }
        
        Type = GameMng.Instance.editType;
    }

    private void EditWeight()
    {
        if (Type == CellType.Road)
        {
            Weight = Mathf.Min(Mathf.Max(MINWeight, Weight + Input.mouseScrollDelta.y / WeightStep), MAXWeight);
        }
    }
}
