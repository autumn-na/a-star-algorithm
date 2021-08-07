using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameObject prefCell;
    public GameObject prefCharacter;

    public Transform cellParent;

    Cell[,] cells;

    private void Awake()
    {
        Init();
    }
    void Start()
    {
        CreateCharacter();
    }

    void Init()
    {
        cells = new Cell[15, 15];

        for (int i_x = 0; i_x < 15; i_x++)
        {
            for (int j_y = 0; j_y < 15; j_y++)
            {
                GameObject cellClone = Instantiate(prefCell, cellParent);
                cellClone.transform.position = new Vector2(i_x, j_y);

                cells[i_x, j_y] = cellClone.GetComponent<Cell>();
            }
        }
    }

    void CreateCharacter()
    {
        GameObject cloneCharacter = Instantiate(prefCharacter);
        Character character = cloneCharacter.GetComponent<Character>();

        character.MoveToCellImmediate(cells[0, 0]);
    }
}
