using System.Collections;
using System.Collections.Generic;
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
            PlayerStatus.WoodCount -= 10;
            PlayerStatus.HasCompletedTraderWoodQuest = true;
            return;
        }

        // Warn the player about killing bees.
        if (!PlayerStatus.HasCompletedTraderLadderQuest && PlayerStatus.HasDestroyedTraderLadder) {
            InteractionToastDisplay.Instance.PopToast(LadderDestructionCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }
        
        InteractionToastDisplay.Instance.PopToast(FinalDialogue, gameObject);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        
    }
    
}
