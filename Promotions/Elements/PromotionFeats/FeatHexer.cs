using System;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Hexer;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Practitioners of the forbidden arts. Hexers have learned creative new ways to cause pain and suffering.
/// Hexers focus on weakening the enemies with curses and damage over time effects.
/// They specialize in applying debuffs, then exploiting those debuffs to deal damage.
/// Skill - Traumatize - Does Mind Damage equivalent to how many negative conditions are on the enemy. 10 turn cooldown. 20% Mana cost.
/// Skill - Blood Curse - Force applies one of the curses randomly at the cost of 10% life. Will prioritize curses you have not applied of the same tier that you roll.
/// Passive - Hexmaster - When applying spell damage or taking damage, there is a chance to apply a hex out of a pool.
/// Passive - Do not cite the deep magic to me - If you have active Debuffs on you, your curses apply at double power and consumes one of the debuffs.
/// </summary>
public class FeatHexer : PromotionFeat
{
    public override string PromotionClassId => Constants.HexerId;
    public override int PromotionClassFeatId => Constants.FeatHexer;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActTraumatizeId,
        Constants.ActBloodCurseId,
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "witch";
    }

    protected override void ApplyInternal()
    {
        // Casting
        // Regeneration
        // Mana Consumption
        //owner.Chara.elements.ModPotential(257, 30);
    }

    internal static List<string> CommonConditions = new List<string>
    {
        nameof(ConPoison),
        nameof(ConWeakness),
        nameof(ConFear),
        nameof(ConWeakResEle),
        nameof(ConNightmare),
    };

    internal static List<string> UncommonConditions = new List<string>
    {
        nameof(ConBurning),
        nameof(ConFreeze),
        nameof(ConParalyze),
        nameof(ConBlind),
        nameof(ConMalaise),
        nameof(ConBleed),
    };

    internal static List<string> RareConditions = new List<string>
    {
        nameof(ConParanoia),
        nameof(ConReapersCall),
        nameof(ConMalaise),
        nameof(ConCorruption),
    };
    
    public static Condition? ApplyCondition(Chara target, Chara caster, int power, bool force)
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
        } else if (EClass.rnd(10) != 0)
        {
            gachaString = "hexer_uncommon_gacha";
            inactiveConditions = UncommonConditions.Except(activeConditions).ToList();
            if (inactiveConditions.Count == 0) inactiveConditions = UncommonConditions;
        } else if (EClass.rnd(10) != 0)
        {
            gachaString = "hexer_rare_gacha";
            inactiveConditions = RareConditions.Except(activeConditions).ToList();
            if (inactiveConditions.Count == 0) inactiveConditions = RareConditions;
        }
        else
        {
            caster.SayRaw("hexer_legendary_gacha".langGame());
            return target.AddCondition<ConDeathSentense>(100, force: true);
        }

        if (inactiveConditions.Count == 0)
        {
            caster.SayRaw("hexer_failed_gacha".langGame());
            return null;
        }
        else
        {
            bool doublePower = false;
            if (casterDebuff != null)
            {
                casterDebuff.Kill();
                doublePower = true;
            }
            caster.SayRaw(gachaString.langGame());
            Condition hex = Condition.Create(inactiveConditions[rng.Next(inactiveConditions.Count)], doublePower ? power * 2 : power);
            return target.AddCondition(hex, force);
        }
    } 
}