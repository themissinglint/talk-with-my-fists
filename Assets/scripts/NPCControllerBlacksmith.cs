using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
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

    private const int MushroomsReq = 10;
    private const int BeesReq = 10;
    
    protected override void InteractWithPlayer() {
        
        // Reward the player for killing the dinosaur (top priority!)
        if (!PlayerStatus.HasCompletedDinosaurKillQuest && PlayerStatus.HasKilledDinosaur) {
            InteractionToastDisplay.Instance.PopToast(DinosaurKillQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Let the player know this character wants mushrooms!
        if (!PlayerStatus.HasCompletedBlacksmithMushroomQuest && PlayerStatus.MushroomCount < MushroomsReq) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the mushroom quest.
        if (!PlayerStatus.HasCompletedBlacksmithMushroomQuest && PlayerStatus.MushroomCount >= MushroomsReq) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteMushroomQuest();
            return;
        }

        // Let the player know this character wants bees DEAD.
        if (!PlayerStatus.HasCompletedBlacksmithBeeQuest && PlayerStatus.BeeKillCount < BeesReq) {
            InteractionToastDisplay.Instance.PopToast(BeeQuestStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Complete the bee quest.
        if (!PlayerStatus.HasCompletedBlacksmithBeeQuest && PlayerStatus.BeeKillCount >= BeesReq) {
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
    
    private void CompleteMushroomQuest () {
        PlayerStatus.MushroomCount -= MushroomsReq;
        PlayerStatus.HasCompletedBlacksmithMushroomQuest = true;
        PlayerInstance.Instance.GetComponent<CharacterHorizontalMovement>().MovementSpeed += 2f;
        PlayerStatus.AddStat(PlayerStat.Achievement, 0.21f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Violence, -0.07f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Exploration, -0.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, 0.14f), 1.8f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Fast Movement"), 1.5f);
    }
    
    private void CompleteBeeQuest () {
        PlayerStatus.HasCompletedBlacksmithMushroomQuest = true;
        PlayerStatus.DamageDealtByDash += 1f;
        PlayerStatus.AddStat(PlayerStat.Violence, 0.14f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Achievement, .14f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Exploration, -.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, .07f), 1.8f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Double Dash Damage"), 1.5f);
    }
    
}
