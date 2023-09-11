using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zain.Save;

public class SaveSlotUI : MonoBehaviour
{
    public Text dataTime, dataScene;
    private Button currentButton;
    private DataSlot currentData;

    private int Index => transform.GetSiblingIndex();

    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(LoadGameData);
    }

    private void OnEnable()
    {
        SetupSlotUI();
    }

    private void SetupSlotUI()
    {
        currentData = SavaLoadManager.Instance.dataSlots[Index];

        if (currentData != null)
        {
            dataTime.text = currentData.DataTime;
            dataScene.text = currentData.DataScene;
        }
        else
        {
            dataTime.text = string.Empty;
            dataScene.text = "¿Õ";
        }
    }

    private void LoadGameData()
    {
        if (currentData != null)
        {
            SavaLoadManager.Instance.Load(Index);
        }
        else
        {
            //ÐÂÓÎÏ·
            
            StartCoroutine(ShowTimeLine());
           
        }
    }

    private IEnumerator ShowTimeLine()
    {
        EventHandler.CallShowTimeLine();
        yield return new WaitUntil(() => TimeLineManager.Instance.timelineIsDone == true);
        EventHandler.CallStartNewGameEvent(Index);
    }


}
