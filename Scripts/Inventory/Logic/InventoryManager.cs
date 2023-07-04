using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zain.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;
        [Header("��������")]
        public InventoryBag_SO playerBag;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            //EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
            ////����
            //EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
            //EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            //EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            //EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
            //EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
            //EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            //EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void OnDropItemEvent(int ID,Vector3 pos)
        {
            RemoveItem(ID, 1);
        }

        /// <summary>
        /// ͨ��ID��ȡ����
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// �����Ʒ��Player����
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy">�Ƿ�Ҫ������Ʒ</param>
        public void AddItem(Item item,bool toDestroy)
        {
            var index = GetItemIndexBag(item.itemID);

            AddItemAtIndex(item.itemID, index, 1);

            Debug.Log(GetItemDetails(item.itemID).itemID + "name:" + GetItemDetails(item.itemID).itemName);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }

            //����UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ͨ����ƷID�ҵ�����������Ʒ��λ��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private int GetItemIndexBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// ��鱳���Ƿ��п�λ
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0;i < playerBag.itemList.Count; i++)
            {
                if(playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ָ��λ�������Ʒ
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if (index == -1 && CheckBagCapacity()) //����û�������Ʒ
            {
                var item = new InventoryItem { itemAmount = amount, itemID = ID };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else //�����������Ʒ
            {
                int currenAmount = playerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemAmount = currenAmount, itemID = ID };
                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// ������Ʒ
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="targetIndex"></param>
        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem currenItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if (targetItem.itemID != 0)
            {
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetIndex] = currenItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = currenItem;
                playerBag.itemList[fromIndex] = new InventoryItem();

            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        //�Ƴ���Ʒ
        private void RemoveItem(int ID,int removeAmount)
        {
            var index = GetItemIndexBag(ID);

            if (playerBag.itemList[index].itemAmount > removeAmount)
            {
                var amount = playerBag.itemList[index].itemAmount - removeAmount;
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                playerBag.itemList[index] = item;
            }
            else if(playerBag.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}

