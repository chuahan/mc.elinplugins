using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using Cwl.Helper.Unity;
namespace BardMod.Stats.BardSongConditions;

public class ConAfterTempestSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }

        // TODO: Play FX
        // TODO: Play SFX
        Effect lightningBallEffect = Effect.Get("Element/ball_Lightning");
        lightningBallEffect.Play(owner.pos).Flip(owner.pos.x > CC.pos.x);
        owner.PlaySound("spell_ball");

        int damage = HelperFunctions.SafeDice(Constants.BardFinaleAfterTempestName, power);
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos, 4f, owner, false, true);
        foreach (Chara target in targets)
        {
            target.AddCondition<ConLightningSunder>(power);
            // target.DamageHP(dmg: damage, ele: Constants.EleLightning, eleP: 100, attackSource: AttackSource.Condition, origin: owner);
            BardCardPatches.CachedInvoker.Invoke(
                target,
                new object[] { damage, Constants.EleLightning, 100, AttackSource.Condition, owner }
            );

            if (GodBlessed &&
                !owner.IsMinion &&
                EClass.rnd(4) > 3 && // 75%
                (EClass.rnd(owner.CHA + 10) > target.LV || EClass.rnd(10) == 0) && // Cha Requirement with 1/10 chance to bypass.
                target.race.IsMachine &&
                target.CanBeTempAlly(owner))
            {
                CoroutineHelper.Deferred(() => target.Say("dominate_machine", owner, target));
                target.PlayEffect("boost");
                target.PlaySound("boost");
                target.ShowEmo(Emo.love);
                target.lastEmo = Emo.angry;
                target.MakeMinion(owner.IsPCParty ? pc : owner);
            }

            Stacks++;
        }

        base.Tick();
    }

    public override void OnRemoved()
    {
        // TODO: Play FX
        // TODO: Play SFX
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos, 5f, Caster, true, false);
        int overguard = HelperFunctions.SafeMultiplier(Stacks, 1 + power / 10.0F);
        foreach (Chara target in targets)
        {
            if (target.HasCondition<ConOverguard>())
            {
                ConOverguard existingOverguard = target.GetCondition<ConOverguard>();
                existingOverguard.AddOverguard(overguard);
            }
            else
            {
                target.AddCondition<ConOverguard>(overguard);
            }
        }
        base.OnRemoved();
    }

    public override void OnWriteNote(List<string> list)
    {
        string plural = Stacks > 1 ? "s" : "";
        list.Add("hintAfterTempestSong".lang(Stacks.ToString(), plural));
    }
}