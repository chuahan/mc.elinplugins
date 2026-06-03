using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using NierMod.Common;
using NierMod.Stats;

namespace NierMod.Patches
{
    [HarmonyPatch(typeof(Chara))]
    internal class CharaPatches : EClass
    {
        /// <summary>
        /// When Nier dies, Applies Eternal Vow buff to the Player Character if applicable,
        /// and Unfinished Business to every other character on the map.
        /// </summary>
        [HarmonyPatch(nameof(Chara.Die))]
        [HarmonyPrefix]
        internal static void EternalVowOnDeath(Chara __instance)
        {
            if (__instance.Evalue(Constants.eternalVowFeatId) > 0)
            {
                bool nierState = NierModHelpers.IsNierAndMarried(__instance);
                if (__instance.IsPCFaction)
                {
                    if (__instance.IsPCParty)
                    {
                        foreach (Chara target in _map.charas)
                        {
                            if (target == __instance)
                            {
                                continue;
                            }
                            else if (target.IsPC)
                            {
                                if (__instance.affinity.value > 100 || nierState)
                                {
                                    if (!EClass.pc.HasCondition<ConEternalVow>())
                                    {
                                        EClass.pc.AddCondition<ConEternalVow>(1 + (nierState ? 1 : 0));
                                    }
                                }
                                else
                                {
                                    if (!target.HasCondition<ConUnfinishedBusiness>())
                                    {
                                        target.AddCondition<ConUnfinishedBusiness>();   
                                    }
                                }
                            }
                            else
                            {
                                if (!target.HasCondition<ConUnfinishedBusiness>())
                                {
                                    target.AddCondition<ConUnfinishedBusiness>();   
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (__instance.IsPCParty || __instance.IsPCFactionMinion)
                {
                    foreach (Chara member in EClass.pc.party.members)
                    {
                        if (member.Evalue(Constants.loversDeathsDemiseFeatId) > 0 &&
                            member.IsAliveInCurrentZone &&
                            member.id != __instance.id)
                        {
                            if (!member.HasCondition<ConLoversDeathsDemise>())
                            {
                                member.AddCondition<ConLoversDeathsDemise>(1);
                            }
                            else
                            {
                                int newStacks = member.GetCondition<ConLoversDeathsDemise>().GetStacks() + 1;
                                member.AddCondition<ConLoversDeathsDemise>(newStacks);
                            }
                        }
                    }
                }
                return;
            }
        }
        
        /// <summary>
        /// When Nier is in the Party and has befriended you, ensure Player has the MyBeloved buff.
        /// </summary>
        [HarmonyPatch(nameof(Chara.Tick))]
        [HarmonyPrefix]
        internal static void WhileBelovedInParty(Chara __instance)
        {
            if (!__instance.IsPC) // Tick on Player.
            {
                return;
            }

            foreach (Chara cc in EClass.pc.party.members)
            {
                if (cc.Evalue(Constants.myBelovedFeatId) > 0 && cc.IsAliveInCurrentZone)
                {
                    bool nierState = NierModHelpers.IsNierAndMarried(cc);
                    if (cc.affinity.value > 100 || nierState)
                    {
                        var belovedPower = 1 + (nierState ? 1 : 0);
                        var condition = pc.GetCondition<ConMyBeloved>() ?? pc.AddCondition<ConMyBeloved>(belovedPower);
                        if (condition.power != belovedPower) // Check if Nier's state has changed, strengthening the buff.
                        {
                            EClass.pc.AddCondition<ConMyBeloved>(belovedPower);
                        }
                        
                        if (condition.value <= 1) // Else just tick it.
                        {
                            continue;
                        }

                        condition?.Mod(1);
                    }

                    break;
                }
            }
        }
        
        /// <summary>
        /// Change Nier's portrait between normal and wedding based off of flag from dialog.
        /// </summary>
        [HarmonyPatch(nameof(Chara.GetIdPortrait))]
        [HarmonyPostfix]
        internal static void NierChangePortrait(Chara __instance, ref string __result)
        {
            if (__instance.id == "nier")
            {
                if (__instance.idSkin != 2)
                {
                    __result = "UN_nier";
                }
                else
                {
                    __result = "UN_nier_wedding";    
                }
            }
        }
    }
}
