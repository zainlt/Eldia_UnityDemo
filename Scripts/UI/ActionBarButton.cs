using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zain.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private bool canUse;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        private void OnEnable()
        {
            EventHandler.UpdateGameStateEvent += OnUpdateGameSateEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateGameStateEvent -= OnUpdateGameSateEvent;
        }

        private void OnUpdateGameSateEvent(GameState gameState)
        {
            canUse = gameState == GameState.GamePlay;
        }

        private void Update()
        {
            if (Input.GetKeyDown(key) && canUse)
            {
                if(slotUI.itemDetails != null)
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                        slotUI.inventoryUI.UpdateSlotHightLight(slotUI.slotIndex);
                    else
                        slotUI.inventoryUI.UpdateSlotHightLight(-1);
                    EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected, slotUI.slotType);
                }

            }
        }
    }
}

