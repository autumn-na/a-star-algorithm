using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellType
    {
        BLANK,
        WALL,
        CHARACTER,
        STARTING_POINT,
        END_POINT
    }

    new SpriteRenderer renderer;

    // 0: black, 1: wall
    private CellType _type = CellType.BLANK;
    public CellType Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;

            switch (value)
            {
                case CellType.BLANK:
                    renderer.color = Color.white;
                    break;
                case CellType.WALL:
                    renderer.color = Color.black;
                    break;
                case CellType.CHARACTER:
                    renderer.color = Color.gray;
                    break;
                case CellType.STARTING_POINT:
                    renderer.color = Color.green;
                    break;
                case CellType.END_POINT:
                    renderer.color = Color.red;
                    break;
            }
        }
    }

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {

    }
}
