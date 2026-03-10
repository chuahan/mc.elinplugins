using System.Linq;
using Cwl.Helper;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Source;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(GameDate))]
public class GameDatePatches
{
    [HarmonyPatch(nameof(GameDate.AdvanceDay))]
    [HarmonyPostfix]
    internal static void GameDate_SendLetterPatch(GameDate __instance)
    {
        EClass.player.dialogFlags.TryGetValue("lailahWorking", out int lailahWorking);
        EClass.player.dialogFlags.TryGetValue("lailahSentLetter", out int lailahSentLetter);
        if (lailahSentLetter == 0 && lailahWorking == 1)
        {
            EClass.player.dialogFlags.TryGetValue("lailahDecipheringTimer", out int lailahDecipheringTimer);
            if (lailahDecipheringTimer >= 1 && EClass.world.date.GetRaw() > lailahDecipheringTimer)
            {
                Thing letter = ThingGen.Create("letter");
                letter.SetStr(53, "lailah_decipheringcomplete_letter");
                EClass.world.SendPackage(letter);
                EClass.player.dialogFlags["lailahSentLetter"] = 1;
            }   
        }
    }
}