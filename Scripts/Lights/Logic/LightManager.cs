using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zain.Save;

public class LightManager : MonoBehaviour//,Isaveable
{
    private LightControl[] sceneLights;
    private LightShift currentLightShift;
    private Season currentSeason;
    private float timeDifference = Settings.lightChangeDuration;

    public string GUID => GetComponent<DataGUID>().guid;

    private void Start()
    {
        //Isaveable saveable = this;
        //saveable.RegisterSaveable();

        sceneLights = FindObjectsOfType<LightControl>();
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadedEvent;
        EventHandler.LightShiftChangeEvent += OnLightShiftChangeEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadedEvent;
        EventHandler.LightShiftChangeEvent -= OnLightShiftChangeEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int obj)
    {
        currentLightShift = LightShift.Morning;
    }

    private void OnAfterSceneLoadedEvent()
    {
        sceneLights = FindObjectsOfType<LightControl>();

        foreach (LightControl light in sceneLights)
        {
            //lightcontrol 改变灯光的方法
            light.ChangeLightShift(currentSeason, currentLightShift, timeDifference);
        }
    }

    private void OnLightShiftChangeEvent(Season season, LightShift lightShift, float timeDifference)
    {
        currentSeason = season;
        this.timeDifference = timeDifference;
        if (currentLightShift != lightShift)
        {
            currentLightShift = lightShift;

            foreach (LightControl light in sceneLights)
            {
                //lightcontrol 改变灯光的方法
                light.ChangeLightShift(currentSeason, currentLightShift, timeDifference);
            }
        }
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();

        saveData.sceneLights = sceneLights;

        return saveData;
    }

    public void RestoreData(GameSaveData saveData)
    {
        this.sceneLights = saveData.sceneLights;
    }
}
