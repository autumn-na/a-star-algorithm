using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // 0: black, 1: wall
    private int _type = 0;
    public int Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            if(value == 0) GetComponent<SpriteRenderer>().color = Color.white;
            else GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    private void OnMouseDown()
    {
        if (Type == 0) Type = 1;
        else Type = 0;
    }
}
