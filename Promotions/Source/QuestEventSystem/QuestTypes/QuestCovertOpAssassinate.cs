using Newtonsoft.Json;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Source.QuestEventSystem.QuestTypes;

/// <summary>
/// Covert Ops: Assassination
/// The target is spawned in a target town along with a group of guards.
/// Once the target is killed, the quest is completed.
/// </summary>
public class QuestCovertOpAssassinate : Quest
{
    [JsonProperty]
    public int targetCharaUID;

    [JsonProperty]
    public int targetZoneUID;
    
    [JsonProperty]
    public int TargetLvl;
    
    public override int RangeDeadLine => 30;

    public override int KarmaOnFail => 0;

    public override bool FameContent => true;
    
    public Zone? Zone => RefZone.Get(targetZoneUID);
    
    private Chara? Target => EClass.game.cards.Find(targetCharaUID);

    public override string RefDrama1 => Target?.Name ?? "???";
    public override string RefDrama2 => Zone?.Name ?? "???";

    public override int BaseMoney => source.money + EClass.curve(DangerLv, 20, 20) * 10;

    public override int FameOnComplete => base.FameOnComplete * 2;

    public override string GetTextProgress()
    {
        return "covertops_assassinate_progress".langGame(RefDrama1, RefDrama2);
    }

    public override void OnInit()
    {
        base.OnInit();
        targetZoneUID = EClass.world.region.GetRandomTown()?.uid ?? 0;
    }
    
    public override void OnStart()
    {
        if (Zone == null)
        {
            // If this quest was created without a Zone, null it.
            EClass.game.quests.Remove(this);
            if (this.chara is { quest: not null } && this.chara.quest.uid == this.uid) this.chara.quest = null;
            return;
        }
        
        // Scale the Target
        float num = difficulty * 0.25f + EClass.rndf(0.25f);
        float num2 = (DangerLv + EClass.rnd(5)) * num + 1f;
        int num3 = Mathf.RoundToInt(EClass.rndf(num2 * 0.25f) + num2 * 0.75f);
        Chara target = CharaGen.Create("capture_target", num3);
        targetCharaUID = chara.uid;
        target.SetGlobal();
         
        // Add additional guards for the target.
        int guardCount = 3 + EClass.rnd(difficulty);
        for (int i = 0; i < guardCount; i++)
        {
            string guardType = (EClass.rnd(4) == 0) ? Constants.EliteGuardCharaId : Constants.BasicGuardCharaId;
            Chara guard = CharaGen.Create(guardType);
            guard.SetGlobal();
            guard.MoveZone(Zone, ZoneTransition.EnterState.RandomVisit);
        }
        
        // Add the Character to the target zone.
        chara.MoveZone(Zone, ZoneTransition.EnterState.RandomVisit);
        _ = new object[3] { difficulty, DangerLv, targetZoneUID };
    }
    
    private bool CountsAsQuestTarget(Chara c)
    {
        return c.uid == targetZoneUID;
    }

    public override void OnKillChara(Chara c)
    {
        if (CountsAsQuestTarget(c))
        {
            Complete();
        }
    }

    public override void OnFail()
    {
        Cleanup();
    }

    public override void OnComplete()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        Target?.Destroy();
    }
}