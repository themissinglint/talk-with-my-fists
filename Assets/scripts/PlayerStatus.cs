using System.Collections.Generic;
using UnityEngine;

public static class PlayerStatus {
    
    // Game Ending State
    public static bool PlayerHasWon;
    public static bool PlayerHasLost;

    // Acorn Quest
    public static int AcornCount;
    public static bool HasCompletedGardenerFirstAcornQuest;
    public static bool HasCompletedGardenerSecondAcornQuest;
    
    // Mushroom Quest
    public static int MushroomCount;
    public static bool HasCompletedBlacksmithMushroomQuest;
    public static bool HasCompletedSecondBlacksmithMushroomQuest;
    
    // Wood Quest
    public static int WoodCount;
    public static bool HasCompletedTraderWoodQuest;
    
    // Bee Slaying Quest
    public static int BeeKillCount;
    public static bool HasCompletedBlacksmithBeeQuest;
    public static bool HasCompletedGardenerBeeQuest;
    public static bool HasCompletedGardenerSecondBeeQuest;
    
    // Dinosaur Slaying Quest
    public static bool HasKilledDinosaur;
    public static bool HasCompletedDinosaurKillQuest;
    
    // Trader Ladder Quest
    public static bool HasDestroyedTraderLadder;
    public static bool HasCompletedTraderLadderQuest;
    
    // Player Damage
    public static float DamageDealtByDash = 1f;

    public static Dictionary<PlayerStat, float> StatValues = new Dictionary<PlayerStat, float> {
        {PlayerStat.Achievement, 0f},
        {PlayerStat.Exploration, 0f},
        {PlayerStat.Social, 0f},
        {PlayerStat.Violence, 0f}
    };

    public static void AddStat(PlayerStat stat, float amountAdded) {
        StatValues[stat] = Mathf.Clamp01(StatValues[stat] + amountAdded);
        if (PlayerStatDisplay.Instance != null) {
            PlayerStatDisplay.Instance.SetNewStatValue(stat, StatValues[stat]);
        }
    }

    public static int GiveCreditForKilledEnemy(GameObject enemy) {
        if (enemy.GetComponent<Bee>() != null) {
            BeeKillCount++;
            return BeeKillCount;
        }
        return 0;
    }

}
