using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zain.Inventory;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descrition;
    [SerializeField] private Image itemImage;

    [Header("����")]
    public GameObject resourcePanel;
    [SerializeField] private Image[] resouceItem;

    public void SetupTooltip(ItemDetails itemDetails,SlotType slotType)
    {
        if(itemDetails != null)
        {
            nameText.text = itemDetails.itemName;

            //typeText.text = itemDetails.itemType.ToString();
            typeText.text = GetItemType(itemDetails.itemType);

            descrition.text = itemDetails.itemDescription;

            itemImage.sprite = itemDetails.itemIcon;

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
        
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "Seed",
            ItemType.Commodity => "��Ʒ",
            ItemType.Furniture => "�Ҿ�",
            ItemType.BreakTool => "����",
            ItemType.ChopTool => "����",
            ItemType.CollectTool => "����",
            ItemType.HoeTool => "����",
            ItemType.ReapTool => "����",
            ItemType.WaterTool => "����",
            _ => "��"
        };
    }

    public void SetupResourcePanel(int ID)
    {
        var bluePrintDetails = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(ID);

        for(int i = 0;i < resouceItem.Length; i++)
        {
            if (i < bluePrintDetails.resourceItem.Length)
            {
                var item = bluePrintDetails.resourceItem[i];
                resouceItem[i].gameObject.SetActive(true);
                resouceItem[i].sprite = InventoryManager.Instance.GetItemDetails(item.itemID).itemIcon;
                resouceItem[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemAmount.ToString();
            }
            else
            {
                resouceItem[i].gameObject.SetActive(false);
            }
        }
    }

}
