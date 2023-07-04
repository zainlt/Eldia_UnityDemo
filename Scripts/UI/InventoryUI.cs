using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zain.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("拖拽图片")]
        public Image dragItem;

        [Header("玩家背包UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;

        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        }


        private void Start()
        {
            for(int i = 0;i < playerSlots.Length; i++)
            {
                //给每个格子序号
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagUI.activeInHierarchy;
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHightLight(-1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 打开关闭背包
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;

            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// 更新slot高亮显示
        /// </summary>
        /// <param name="index"></param>
        public void UpdateSlotHightLight(int index)
        {
            foreach(var slot in playerSlots)
            {
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightLight.gameObject.SetActive(false);
                }
            }
        }
    }
}

