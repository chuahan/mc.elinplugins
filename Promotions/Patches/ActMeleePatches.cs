using System;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActMelee))]
public class ActMeleePatches
{
    [HarmonyPatch(nameof(ActMelee.Attack), typeof(float))]
    [HarmonyPrefix]
    public static bool AttackPrefixPatch(ActMelee __instance, ref float dmgMulti)
    {
        // Sentinel - If the target is a Sentinel with a Shield, they have a chance to block the blow, reducing the damage.
        if (Act.TC.Chara.MatchesPromotion(Constants.FeatSentinel) &&
            Act.TC.Chara.body.GetAttackStyle() == AttackStyle.Shield &&
            !Act.TC.IsDisabled && !Act.TC.IsRestrainedResident)
        {
            // Shield Skill * 2 + 50
            int blockChance = Act.TC.Evalue(123) * 2 + 50;
            if (EClass.rnd(blockChance) > 100)
            {
                // Maximum damage reduction of 85%. Applied as a damage multiplier.
                float blockPercent = (100 - Math.Min(FeatSentinel.GetShieldPower(Act.TC.Chara), 85)) / 100F;
                dmgMulti = blockPercent;
            }
        }

        // Sentinel - If the attacker is a Sentinel in Rage Stance, add a multiplier of damage based off of their PV difference.
        if (Act.CC.MatchesPromotion(Constants.FeatSentinel) && Act.CC.HasCondition<StanceRage>())
        {
            // Every 5 PV adds 1% more damage.
            StanceRage stance = Act.CC.GetCondition<StanceRage>();
            dmgMulti += stance.power / 5F;
        }
        
        // Vantage will allow the defender to counterattack first immediately when attacked.
        Chara originalAttacker = Act.CC;
        Card originalDefender = Act.TC;
        if (originalDefender is { isChara: true, ExistsOnMap: true, IsRestrainedResident: false, IsDisabled: false} &&
            originalDefender != originalAttacker &&
            ACT.Melee.CanPerform(originalDefender.Chara, originalAttacker) &&
            (!originalDefender.IsPCFactionOrMinion || !EClass._zone.isPeace) &&
            !originalAttacker.HasCondition<ConFear>())
        {
            // Vantage - Triggers when 50% or lower HP.
            if (Act.TC.HasElement(Constants.FeatVantageId) &&
                Act.TC.hp <= Act.TC.MaxHP / 2 &&
                Act.CC.IsHostile(Act.TC.Chara))
            {
                Act.TC.Say("vantage_activation".langGame(Act.TC.NameSimple));
                Act.TC.PlaySound("parry");
                if (!HelperFunctions.NihilActivated(Act.CC))
                {
                    new ActMeleeCounter().Perform(originalDefender.Chara, originalAttacker);
                }
            }

            // Vantage+ - Triggers automatically
            if (Act.TC.HasElement(Constants.FeatVantagePlusId) &&
                Act.CC.IsHostile(Act.TC.Chara))
            {
                Act.TC.Say("vantage_activation".langGame(Act.TC.NameSimple));
                Act.TC.PlaySound("parry");
                if (!HelperFunctions.NihilActivated(Act.CC))
                {
                    new ActMeleeCounter().Perform(originalDefender.Chara, originalAttacker);
                }
            }
        }

        return true;
    }

    [HarmonyPatch(nameof(ActMelee.CanPerform))]
    [HarmonyPrefix]
    public static bool CanPerformPatch(ref bool __result)
    {
        if (Act.CC.HasCondition<ConDisable>())
        {
            __result = false;
            return false;
        }
        return true;
    }
}