using UnityEngine;

public class NPCControllerGardener : NPCController {

    [Header("Dialogue")]
    public InteractionToastData AcornQuestGiving;
    public InteractionToastData AcornQuestCompletion;
    public InteractionToastData BeeWarning;
    public InteractionToastData BeeFirstQuestCompleted;
    public InteractionToastData BeeWarningSecond;
    public InteractionToastData BeeSecondQuestCompleted;
    public InteractionToastData FinalDialogue;
    
    
    protected override void InteractWithPlayer() {

        // Let the player know this character wants acorns!
        if (!PlayerStatus.HasCompletedGardenerAcornQuest && PlayerStatus.AcornCount < 43) {
            InteractionToastDisplay.Instance.PopToast(AcornQuestGiving, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the acorn quest.
        if (!PlayerStatus.HasCompletedGardenerAcornQuest && PlayerStatus.AcornCount >= 43) {
            InteractionToastDisplay.Instance.PopToast(AcornQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            PlayerStatus.AcornCount -= 43;
            PlayerStatus.HasCompletedGardenerAcornQuest = true;
            return;
        }

        // Warn the player about killing bees.
        if (!PlayerStatus.HasCompletedGardenerBeeQuest && PlayerStatus.BeeKillCount < 11) {
            InteractionToastDisplay.Instance.PopToast(BeeWarning, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }

        // Be upset about lack of bees.
        if (!PlayerStatus.HasCompletedGardenerBeeQuest && PlayerStatus.BeeKillCount >= 11) {
            InteractionToastDisplay.Instance.PopToast(BeeFirstQuestCompleted, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            PlayerStatus.HasCompletedGardenerBeeQuest = true;
        }

        // Warn the player again about killing bees.
        if (!PlayerStatus.HasCompletedGardenerSecondBeeQuest && PlayerStatus.BeeKillCount < 20) {
            InteractionToastDisplay.Instance.PopToast(BeeWarningSecond, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }

        // Be vengeful about lack of bees.
        if (!PlayerStatus.HasCompletedGardenerSecondBeeQuest && PlayerStatus.BeeKillCount >= 20) {
            InteractionToastDisplay.Instance.PopToast(BeeSecondQuestCompleted, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            PlayerStatus.HasCompletedGardenerSecondBeeQuest = true;
        }
        
        InteractionToastDisplay.Instance.PopToast(FinalDialogue, gameObject);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        
    }
    
}
