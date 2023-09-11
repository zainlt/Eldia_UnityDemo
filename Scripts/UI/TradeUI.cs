using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Zain.Inventory
{
    public class TradeUI : MonoBehaviour
    {
        public Text tradeText;
        public InputField tradeAmount;
        public Button addAmountBut;
        public Button deleteAmountBut;
        public Button submitButton;
        public TextMeshProUGUI submitAmount;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemType;
        public TextMeshProUGUI itemTooltipCoin;
        public Image itemImage;
        public int cost;

        private ItemDetails item;
        private bool isSellTrade;

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Awake()
        {
            submitButton.onClick.AddListener(TradeItem);
            addAmountBut.onClick.AddListener(AddAmount);
            deleteAmountBut.onClick.AddListener(DeleteAmount);
        }

        private void Update()
        {
            CalculateAmount();
            submitAmount.text = cost.ToString() + "<color=white>C</color>";
        }

        public void SetupTradeUI(ItemDetails item, bool isSell)
        {
            this.item = item;

            if (isSell)
            {
                tradeText.text = "SELL";
                itemTooltipCoin.text = (item.itemPrice * item.sellPercentage).ToString();
            }
            else
            {
                tradeText.text = "BUY";
                itemTooltipCoin.text = item.itemPrice.ToString();
            }
            isSellTrade = isSell;
            tradeAmount.text = string.Empty;
            submitAmount.text = "0 <color=white>C</color>";
            itemName.text = item.itemName;
            itemType.text = GetItemType(item.itemType);
            itemImage.sprite = item.itemIcon;
        }

        private void CalculateAmount()
        {
            int tradeAmountValue;
            if (int.TryParse(tradeAmount.text, out tradeAmountValue))
            {
                cost = tradeAmountValue * item.itemPrice;

                //��
                if (isSellTrade)
                {
                    cost = (int)(cost * item.sellPercentage);
                }
            }
            else
            {
                tradeAmountValue = 0;
                cost = tradeAmountValue * item.itemPrice;
            }
        }

        private void AddAmount()
        {
            int tradeAmountValue;
            if (!int.TryParse(tradeAmount.text, out tradeAmountValue))
            {
                tradeAmount.text = "0";
                tradeAmountValue = 0;
            }
            if (tradeAmountValue + 1 <= 999)
                tradeAmountValue++;
            tradeAmount.text = tradeAmountValue.ToString();
        }

        private void DeleteAmount()
        {
            int tradeAmountValue;
            if (!int.TryParse(tradeAmount.text, out tradeAmountValue))
            {
                tradeAmount.text = "0";
                tradeAmountValue = 0;
            }
            if (tradeAmountValue - 1 >= 0)
                tradeAmountValue--;
            tradeAmount.text = tradeAmountValue.ToString();
        }

        private void TradeItem()
        {
            var amount = Convert.ToInt32(tradeAmount.text);

            InventoryManager.Instance.TradeItem(item, amount, isSellTrade);

            //�رս��״���
            inventoryUI.OnCloseTradeUI();

            //CancelTrade();

            //inventoryUI.UpdateSlotHightLight(-1);
        }

        private void CancelTrade()
        {
            this.gameObject.SetActive(false);
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
    }
}

