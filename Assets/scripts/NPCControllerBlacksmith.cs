using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class NPCControllerBlacksmith : NPCController {

    [Header("Dialogue")]
    public InteractionToastData DinosaurKillQuestCompletion;
    public InteractionToastData MushroomQuestFirstStart;
    public InteractionToastData MushroomQuestFirstCompletion;
    public InteractionToastData MushroomQuestSecondStart;
    public InteractionToastData MushroomQuestSecondCompletion;
    public InteractionToastData BeeQuestStart;
    public InteractionToastData BeeQuestCompletion;
    public InteractionToastData DinosaurKillQuestHint;
    public InteractionToastData FinalDialogue;

    private const int MushroomsReqFirst = 2;
    private const int MushroomsReqSecond = 5;
    private const int BeesReq = 10;
    
    protected override void InteractWithPlayer() {
        
        // Reward the player for killing the dinosaur (top priority!)
        if (!PlayerStatus.HasCompletedDinosaurKillQuest && PlayerStatus.HasKilledDinosaur) {
            InteractionToastDisplay.Instance.PopToast(DinosaurKillQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }

        // Let the player know this character wants mushrooms!
        if (!PlayerStatus.HasCompletedBlacksmithMushroomQuest && PlayerStatus.MushroomCount < MushroomsReqFirst) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestFirstStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the mushroom quest.
        if (!PlayerStatus.HasCompletedBlacksmithMushroomQuest && PlayerStatus.MushroomCount >= MushroomsReqFirst) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestFirstCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteFirstMushroomQuest();
            return;
        }
        
        // Let the player know this character wants MORE mushrooms!
        if (!PlayerStatus.HasCompletedSecondBlacksmithMushroomQuest && PlayerStatus.MushroomCount < MushroomsReqSecond) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestFirstStart, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the second mushroom quest.
        if (!PlayerStatus.HasCompletedSecondBlacksmithMushroomQuest && PlayerStatus.MushroomCount >= MushroomsReqSecond) {
            InteractionToastDisplay.Instance.PopToast(MushroomQuestSecondCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs() {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteSecondMushroomQuest();
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
            CompleteBeeQuest();
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
    
    private void CompleteFirstMushroomQuest () {
        PlayerStatus.MushroomCount -= MushroomsReqFirst;
        PlayerStatus.HasCompletedBlacksmithMushroomQuest = true;
        PlayerInstance.Instance.GetComponent<CharacterHorizontalMovement>().MovementSpeed += 1.5f;
        PlayerStatus.AddStat(PlayerStat.Achievement, 0.21f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Violence, -0.07f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Exploration, -0.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, 0.14f), 1.8f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Faster Movement"), 1.5f);
    }
    
    private void CompleteSecondMushroomQuest () {
        PlayerStatus.MushroomCount -= MushroomsReqSecond;
        PlayerStatus.HasCompletedSecondBlacksmithMushroomQuest = true;
        PlayerInstance.Instance.GetComponent<CharacterHorizontalMovement>().MovementSpeed += 1.5f;
        PlayerStatus.AddStat(PlayerStat.Achievement, 0.21f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Violence, -0.07f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Exploration, -0.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, 0.14f), 1.8f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Fastest Movement"), 1.5f);
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
