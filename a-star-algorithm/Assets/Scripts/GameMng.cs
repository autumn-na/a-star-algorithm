using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : Singleton<GameMng>
{
    public Cell.CellType editType;
    public MapCreator mapCreator;

    public void RemoveEndPoint()
    {
        for (var iX = 0; iX < MapCreator.MapX; iX++)
        {
            for (var jY = 0; jY < MapCreator.MapY; jY++)
            {
                if (mapCreator.Cells[iX, jY].Type == Cell.CellType.EndPoint)
                {
                    mapCreator.Cells[iX, jY].Type = Cell.CellType.Road;
                }
            }
        }
    }
}
