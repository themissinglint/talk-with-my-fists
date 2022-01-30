using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControllerGardener : NPCController {

    [Header("Dialogue")]
    public InteractionToastData AcornQuestGiving;
    public InteractionToastData AcornQuestCompletion;
    public InteractionToastData BeeWarning;
    
    protected override void InteractWithPlayer() {

        // Let the player know this character wants acorns!
        if (!PlayerStatus.HasCompletedGardenerQuest && PlayerStatus.AcornCount < 10) {
            InteractionToastDisplay.Instance.PopToast(AcornQuestGiving, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the acorn quest.
        if (!PlayerStatus.HasCompletedGardenerQuest && PlayerStatus.AcornCount >= 10) {
            InteractionToastDisplay.Instance.PopToast(AcornQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            PlayerStatus.AcornCount -= 10;
            PlayerStatus.HasCompletedGardenerQuest = true;
            return;
        }

        // Warn the player about killing bees.
        if (PlayerStatus.HasCompletedGardenerQuest) {
            InteractionToastDisplay.Instance.PopToast(BeeWarning, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }
        
    }
    
}
