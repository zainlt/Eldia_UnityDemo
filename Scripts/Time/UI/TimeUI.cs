using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI dateText;

    private void OnEnable()
    {
        //EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }

    private void OnDisable()
    {
        //EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }

    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        dateText.text = day.ToString() + "days";
    }
}
