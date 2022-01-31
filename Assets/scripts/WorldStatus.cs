using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldStatus {

    public const float GameDuration = 20f;
    public static float GameTimeProgress => Mathf.Clamp01(Time.time / GameDuration);
    public static float GameTimeLeft => Mathf.Max(0f, GameDuration - Time.time);
    public static float TimeSinceGameEnd => Time.time - GameDuration;

}
