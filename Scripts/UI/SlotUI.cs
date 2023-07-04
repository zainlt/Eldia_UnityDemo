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

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

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
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }

            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;
            isSelected = !isSelected;

            inventoryUI.UpdateSlotHightLight(slotIndex);

            if(slotType == SlotType.Bag)
            {
                //通知物品被选中的状态
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
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

                //
                inventoryUI.UpdateSlotHightLight(-1);
            }
            else
            {
                if (itemDetails.canDropped)
                {
                    
                    var post = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    EventHandler.CallInstantiateItemInSecene(itemDetails.itemID, post);
                }

            }
        }
    }

}