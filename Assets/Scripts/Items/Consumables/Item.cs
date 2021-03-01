using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    [SerializeField] ItemBase _base;
    [SerializeField] int quantity;

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }
    public ItemBase Base
    {
        get { return _base; }
        set { _base = value; }
    }

    public void UseItem(int used)
    {
        quantity -= used;
    }
    public void GainItem(int gained)
    {
        quantity += gained;
    }
}
