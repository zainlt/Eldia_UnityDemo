using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("“Ù¿÷ ˝æ›ø‚")]
    public SoundDetailsList_SO soundDetailsData;
    public SceneSoundList_SO sceneSoundData;

    [Header("Audio Source")]
    public AudioSource ambientSource;
    public AudioSource gameSource;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("SnapShots")]
    public AudioMixerSnapshot normalSnapShot;
    public AudioMixerSnapshot ambientSnapShot;
    public AudioMixerSnapshot muteSnapShot;
    private float musicTransitionSecond = 8f;

    private float musicValue;
    private float ambientValue;

    private int isStart = 0;

    private Coroutine soundRoutine;

    public float MusicStartSecond => Random.Range(5f, 15f);

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        EventHandler.PlaySoundEvent += OnPlaySoundEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        EventHandler.PlaySoundEvent -= OnPlaySoundEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
    }

    private void OnEndGameEvent()
    {
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
        muteSnapShot.TransitionTo(1f);
    }

    private void OnPlaySoundEvent(SoundName soundName)
    {
        var sounDetails = soundDetailsData.GetSoundDetails(soundName);
        if (sounDetails != null)
            EventHandler.CallInitSoundEffect(sounDetails);
    }

    private void OnAfterSceneLoadEvent()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        SceneSoundItem sceneSound = sceneSoundData.GetSceneSoundItem(currentScene);
        if (sceneSound == null)
            return;

        SoundDetails ambient = soundDetailsData.GetSoundDetails(sceneSound.ambient);
        SoundDetails music = soundDetailsData.GetSoundDetails(sceneSound.music);

        if (soundRoutine != null)
            StopCoroutine(soundRoutine);

        if (isStart == 0)
        {
            musicTransitionSecond = 15f;
            isStart = 1;
        }
        else musicTransitionSecond = 8f;
        soundRoutine = StartCoroutine(PlaySoundRoutine(music, ambient));
    }

    private IEnumerator PlaySoundRoutine(SoundDetails music,SoundDetails ambient)
    {
        if (music != null && ambient != null)
        {
            PlayAmbientClip(ambient, 1f);
            yield return new WaitForSeconds(MusicStartSecond);
            PlayMusicClip(music, musicTransitionSecond);
        }
    }

    /// <summary>
    /// ≤•∑≈±≥æ∞“Ù¿÷
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayMusicClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("MusicVolume", ConvertSoundVolume(soundDetails.soundVolume));
        gameSource.clip = soundDetails.soundClip;
        if (gameSource.isActiveAndEnabled)
            gameSource.Play();

        normalSnapShot.TransitionTo(transitionTime);
    }

    /// <summary>
    /// ≤•∑≈±≥æ∞“Ù–ß
    /// </summary>
    /// <param name="soundDetails"></param>
    private void PlayAmbientClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("AmbientVolume", ConvertSoundVolume(soundDetails.soundVolume));
        ambientSource.clip = soundDetails.soundClip;
        if (ambientSource.isActiveAndEnabled)
            ambientSource.Play();

        ambientSnapShot.TransitionTo(transitionTime);
    }

    private float ConvertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }

    public void SetMusicMaterVolume(float value)
    {
        if ((value * 100 - 80) > 0)
            audioMixer.SetFloat("MusicVolume", 0);
        else
            audioMixer.SetFloat("MusicVolume", (value * 100 - 80));
    }

    public void SetAmbientVolume(float value)
    {
        if ((value * 100 - 80) > 0)
            audioMixer.SetFloat("AmbientVolume", 0);
        else
            audioMixer.SetFloat("AmbientVolume", (value * 100 - 80));
    }

    public float GetMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out musicValue);
        return (musicValue + 80) / 100;
    }

    public float GetAmbientVolume()
    {
        audioMixer.GetFloat("AmbientVolume", out ambientValue);
        return (ambientValue + 80) / 100;
    }
}
