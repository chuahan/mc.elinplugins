using Newtonsoft.Json;
using PromotionMod.Common;
using PromotionMod.Trait.QuestTraits;
using UnityEngine;
namespace PromotionMod.Source.QuestEventSystem.QuestTypes;

/// <summary>
///     Covert Ops: Abduction
///     The player is given a special monster ball.
///     The target is spawned in a target town along with a group of guards. They will have a debuff that prevents them
///     from dying.
///     However, when engaged in combat, every 3 turns they will spawn additional enemies around them.
///     - Having 75% or higher HP will cause them to spawn 2 guards per turn.
///     - Having 50% or lower HP will cause them to spawn 4 guards per turn.
///     - Having 25% or lower HP will cause them to spawn 8 guards per turn.
///     Once the enemy is captured, the ball must be returned to the quest giver for completion.
/// </summary>
public class QuestCovertOpAbduct : Quest
{
    [JsonProperty] public int targetCharaUID;

    [JsonProperty] public int TargetLvl;

    [JsonProperty] public int targetZoneUID;

    public override int RangeDeadLine => 30;

    public override int KarmaOnFail => 0;

    public override bool FameContent => true;

    public Zone? Zone => RefZone.Get(targetZoneUID);

    private Chara? Target => game.cards.Find(targetCharaUID);

    public override string RefDrama1 => Target?.Name ?? "???";
    public override string RefDrama2 => Zone?.Name ?? "???";

    public override string RefDrama3 => person?.Name ?? "???";

    public override int BaseMoney => source.money + EClass.curve(DangerLv, 20, 20) * 10;

    public override int FameOnComplete => base.FameOnComplete * 2;

    public override string GetTextProgress()
    {
        return "covertops_abduction_progress".langGame(RefDrama1, RefDrama2);
    }

    public override void OnInit()
    {
        base.OnInit();
        targetZoneUID = world.region.GetRandomTown()?.uid ?? 0;
    }

    public override void OnStart()
    {
        if (Zone == null)
        {
            // If this quest was created without a Zone, null it.
            game.quests.Remove(this);
            if (chara is { quest: not null } && chara.quest.uid == uid) chara.quest = null;
            return;
        }

        // Scale the Target
        float num = difficulty * 0.25f + EClass.rndf(0.25f);
        float num2 = (DangerLv + EClass.rnd(5)) * num + 1f;
        int scaledLevel = Mathf.RoundToInt(EClass.rndf(num2 * 0.25f) + num2 * 0.75f);
        Chara target = CharaGen.Create("capture_target", scaledLevel);
        targetCharaUID = chara.uid;
        target.SetGlobal();

        // Add additional guards for the target.
        int guardCount = 3 + EClass.rnd(difficulty);
        for (int i = 0; i < guardCount; i++)
        {
            string guardType = EClass.rnd(4) == 0 ? Constants.EliteGuardCharaId : Constants.BasicGuardCharaId;
            Chara guard = CharaGen.Create(guardType);
            guard.SetGlobal();
            guard.MoveZone(Zone, ZoneTransition.EnterState.RandomVisit);
        }

        // Add the Character to the target zone.
        chara.MoveZone(Zone, ZoneTransition.EnterState.RandomVisit);
        _ = new object[3]
        {
            difficulty,
            DangerLv,
            targetZoneUID
        };

        // Provide the player with a Bokuto if they don't have one.
        if (pc.things.Find("sword_bokuto") == null)
        {
            Thing standardIssueBokuto = ThingGen.Create("sword_bokuto", lv: scaledLevel);
            pc.things.Add(standardIssueBokuto);
        }

        // Provide the player with the snagball.
        pc.things.Add(ThingGen.Create("snagball", lv: scaledLevel));
    }

    private bool CountsAsQuestTarget(Chara c)
    {
        return c.uid == targetZoneUID;
    }

    public override void OnKillChara(Chara c)
    {
        // Target must be captured, not killed.
        if (CountsAsQuestTarget(c))
        {
            Fail();
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

    public override void OnGiveItem(Chara c, Thing t)
    {
        if (c.id == person.id)
        {
            if (t.id == "snagball" && (t.trait as TraitSnagBall).CapturedTarget.uid == targetCharaUID)
            {

            }
        }
        base.OnGiveItem(c, t);
    }
}