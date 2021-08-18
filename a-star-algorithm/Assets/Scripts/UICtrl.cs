using System;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    public Dropdown dropDown;

    private void Start()
    {
        dropDown.onValueChanged.AddListener(SetEditType);
    }

    private static void SetEditType(int cellTypeInt)
    {
        GameMng.Instance.editType = (Cell.CellType)cellTypeInt;
    }

    public void RunAStar()
    {
        GameMng.Instance.mapCreator.character.GetComponent<AStar>().RunAStar();
    }
}
