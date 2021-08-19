using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public delegate void CharacterMoveHandler(Cell cell);
    public delegate void ResetMapHandler();

    public class MapCreator : MonoBehaviour
    {
        public static MapCreator Instance;
        
        public GameObject prefCell;

        public Transform cellParent;

        public Cell[,] Cells;
        
        private const int MapX = 15;
        private const int MapY = 15;

        public event CharacterMoveHandler CharacterMoveEvent;

        private void RunCharacterMoveEvent(Cell cell)
        {
            CharacterMoveEvent?.Invoke(cell);
        }
        
        public event ResetMapHandler ResetMapEvent;

        private void RunResetMapEvent()
        {
            ResetMapEvent?.Invoke();
        }
        
        private void Awake()
        {
            Instance = this;
            CreateCells();
        }
        private void Start()
        {
            CreateRandomMap();
        }

        private void CreateCells()
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

            RunCharacterMoveEvent(Cells[randomX, randomY]);
        }

        public void ResetMap()
        {
            for (var iX = 0; iX < MapX; iX++)
            {
                for (var jY = 0; jY < MapY; jY++)
                {
                    Cells[iX, jY].Type = Cell.CellType.Road;
                    Cells[iX, jY].parent = null;
                    
                    RunResetMapEvent();
                }
            }
        
            RunCharacterMoveEvent(Cells[0, 0]);
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
            catch
            {
                return null;
            }
        }

        public static bool IsEnable(Vector2 vec2)
        {
            return !(vec2.x < 0 | vec2.x >= MapX | vec2.y < 0 | vec2.y >= MapY);
        }
    }
}
