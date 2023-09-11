using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Zain.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHightLight;
        [SerializeField] private Button button;

        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;       //在Inventory里赋值

        public ItemDetails itemDetails;
        public int itemAmount;
        private bool isOpened;

        public InventoryLocation location
        {
            get
            {
                return slotType switch
                {
                    SlotType.Bag => InventoryLocation.Player,
                    SlotType.Box => InventoryLocation.Box,
                    _ => InventoryLocation.Player,
                };
            }
        }

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;
            if (itemDetails == null)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子信息和UI
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            slotImage.enabled = true;
            amountText.text = amount.ToString();
            button.interactable = true;
        }

        /// <summary>
        /// slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;

                inventoryUI.UpdateSlotHightLight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected ,slotType);
            }

            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
            itemAmount = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;
            

            if (slotType == SlotType.Bag)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    isSelected = !isSelected;

                    inventoryUI.UpdateSlotHightLight(slotIndex);

                    //通知物品被选中的状态
                    EventHandler.CallItemSelectedEvent(itemDetails, isSelected, slotType);
                }
                //鼠标右键
                else if (eventData.button == PointerEventData.InputButton.Right && GetItemType(itemDetails))
                {

                    EventHandler.CallShowTradeUI(itemDetails, true, slotType);
                }
            }
            else if (slotType == SlotType.Shop)
            {
                isSelected = !isSelected;

                inventoryUI.UpdateSlotHightLight(slotIndex);
                //通知物品被选中的状态
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected, slotType);
                EventHandler.CallShowTradeUI(itemDetails, false, slotType);
            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0 && slotType != SlotType.Shop)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightLight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            //Debug.Log(eventData.pointerCurrentRaycast);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var target = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = target.slotIndex;

                //Player
                if (slotType == SlotType.Bag && target.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                else if (slotType != SlotType.Shop && target.slotType != SlotType.Shop && slotType != target.slotType)
                {
                    //跨背包交换数据
                    InventoryManager.Instance.SwapItem(location, slotIndex, target.location, target.slotIndex);
                }

                //
                inventoryUI.UpdateSlotHightLight(-1);
            }
            //else
            //{
            //    if (itemDetails.canDropped)
            //    {
                    
            //        var post = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            //        //EventHandler.CallInstantiateItemInSecene(itemDetails.itemID, post);
            //        EventHandler.CallDropItemEvent(itemDetails.itemID, post, itemDetails.itemType);
            //    }

            //}
        }

        private bool GetItemType(ItemDetails itemDetails)
        {
            bool isCanSell = itemDetails.itemType switch
            {
                ItemType.Seed => true,
                ItemType.Furniture => true,
                ItemType.Commodity => true,
                _ => false,
            };

            return isCanSell;
        }
    }

}