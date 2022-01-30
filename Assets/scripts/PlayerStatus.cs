using System.Collections.Generic;

public static class PlayerStatus {

    // Acorn Quest
    public static int AcornCount;
    public static bool HasCompletedGardenerAcornQuest;
    
    // Mushroom Quest
    public static int MushroomCount;
    public static bool HasCompletedBlacksmithMushroomQuest;
    
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
