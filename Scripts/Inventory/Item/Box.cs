using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zain.Inventory
{
    public class Box : MonoBehaviour
    {
        public InventoryBag_SO boxBagTemplate;
        public InventoryBag_SO boxBagData;

        public GameObject signIcon;
        private bool canOpen = false;
        private bool isOpen;

        public int index;   //指示ID

        private void OnEnable()
        {
            if(boxBagData == null)
            {
                boxBagData = Instantiate(boxBagTemplate);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = true;
                signIcon.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = false;
                signIcon.SetActive(false);
            }
        }

        private void Update()
        {
            if(!isOpen && canOpen && Input.GetKeyDown(KeyCode.E))
            {
                //打开箱子
                EventHandler.CallBaseBagOpenEvent(SlotType.Box, boxBagData);
                isOpen = true;
            }

            //人物已经离开范围
            if(!canOpen && isOpen)
            {
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
        }

        /// <summary>
        /// 初始化Box和数据
        /// </summary>
        /// <param name="boxIndex"></param>
        public void InitBox(int boxIndex)
        {
            index = boxIndex;
            var key = this.name + index;
            if (InventoryManager.Instance.GetBoxDataList(key) != null)  //刷新地图读取数据
            {
                boxBagData.itemList = InventoryManager.Instance.GetBoxDataList(key);
            }
            else     //新建箱子
            {
                InventoryManager.Instance.AddBoxDataDict(this);
            }
        }

    }
}

