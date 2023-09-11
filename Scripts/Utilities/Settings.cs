using System;
using UnityEngine;
public class Settings
{
    public const float itemFadeDuration = 0.35f;

    public const float targetAlpha = 0.45f;

    public const float secondThreshold = 0.01f;
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //Transition
    public const float fadeDuration = 0.8f;

    public const float sleepFadeDuration = 1f;

    //NPC网格移动
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;
    public const float pixelSize = 0.05f; //16*16 1/16
    public const float animationBreakTime = 20f; //动画间隔

    public const int maxGridSize = 9999;

    //灯光
    public const float lightChangeDuration = 30f;
    public static TimeSpan morningTime = new TimeSpan(5, 0, 0);
    public static TimeSpan nightTime = new TimeSpan(19, 0, 0);
    public static TimeSpan midnightTime = new TimeSpan(23, 59, 59);

    public static Vector3 playerStartPos = new Vector3(-16.5f, -11.6f, 0);
    public const int playerStartMoney = 5000;
}
