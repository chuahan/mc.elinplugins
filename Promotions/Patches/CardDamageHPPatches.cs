using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionAbilities.Hexer;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Harbinger;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardDamageHPPatches
{
    internal static List<AttackSource> ActiveAttackSources = new List<AttackSource>
    {
        AttackSource.Melee,
        AttackSource.Range,
        AttackSource.Throw,
        AttackSource.MagicSword,
        AttackSource.Shockwave,
        AttackSource.None
    };

    internal static MethodInfo TargetMethod()
    {
        return AccessTools.GetDeclaredMethods(typeof(Card))
                .Where(mi => mi.Name == nameof(Card.DamageHP))
                .OrderByDescending(mi => mi.GetParameters().Length)
                .First();
    }

    public static readonly FastInvokeHandler CachedInvoker = MethodInvoker.GetHandler(TargetMethod(), true);

    [HarmonyPrefix]
    internal static bool OnDamageHP(Card __instance, ref int dmg, ref int ele, ref int eleP, AttackSource attackSource, Card origin, bool showEffect, Thing weapon)
    {
        if (__instance.isChara)
        {
            Chara target = __instance.Chara;
            // Berserker - Berserkers under the effect of bloodlust will automatically try to retaliate against Melee Attacks
            if (target.GetCondition<ConBloodlust>() != null && attackSource == AttackSource.Melee || attackSource == AttackSource.MagicSword && origin is { isChara: true })
            {
                new ActMelee().Perform(target, origin, origin.pos);
            }

            // Harbinger - Every active Miasma on the target boosts damage from Harbingers by 5%.
            if (target.GetCondition<ConHarbingerMiasma>() != null && origin.isChara && origin.Evalue(Constants.FeatHarbinger) > 0)
            {
                int miasmaCount = target.conditions.Count(con => con is ConHarbingerMiasma);
                dmg *= (100 + miasmaCount * 5) / 100;
            }

            // Headhunter - Damage is increased against higher quality enemies.
            if (target.source.quality >= 3 && origin.isChara && origin.Evalue(Constants.FeatHeadhunter) > 0)
            {
                dmg = HelperFunctions.SafeMultiplier(dmg, 1.25F);
            }
            
            // Hexer - When applying spell damage there is a 10% chance to force apply a hex out of a pool.
            if (origin.Evalue(Constants.FeatHexer) > 0 && attackSource == AttackSource.None)
            {
                if (EClass.rnd(10) == 0)
                {
                    origin.SayRaw("hexer_hextouch");
                    FeatHexer.ApplyCondition(__instance.Chara, origin.Chara, 100, true);
                }
            }

            // Hexer - When taking damage, there is a 10% chance to force apply a hex out of a pool.
            if (target.Evalue(Constants.FeatHexer) > 0 && ActiveAttackSources.Contains(attackSource))
            {
                if (EClass.rnd(10) == 0)
                {
                    origin.SayRaw("hexer_revenge");
                    FeatHexer.ApplyCondition(origin.Chara, __instance.Chara, 100, true);
                }
            }
        }
        return true;
    }
}

