using MoreMountains.CorgiEngine;
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

    private const int AcornsReq = 43;
    
    
    protected override void InteractWithPlayer() {

        // Let the player know this character wants acorns!
        if (!PlayerStatus.HasCompletedGardenerAcornQuest && PlayerStatus.AcornCount < AcornsReq) {
            InteractionToastDisplay.Instance.PopToast(AcornQuestGiving, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            return;
        }
        
        // Complete the acorn quest.
        if (!PlayerStatus.HasCompletedGardenerAcornQuest && PlayerStatus.AcornCount >= AcornsReq) {
            InteractionToastDisplay.Instance.PopToast(AcornQuestCompletion, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
            CompleteAcornQuest();
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
            CompleteFirstBeeQuest();
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
            CompleteSecondBeeQuest();
        }
        
        InteractionToastDisplay.Instance.PopToast(FinalDialogue, gameObject);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, new FXArgs {Amplitude = PerkUpAmp, InputVector = PerkUpDir, Speed = PerkUpSpeed});
        
    }

    private void CompleteAcornQuest() {
        PlayerStatus.AcornCount -= AcornsReq;
        PlayerStatus.HasCompletedGardenerAcornQuest = true;
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
