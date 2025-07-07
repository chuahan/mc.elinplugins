using BardMod.Common;
using HarmonyLib;
namespace BardMod.Patches;

[HarmonyPatch(typeof(GoalCombat))]
internal class BardGoalCombatPatches
{
    // Patch to debug.
    [HarmonyPatch(typeof(GoalCombat), nameof(GoalCombat.TryUseAbility))]
    [HarmonyPrefix]
    internal static bool OnTryUseAbility(GoalCombat __instance, int dist, bool beforeMove = false)
    {
        if (__instance.owner != null && (__instance.owner.id == Constants.NiyonCharaId || __instance.owner.id == Constants.SelenaCharaId))
        {
            //DEBUG BEGIN.
            if (__instance.abilities.Count != 0)
            {
                foreach (GoalCombat.ItemAbility ability in __instance.abilities)
                {
                    Act act = ability.act;
                    ability.priority = 0;
                    ability.tg = null;
                    ability.pt = false;
                    SourceElement.Row s = act.source;
                    string text = s.abilityType[0];
                    // Why does this go null?
                    if (__instance.owner == null)
                    {
                        continue;
                    }

                    var tactics = __instance.owner.tactics;
                    Act.Cost cost = ability.act.GetCost(__instance.owner);
                    if (ability.act.CanPerform(__instance.owner, ability.tg ?? __instance.tc) &&
                        __instance.owner.UseAbility(ability.act, ability.tg ?? __instance.tc, null, (ability.act.HaveLongPressAction && ability.pt) || ability.aiPt))
                    {
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                return true;
            }
        }
        
        return true;
    }
}