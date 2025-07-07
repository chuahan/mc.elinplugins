using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
using BardMod.Stats.BardSongConditions;
using HarmonyLib;

namespace BardMod.Patches;

[HarmonyPatch(typeof(AttackProcess))]
internal class BardAttackProcessPatches : EClass
{
    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPrefix]
    internal static bool OnPerform(AttackProcess __instance, int count, bool hasHit, float dmgMulti, bool maxRoll,
        bool subAttack)
    {
        if (__instance.TC.isChara)
        {
            Chara target = __instance.TC.Chara;
            
            // Steps of the Wind Goddess - Target has a chance of evading Ranged and Melee attacks completely.
            // Do not counterattack for training purposes.
            if (target.HasCondition<ConLulwyStepSong>() && !target.isRestrained)
            {
                ConLulwyStepSong conLulwyStepSong = target.GetCondition<ConLulwyStepSong>();
                bool windDodged = false;
                if (__instance.IsRanged)
                {
                    windDodged = EClass.rnd(100) <= conLulwyStepSong.CalcRangedDodge();
                }
                else
                {
                    windDodged = EClass.rnd(100) <= conLulwyStepSong.CalcMeleeDodge();
                }
                
                // True Dodge.
                // If the Bard worships Lulwy, the condition will counterattack and stun the attacker.
                if (windDodged)
                {
                    if (conLulwyStepSong.GodBlessed)
                    {
                        target.PlaySound("miss_arrow");
                        __instance.CC.PlaySound("whip");
                        int damage =  HelperFunctions.SafeDice(Constants.BardFinaleLulwyStepName, conLulwyStepSong.power);
                        Msg.Say("windsong_retaliate".langGame(target.NameSimple));
                        __instance.CC.DamageHP(damage, Constants.EleLightning, conLulwyStepSong.GetRetaliationPower() * 10, AttackSource.Condition);
                        __instance.CC.AddCondition<ConParalyze>(30 + conLulwyStepSong.GetRetaliationPower());
                        return false;
                    }
                    else
                    {
                        target.PlaySound("miss_arrow");
                        Msg.Say("windsong_protect".langGame(target.NameSimple));
                        return false;
                    }
                }
            }
            
            // Moonlit Flight of Dreams - When attacked, Moonlit stacks increase.
            if (target.HasCondition<ConMoonlitFlightSong>() && !target.isRestrained)
            {
                target.GetCondition<ConMoonlitFlightSong>().Stacks++;
            }
        }
        return true;
    }

    [HarmonyPatch(nameof(AttackProcess.CalcHit))]
    [HarmonyPostfix]
    internal static void OnCalcHit(AttackProcess __instance, ref bool __result)
    {
        if (!__result)
        {
            if (__instance.TC is { isChara: true })
            {
                if (__instance.TC.Chara.Evalue(Constants.FeatConstruct) > 0)
                {
                    if (!__instance.TC.Chara.HasCooldown(Constants.FeatConstruct))
                    {
                        // TODO: Add SFX
                        // TODO: Add FX
                        __instance.TC.Chara.AddCooldown(Constants.FeatConstruct, 15);
                        __instance.TC.Chara.AddCondition<ConMatrix>(1, force:true);
                    }
                }
            }
        }
    }
}