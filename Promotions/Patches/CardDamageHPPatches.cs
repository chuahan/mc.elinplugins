using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Harbinger;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Card))]
public class CardDamageHPPatches
{
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

            if (target.GetCondition<ConHarbingerMiasma>() != null && origin.isChara && origin.Evalue(Constants.FeatHarbinger) > 0)
            {
                // Harbinger - Every active Miasma on the target boosts damage from Harbingers by 5%.
                int miasmaCount = target.conditions.Count(con => con is ConHarbingerMiasma);
                dmg *= (100 + miasmaCount * 5) / 100;
            }
        }
        return true;
    }
}

