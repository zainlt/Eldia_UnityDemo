using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bed : MonoBehaviour
{
    private bool isFade;
    private bool isSleep;
    private bool canSleep = false;
    public GameObject signIcon;
    public CanvasGroup fadeCanvasGroup;

    public TimeSpan GameTime => TimeManager.Instance.GameTime;

    private void Awake()
    {
        fadeCanvasGroup = GameObject.FindWithTag("BedFader").GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        CanSleep();

        if(canSleep && Input.GetKeyDown(KeyCode.E) && !isFade)
        {
            StartCoroutine(AtSleep());
            isSleep = true;
        }

        if (!canSleep)
        {
            isSleep = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canSleep)
        {
            signIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            signIcon.SetActive(false);
        }
    }

    private void CanSleep()
    {
        if (GameTime < Settings.morningTime || GameTime >= Settings.nightTime)
        {
            canSleep = true;
        }
        else
        {
            canSleep = false;
        }

    }

    private void SleepDayIncreate()
    {
        if(GameTime >= Settings.nightTime && GameTime <= Settings.midnightTime)
        {
            TimeManager.Instance.gameDay++;
            TimeManager.Instance.gameHour = 5;
            TimeManager.Instance.gameMinute = 0;
            TimeManager.Instance.gameSecond = 0;
            TimeManager.Instance.UpdateGameTime();
            EventHandler.CallGameDateEvent(TimeManager.Instance.gameHour, TimeManager.Instance.gameDay, 
                TimeManager.Instance.gameMonth, TimeManager.Instance.gameYear, TimeManager.Instance.gameSeason);
            EventHandler.CallGameDayEvent(TimeManager.Instance.gameDay, TimeManager.Instance.gameSeason);
        }
        else
        {
            TimeManager.Instance.gameHour = 5;
            TimeManager.Instance.gameMinute = 0;
            TimeManager.Instance.gameSecond = 0;
            TimeManager.Instance.UpdateGameTime();
        }
    }

    private IEnumerator AtSleep()
    {
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
        yield return Fade(1f);
        SleepDayIncreate();
        yield return new WaitForSeconds(2.5f);
        yield return Fade(0);
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;

        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.sleepFadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;
    }
}
