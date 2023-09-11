using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.Timeline;
using System;

public class TimeLineManager : Singleton<TimeLineManager>
{
    public PlayableDirector startDirector;
    public PlayableDirector currentDirector;
    private bool isDone;
    public bool IsDone { set => isDone = value; }
    private bool isPause;
    public bool timelineIsDone;

    protected override void Awake()
    {
        base.Awake();
        currentDirector = startDirector;
    }

    private void OnEnable()
    {
        EventHandler.ShowTimeLine += OnShowTimeLine;
    }

    private void OnDisable()
    {
        EventHandler.ShowTimeLine -= OnShowTimeLine;
    }

    private void Update()
    {
        if(isPause && Input.GetKeyDown(KeyCode.Space) && isDone)
        {
            isPause = false;
            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentDirector.playableGraph.GetRootPlayable(0).SetTime(47d);
            timelineIsDone = true;
        }
        if (Input.GetKey(KeyCode.F))
        {
            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(3d);
        }
        if (currentDirector.time > 45f)
        {
            timelineIsDone = true;
        }
        

    }

    public void PauseTimeLine(PlayableDirector director)
    {
        currentDirector = director;
        currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        isPause = true;
    }

    private void OnShowTimeLine()
    {
        currentDirector = FindObjectOfType<PlayableDirector>();
        if (currentDirector != null)
            currentDirector.Play();
    }

    private void OnStopped(PlayableDirector obj)
    {
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
    }

    private void OnPlayed(PlayableDirector obj)
    {
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
}
