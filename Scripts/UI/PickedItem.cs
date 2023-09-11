using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PickedItem : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;

    public void SetItem(ItemDetails itemDetails)
    {
        itemIcon.sprite = itemDetails.itemIcon;
        itemName.text = itemDetails.itemName;
    }
}
