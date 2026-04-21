using HarmonyLib;
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

        EClass.player.dialogFlags.TryGetValue("lailahDeciphering", out int lailahDeciphering);
        EClass.player.dialogFlags.TryGetValue("lailahManualDeciphered", out int lailahManualDeciphered);


        // Increment her progress on her own.
        if (lailahWorking != 1 && lailahDeciphering < 100 && lailahManualDeciphered != 1)
        {
            EClass.player.dialogFlags["lailahDeciphering"] = lailahDeciphering + 1;
            if (EClass.player.dialogFlags["lailahDeciphering"] >= 100)
            {
                // TODO: If this reaches 100, go through all the hoops and whistles to start the timer like in PromoDramaExpansion.
            }
        }
        else if (lailahSentLetter == 0 && lailahWorking == 1)
        {
            EClass.player.dialogFlags.TryGetValue("lailahDecipheringTimer", out int lailahDecipheringTimer);
            // If her work is complete, send the letter and update the flag so this doesn't run again.
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