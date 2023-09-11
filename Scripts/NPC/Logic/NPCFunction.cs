using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCFunction : Singleton<NPCFunction>
{
    public InventoryBag_SO shopData;
    private bool isOpen;

    private void Update()
    {
        if(isOpen && Input.GetKeyDown(KeyCode.Escape) )
        {
            //¹Ø±Õ±³°ü
            CloseShop();
        }
    }

    public void OpenShop()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, shopData);
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }

    public void CloseShop()
    {
        isOpen = false;
        EventHandler.CallBaseBagCloseEvent(SlotType.Shop, shopData);
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
    }
}
