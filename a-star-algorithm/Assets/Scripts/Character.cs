using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    bool isMoving = false;
    Cell targetCell;

    private void Update()
    {
        ProcMove();
    }

    void ProcMove()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetCell.transform.position, Time.deltaTime);

            if (transform == targetCell.transform)
            {
                isMoving = false;
                targetCell = null;
            }
        }
    }

    public void MoveToCell(Cell cell) 
    {
        targetCell = cell;
        isMoving = true;
    }

    public void MoveToCellImmediate(Cell cell)
    {
        transform.position = cell.transform.position;
    }
}
