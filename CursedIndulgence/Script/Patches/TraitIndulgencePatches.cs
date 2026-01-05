
using HarmonyLib;

namespace CursedIndulgence.Patches
{
    [HarmonyPatch(typeof(TraitIndulgence))]
    internal class TraitIndulgencePatches : EClass
    {
        [HarmonyPatch(nameof(TraitIndulgence.OnRead))]
        [HarmonyPrefix]
        internal static bool OnReadPatch(TraitIndulgence __instance, Chara c)
        {
            switch (__instance.owner.blessedState)
            {
                case BlessedState.Blessed:
                    c.PlaySound("holyveil");
                    c.PlayEffect("holyveil");
                    Msg.Say("indulgence", c);
                    if (c.IsPC)
                    {
                        EClass.player.ModKarma(CursedIndulgenceConfig.BlessedAmount != null ? CursedIndulgenceConfig.BlessedAmount.Value : 40);
                    }
                    __instance.owner.ModNum(-1);
                    break;
                case BlessedState.Normal:
                    return true; // Take the normal route.
                case BlessedState.Cursed:
                    c.PlaySound("curse3");
                    c.PlayEffect("curse");
                    Msg.Say("uncurseEQ_curse", c);
                    if (c.IsPC)
                    {
                        EClass.player.ModKarma(CursedIndulgenceConfig.CursedAmount != null ? CursedIndulgenceConfig.CursedAmount.Value : -20);
                    }
                    __instance.owner.ModNum(-1);
                    break;
                case BlessedState.Doomed:
                    c.PlaySound("curse3");
                    c.PlayEffect("curse");
                    Msg.Say("uncurseEQ_curse", c);
                    if (c.IsPC)
                    {
                        EClass.player.ModKarma(CursedIndulgenceConfig.DoomedAmount != null ? CursedIndulgenceConfig.DoomedAmount.Value : -40);
                    }
                    __instance.owner.ModNum(-1);
                    break;
            }

            return false;
        }
    }
}
