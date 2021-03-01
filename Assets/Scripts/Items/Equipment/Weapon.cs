using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Weapon
{
    [SerializeField] EquipmentBase _base;
    [SerializeField] string equipedTo;

    public EquipmentBase Base
    {
        get { return _base; }
        set { _base = value; }
    }

    public string EquipedTo
    {
        get { return equipedTo; }
        set { equipedTo = value; }
    }
}
