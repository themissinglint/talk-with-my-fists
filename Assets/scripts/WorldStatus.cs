using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldStatus {

    public const float GameDuration = 270f;
    public static float GameTimeProgress => Mathf.Clamp01(Time.time / GameDuration);
    public static float GameTimeLeft => Mathf.Max(0f, GameDuration - Time.time);
    public static float TimeThatPlayerWon;
    public static float TimeSinceGameEnd => PlayerStatus.PlayerHasLost ? Time.time - GameDuration : PlayerStatus.PlayerHasWon ? Time.time - TimeThatPlayerWon : 0f;

}
