using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldInfoBox : MonoBehaviour
{
    [SerializeField] GameObject infoBox;
    [SerializeField] Text infoText;

    public event Action Show;
    public event Action Hide;

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            infoBox.SetActive(false);
            Hide();
        }
    }

    public void DisplayMessage(string message)
    {
        infoText.text = message;
        infoBox.SetActive(true);
        Show();
    }
}
