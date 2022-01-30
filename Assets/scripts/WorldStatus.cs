using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldStatus {

    public const float GameDuration = 240f;
    public static float GameTimeProgress => Time.time / GameDuration;

}
