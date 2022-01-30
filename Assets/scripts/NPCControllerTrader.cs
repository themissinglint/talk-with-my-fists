using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class NPCControllerTrader : NPCController {

    [Header("Dialogue")]
    public InteractionToastData WoodQuestStart;
    public InteractionToastData WoodQuestCompletion;
    public InteractionToastData LadderDestructionCompletion;
    public InteractionToastData FinalDialogue;
    
    protected override void InteractWithPlayer() {

        // Let the player know this character wants wood!
        if (!PlayerStatus.HasCompletedTraderWoodQuest && PlayerStatus.WoodCount < 10) {
            InteractionToastDisplay.Instance.PopToast(WoodQuestStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the wood quest.
        if (!PlayerStatus.HasCompletedTraderWoodQuest && PlayerStatus.WoodCount >= 10) {
            InteractionToastDisplay.Instance.PopToast(WoodQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteWoodQuest();
            return;
        }

        // The player just destroyed a ladder. What a jerk!
        if (!PlayerStatus.HasCompletedTraderLadderQuest && PlayerStatus.HasDestroyedTraderLadder) {
            InteractionToastDisplay.Instance.PopToast(LadderDestructionCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }
        
        InteractionToastDisplay.Instance.PopToast(FinalDialogue, gameObject);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        
    }

    private void CompleteWoodQuest() {
        PlayerStatus.WoodCount -= 10;
        PlayerStatus.HasCompletedTraderWoodQuest = true;
        PlayerInstance.Instance.GetComponent<CharacterDash>().DashDistance += 2f;
        PlayerStatus.AddStat(PlayerStat.Exploration, 0.21f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Achievement, -.07f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Violence, -.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Long Dash"), 1.5f);
    }
    
}
