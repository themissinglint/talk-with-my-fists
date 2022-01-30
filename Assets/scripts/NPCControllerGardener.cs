using MoreMountains.CorgiEngine;
using UnityEngine;

public class NPCControllerGardener : NPCController {

    [Header("Dialogue")]
    public InteractionToastData AcornFirstQuestGiving;
    public InteractionToastData AcornFirstQuestCompletion;
    public InteractionToastData AcornSecondQuestGiving;
    public InteractionToastData AcornSecondQuestCompletion;
    public InteractionToastData BeeWarning;
    public InteractionToastData BeeFirstQuestCompleted;
    public InteractionToastData BeeWarningSecond;
    public InteractionToastData BeeSecondQuestCompleted;
    public InteractionToastData FinalDialogue;

    private const int AcornsReqFirst = 13;
    private const int AcornsReqSecond = 28;
    
    
    protected override void InteractWithPlayer() {
        
        // Be upset about lack of bees.
        if (!PlayerStatus.HasCompletedGardenerBeeQuest && PlayerStatus.BeeKillCount >= 11) {
            InteractionToastDisplay.Instance.PopToast(BeeFirstQuestCompleted, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteFirstBeeQuest();
        }
        
        // Be vengeful about lack of bees.
        if (!PlayerStatus.HasCompletedGardenerSecondBeeQuest && PlayerStatus.BeeKillCount >= 20) {
            InteractionToastDisplay.Instance.PopToast(BeeSecondQuestCompleted, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteSecondBeeQuest();
        }

        // Let the player know this character wants acorns!
        if (!PlayerStatus.HasCompletedGardenerFirstAcornQuest && PlayerStatus.AcornCount < AcornsReqFirst) {
            InteractionToastDisplay.Instance.PopToast(AcornFirstQuestGiving, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the first acorn quest.
        if (!PlayerStatus.HasCompletedGardenerFirstAcornQuest && PlayerStatus.AcornCount >= AcornsReqFirst) {
            InteractionToastDisplay.Instance.PopToast(AcornFirstQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteFirstAcornQuest();
            return;
        }
        
        // Let the player know this character wants acorns!
        if (!PlayerStatus.HasCompletedGardenerSecondAcornQuest && PlayerStatus.AcornCount < AcornsReqSecond) {
            InteractionToastDisplay.Instance.PopToast(AcornFirstQuestGiving, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the second acorn quest.
        if (!PlayerStatus.HasCompletedGardenerSecondAcornQuest && PlayerStatus.AcornCount >= AcornsReqSecond) {
            InteractionToastDisplay.Instance.PopToast(AcornFirstQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteSecondAcornQuest();
            return;
        }

        // Warn the player about killing bees.
        if (!PlayerStatus.HasCompletedGardenerBeeQuest && PlayerStatus.BeeKillCount < 11) {
            InteractionToastDisplay.Instance.PopToast(BeeWarning, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }

        // Warn the player again about killing bees.
        if (!PlayerStatus.HasCompletedGardenerSecondBeeQuest && PlayerStatus.BeeKillCount < 20) {
            InteractionToastDisplay.Instance.PopToast(BeeWarningSecond, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        }
        
        InteractionToastDisplay.Instance.PopToast(FinalDialogue, gameObject);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        
    }

    private void CompleteFirstAcornQuest() {
        PlayerStatus.AcornCount -= AcornsReqFirst;
        PlayerStatus.HasCompletedGardenerFirstAcornQuest = true;
        PlayerInstance.Instance.GetComponent<CharacterJump>().NumberOfJumps++;
        PlayerStatus.AddStat(PlayerStat.Exploration, 0.14f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Achievement, .14f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Violence, -.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, .14f), 1.8f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Double Jump"), 1.5f);
    }

    private void CompleteSecondAcornQuest() {
        PlayerStatus.AcornCount -= AcornsReqSecond;
        PlayerStatus.HasCompletedGardenerSecondAcornQuest = true;
        PlayerInstance.Instance.GetComponent<CharacterJump>().NumberOfJumps++;
        PlayerStatus.AddStat(PlayerStat.Exploration, 0.14f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Achievement, .14f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Violence, -.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, .14f), 1.8f);
        ThreadingUtil.Instance.RunLater(() => UnlockableDisplay.Instance.DoUnlockDisplay("Double Jump"), 1.5f);
    }

    private void CompleteFirstBeeQuest() {
        PlayerStatus.HasCompletedGardenerBeeQuest = true;
        PlayerStatus.AddStat(PlayerStat.Violence, 0.14f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Achievement, .14f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Exploration, -.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, -.07f), 1.8f);
    }

    private void CompleteSecondBeeQuest() {
        PlayerStatus.HasCompletedGardenerSecondBeeQuest = true;
        PlayerStatus.AddStat(PlayerStat.Violence, 0.21f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Achievement, -.07f), 0.6f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Exploration, -.07f), 1.2f);
        ThreadingUtil.Instance.RunLater(() => PlayerStatus.AddStat(PlayerStat.Social, -.14f), 1.8f);
    }
    
}
