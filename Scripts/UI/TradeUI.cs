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

                //卖
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

            //关闭交易窗口
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
                ItemType.Commodity => "商品",
                ItemType.Furniture => "家具",
                ItemType.BreakTool => "工具",
                ItemType.ChopTool => "工具",
                ItemType.CollectTool => "工具",
                ItemType.HoeTool => "工具",
                ItemType.ReapTool => "工具",
                ItemType.WaterTool => "工具",
                _ => "无"
            };
        }
    }
}

