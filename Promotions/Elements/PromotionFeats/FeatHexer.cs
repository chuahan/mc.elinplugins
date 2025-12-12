using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Hexer;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     Practitioners of the forbidden arts. Hexers have learned creative new ways to cause pain and suffering.
///     Hexers focus on weakening the enemies with curses and damage over time effects.
///     They specialize in applying debuffs, then exploiting those debuffs to deal damage.
///     Skill - Traumatize - Does Mind Damage equivalent to how many negative conditions are on the enemy. 10 turn
///     cooldown. 20% Mana cost.
///     Skill - Blood Curse - Force applies one of the curses randomly at the cost of 10% life. Will prioritize curses you
///     have not applied of the same tier that you roll.
///     Skill - Revenge - Consumes a debuff on the Hexer and does damage based on its power against a target.
///     Must have a debuff on yourself to use.
///     Passive - Hexmaster - When applying spell damage or taking damage, there is a chance to apply a hex out of a pool.
///     Passive - Do not cite the deep magic to me - If you have active Debuffs on you, your curses apply at double power
///     and consumes one of the debuffs.
/// </summary>
public class FeatHexer : PromotionFeat
{

    internal static List<string> CommonConditions = new List<string>
    {
        nameof(ConPoison),
        nameof(ConWeakness),
        nameof(ConFear),
        nameof(ConWeakResEle),
        nameof(ConNightmare)
    };

    internal static List<string> UncommonConditions = new List<string>
    {
        nameof(ConBurning),
        nameof(ConFreeze),
        nameof(ConParalyze),
        nameof(ConBlind),
        nameof(ConMalaise),
        nameof(ConBleed)
    };

    internal static List<string> RareConditions = new List<string>
    {
        nameof(ConParanoia),
        nameof(ConReapersCall),
        nameof(ConMalaise),
        nameof(ConCorruption)
    };

    public override string PromotionClassId => Constants.HexerId;
    public override int PromotionClassFeatId => Constants.FeatHexer;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActTraumatizeId,
        Constants.ActBloodCurseId,
        Constants.ActRevengeId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActTraumatizeId, 50, false);
        c.ability.Add(Constants.ActBloodCurseId, 100, false);
        c.ability.Add(Constants.ActRevengeId, 50, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "witch";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }

    public static void ApplyCondition(Chara target, Chara caster, int power, bool force)
    {
        Random rng = new Random();
        Condition? casterDebuff = caster.conditions.FirstOrDefault(x => x.Type == ConditionType.Bad || x.Type == ConditionType.Debuff);
        List<string> activeConditions = target.conditions.Select(t => nameof(t)).ToList();
        List<string> inactiveConditions = new List<string>();
        string gachaString;
        if (EClass.rnd(4) != 0)
        {
            gachaString = "hexer_common_gacha";
            inactiveConditions = CommonConditions.Except(activeConditions).ToList();
            if (inactiveConditions.Count == 0) inactiveConditions = CommonConditions;
        }
        else if (EClass.rnd(10) != 0)
        {
            gachaString = "hexer_uncommon_gacha";
            inactiveConditions = UncommonConditions.Except(activeConditions).ToList();
            if (inactiveConditions.Count == 0) inactiveConditions = UncommonConditions;
        }
        else if (EClass.rnd(10) != 0)
        {
            gachaString = "hexer_rare_gacha";
            inactiveConditions = RareConditions.Except(activeConditions).ToList();
            if (inactiveConditions.Count == 0) inactiveConditions = RareConditions;
        }
        else
        {
            caster.Talk("hexer_legendary_gacha".langGame());
            target.AddCondition<ConDeathSentense>(100, true);
            return;
        }

        if (inactiveConditions.Count == 0)
        {
            caster.Talk("hexer_failed_gacha".langGame());
            return;
        }
        bool doublePower = false;
        if (casterDebuff != null)
        {
            casterDebuff.Kill();
            doublePower = true;
        }
        caster.Talk(gachaString.langGame());
        Condition hex = Condition.Create(inactiveConditions[rng.Next(inactiveConditions.Count)], doublePower ? power * 2 : power);

        if (!force)
        {
            ActEffect.ProcAt(EffectId.Debuff, doublePower ? power * 2 : power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = inactiveConditions[rng.Next(inactiveConditions.Count)]
            });
        }
        else
        {
            target.AddCondition(hex, force);
        }
    }
}