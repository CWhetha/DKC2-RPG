using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite closedChest;
    [SerializeField] Sprite openChest;

    [SerializeField] ItemBase item;
    [SerializeField] int itemQuantity;
    [SerializeField] int currencyAmount;
    [SerializeField] WorldInfoBox infoBox;

    bool canActivate;
    bool itemGiven;
    private SpriteRenderer spriteR;

    void Start()
    {
        canActivate = false;
        itemGiven = false;
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = closedChest;
    }

    public void HandleUpdate()
    {
        if (canActivate && !itemGiven && Input.GetKeyDown(KeyCode.E))
        {
            GiveItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerSight"))
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerSight"))
        {
            canActivate = false;
        }
    }

    private void GiveItem()
    {
        itemGiven = true;
        spriteR.sprite = openChest;

        string message = "Recieved " + itemQuantity.ToString() + " " + item.Name;
        infoBox.DisplayMessage(message);

        if (itemQuantity != 0)
        {
            PlayerParty.GiveItem(item, itemQuantity);
        }
        if (currencyAmount != 0)
        {
            PlayerParty.Bananas += currencyAmount;
        }
    }

}

