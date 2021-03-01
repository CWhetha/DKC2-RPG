using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollectable : MonoBehaviour
{
    public int amount;
    public CollectibleUI ui;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerParty.Bananas += amount;
            ui.DisplayBanana();
            gameObject.SetActive(false);
        }
    }
}
