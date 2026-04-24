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


        // Increment her progress on her own if she has at least 1 book turned in.
        if (lailahWorking != 1 && lailahDeciphering is < 100 and >= 1 && lailahManualDeciphered != 1)
        {
            EClass.player.dialogFlags["lailahDeciphering"] = lailahDeciphering + 1;
            if (EClass.player.dialogFlags["lailahDeciphering"] >= 100)
            {
                // If this reaches 100, go through all the hoops and whistles to start the timer like in PromoDramaExpansion.
                // Store the current time onto the player.
                EClass.player.dialogFlags["lailahDecipheringTimer"] = EClass.world.date.GetRaw() + 4320; // 3 Days, 1440 turns a day.
                EClass.player.dialogFlags["lailahManualDeciphered"] = 1;
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