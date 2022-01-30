using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControllerBlacksmith : NPCController {

    [Header("Dialogue")]
    public InteractionToastData DinosaurKillQuestCompletion;
    public InteractionToastData MushroomQuestStart;
    public InteractionToastData MushroomQuestCompletion;
    public InteractionToastData BeeQuestStart;
    public InteractionToastData BeeQuestCompletion;
    public InteractionToastData DinosaurKillQuestHint;
    public InteractionToastData FinalDialogue;
    
    protected override void InteractWithPlayer() {
        
        // Reward the player for killing the dinosaur (top priority!)
        if (!PlayerStatus.HasCompletedDinosaurKillQuest && PlayerStatus.HasKilledDinosaur) {
            InteractionToastDisplay.Instance.PopToast(DinosaurKillQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Let the player know this character wants mushrooms!
        if (!PlayerStatus.HasCompletedBlacksmithMushroomQuest && PlayerStatus.MushroomCount < 10) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the mushroom quest.
        if (!PlayerStatus.HasCompletedBlacksmithMushroomQuest && PlayerStatus.MushroomCount >= 10) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            PlayerStatus.MushroomCount -= 10;
            PlayerStatus.HasCompletedBlacksmithMushroomQuest = true;
            return;
        }

        // Let the player know this character wants bees DEAD.
        if (!PlayerStatus.HasCompletedBlacksmithBeeQuest && PlayerStatus.BeeKillCount < 10) {
            InteractionToastDisplay.Instance.PopToast(BeeQuestStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Complete the bee quest.
        if (!PlayerStatus.HasCompletedBlacksmithBeeQuest && PlayerStatus.BeeKillCount >= 10) {
            InteractionToastDisplay.Instance.PopToast(BeeQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Give a hint about the dino.
        if (!PlayerStatus.HasCompletedDinosaurKillQuest) {
            InteractionToastDisplay.Instance.PopToast(DinosaurKillQuestHint, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Final dialogue if nothing else happens.
        InteractionToastDisplay.Instance.PopToast(FinalDialogue, gameObject);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        
    }
    
}
