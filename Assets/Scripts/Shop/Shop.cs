using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum Shoptype
    {
        Items, Equipment
    }
public class Shop : MonoBehaviour
{
        bool canActivate;
        public GameObject prompt;
        [SerializeField] List<EquipmentBase> weaponList;
        [SerializeField] List<EquipmentBase> armorList;
        [SerializeField] List<ItemBase> itemList;
        [SerializeField] Shoptype shoptype;
        [SerializeField] ShopItemMenu menu;
        [SerializeField] ShopEquipmentMenu equipMenu;

        void Start()
        {
            canActivate = false;
            prompt.SetActive(false);
        }

        public void HandleUpdate()
        {
            if (canActivate && Input.GetKeyDown(KeyCode.E))
            {
                ChangeState();
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                prompt.SetActive(true);
                canActivate = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                prompt.SetActive(false);
                canActivate = false;
            }
        }

        private void ChangeState()
        {
            if (shoptype == Shoptype.Items)
            {
                menu.LoadShop(itemList);
            }
            else if (shoptype == Shoptype.Equipment)
            {
                equipMenu.LoadShop(weaponList, armorList);
            }
        }
    

}
