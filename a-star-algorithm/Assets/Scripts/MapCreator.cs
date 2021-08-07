using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameObject prefCell;
    public GameObject prefCharacter;

    public Transform cellParent;

    public Cell[,] Cells;
    public const int MapX = 15;
    public const int MapY = 15;

    public Character character;

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        CreateCharacter();
        CreateRandomMap();
        RandomCharacter();
    }

    private void Init()
    {
        Cells = new Cell[MapX, MapY];

        for (var iX = 0; iX < MapX; iX++)
        {
            for (var jY = 0; jY < MapY; jY++)
            {
                var cellClone = Instantiate(prefCell, cellParent);
                cellClone.transform.position = new Vector2(iX, jY);

                Cells[iX, jY] = cellClone.GetComponent<Cell>();
            }
        }
    }

    public void CreateRandomMap()
    {
        for (var iX = 0; iX < MapX; iX++)
        {
            for (var jY = 0; jY < MapY; jY++)
            {
                Cells[iX, jY].Type = (Cell.CellType)Random.Range(0, 2);

                if (Cells[iX, jY].Type == Cell.CellType.Road)
                {
                    Cells[iX, jY].Weight = Random.Range(0, Cell.MAXWeight);
                }
            }
        }

        Cells[Random.Range(0, MapX), Random.Range(0, MapY)].Type = Cell.CellType.EndPoint;

        RandomCharacter();
    }

    public void RandomCharacter()
    {
        var randomX = Random.Range(0, MapX);
        var randomY = Random.Range(0, MapY);
        
        while (Cells[randomX, randomY].Type != Cell.CellType.Road)
        {
            randomX = Random.Range(0, MapX);
            randomY = Random.Range(0, MapY);
        }
        
        character.MoveToCellImmediate(Cells[randomX, randomY]);
    }

    public void ResetMap()
    {
        for (var iX = 0; iX < MapX; iX++)
        {
            for (var jY = 0; jY < MapY; jY++)
            {
                Cells[iX, jY].Type = Cell.CellType.Road;
            }
        }
        
        character.MoveToCellImmediate(Cells[0, 0]);
    }

    private void CreateCharacter()
    {
        var cloneCharacter = Instantiate(prefCharacter);
        character = cloneCharacter.GetComponent<Character>();

        character.MoveToCellImmediate(Cells[0, 0]);
    }
}
