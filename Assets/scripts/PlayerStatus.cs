using System.Collections.Generic;
using UnityEngine;

public static class PlayerStatus {

    public static int AcornCount;
    public static bool HasCompletedGardenerQuest;

    public static Dictionary<PlayerStat, float> StatValues = new Dictionary<PlayerStat, float> {
        {PlayerStat.Achievement, 0f},
        {PlayerStat.Exploration, 0f},
        {PlayerStat.Social, 0f},
        {PlayerStat.Violence, 0f}
    };

    public static void AddStat(PlayerStat stat, float amountAdded) {
        StatValues[stat] = StatValues[stat] + amountAdded;
        if (PlayerStatDisplay.Instance != null) {
            PlayerStatDisplay.Instance.SetNewStatValue(stat, StatValues[stat]);
        }
    }

}
