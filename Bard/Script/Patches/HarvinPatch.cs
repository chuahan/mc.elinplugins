using BardMod.Common;
using HarmonyLib;
namespace BardMod.Patches;

[HarmonyPatch(typeof(ActEffect))]
internal class HarvinPatch : EClass
{
    // Yes this is a patch to stop Harvins from growing taller.
    [HarmonyPatch(nameof(ActEffect.Proc), typeof(EffectId), typeof(int), typeof(BlessedState), typeof(Card), typeof(Card), typeof(ActRef))]
    [HarmonyPrefix]
    internal static bool OnMilkEffect(EffectId id, int power, BlessedState state, Card cc, Card tc, ActRef actRef = default(ActRef))
    {
        bool blessed = state >= BlessedState.Blessed;
        if (id == EffectId.DrinkMilk && blessed)
        {
            Chara target = cc.Chara;
            if (target.Evalue(Constants.FeatHarvin) > 0)
            {
                Msg.Say("wishFail");
                return false;
            }
        }
        return true;
    }
}