using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using Random = UnityEngine.Random;

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
        ResetMap();
        
        for (var iX = 0; iX < MapX; iX++)
        {
            for (var jY = 0; jY < MapY; jY++)
            {
                Cells[iX, jY].Type = (Cell.CellType)Random.Range(0, 2);

                if (Cells[iX, jY].Type == Cell.CellType.Road)
                {
                    Cells[iX, jY].Weight = Random.Range(Cell.MINWeight, Cell.MAXWeight);
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
                Cells[iX, jY].parent = null;

                character.Init();
                character.GetComponent<AStar>().Init();
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

    public Vector2 CellToPos(Cell cell)
    {
        var result = new Vector2(-1, -1);
        
        for (var iX = 0; iX < MapX; iX++)
        {
            for (var jY = 0; jY < MapY; jY++)
            {
                if (Cells[iX, jY] != cell) continue;
                result.x = iX;
                result.y = jY;
                    
                return result;
            }
        }

        return result;
    }

    public Cell PosToCell(Vector2 vec2)
    {
        try
        {
            return Cells[(int) vec2.x, (int) vec2.y];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static bool IsEnable(Vector2 vec2)
    {
        return !(vec2.x < 0 | vec2.x >= MapX | vec2.y < 0 | vec2.y >= MapY);
    }
}
