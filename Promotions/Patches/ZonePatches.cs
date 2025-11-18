using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Zone))]
public class ZonePatches
{
    private const float HarbingerChance = 0.02F;

    [HarmonyPatch(nameof(Zone.OnGenerateMap))]
    [HarmonyPostfix]
    internal static void ZoneOnGeneratePostfix(Zone __instance)
    {
        // Maiar - Try to spawn Candlebearers and Darklings.
        // Can only spawn in Zone_Dungeon or Zone_Field
        if (EClass.pc.Evalue(Constants.FeatMaia) > 0 && __instance.DangerLv >= 12 && (__instance is Zone_Field || __instance is Zone_Dungeon))
        {
            // Try to generate a Candlebearer
            if (HarbingerChance <= EClass.rndf(1f))
            {
                Chara candlebearer = CharaGen.Create(Constants.CandlebearerCharaId);

                // Being Good or an Enlightened Maia will cause Candlebearers to become friendly
                if (EClass.pc.HasElement(1270) || EClass.pc.Evalue(Constants.FeatMaiaEnlightened) > 0)
                {
                    candlebearer.SetHostility(Hostility.Friend);
                }

                // Being Evil or a Corrupted Maia will cause Candlebearers to become hostile
                if (EClass.pc.HasElement(1271) || EClass.pc.Evalue(Constants.FeatMaiaCorrupted) > 0)
                {
                    candlebearer.SetHostility(Hostility.Enemy);
                }

                EClass._zone.AddCard(candlebearer, __instance.GetSpawnPos(SpawnPosition.Random, 10000));
                Msg.Say("sign_candlebearer");
            }

            // Try to generate a Darkling
            if (HarbingerChance <= EClass.rndf(1f))
            {
                Chara darkling = CharaGen.Create(Constants.DarklingCharaId);

                // Being Evil or a Corrupted Maia will cause Darklings to become friendly.
                if (EClass.pc.HasElement(1271) || EClass.pc.Evalue(Constants.FeatMaiaCorrupted) > 0)
                {
                    darkling.SetHostility(Hostility.Friend);
                }

                // Being Good or an Enlightened Maia will cause Darklings to become be hostile.
                if (EClass.pc.HasElement(1270) || EClass.pc.Evalue(Constants.FeatMaiaEnlightened) > 0)
                {
                    darkling.SetHostility(Hostility.Enemy);
                }

                EClass._zone.AddCard(darkling, __instance.GetSpawnPos(SpawnPosition.Random, 10000));
                Msg.Say("sign_darkling");
            }
        }

        // Druids - If there is a druid in the party, animals and plantlife will become friendly.
        if (EClass.pc.party.members.Any(c => c.Evalue(Constants.FeatDruid) > 0))
        {
            foreach (Chara chara in EClass._map.charas.Where(chara => !chara.IsGlobal && chara is { hostility: < Hostility.Neutral, OriginalHostility: < Hostility.Friend })
                             .Where(chara => chara.IsAnimal || chara.IsPlant))
            {
                chara.hostility = Hostility.Friend;
            }
        }
    }
}