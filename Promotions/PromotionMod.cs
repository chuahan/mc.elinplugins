using BepInEx;
using HarmonyLib;
namespace PromotionMod;

internal static class ModInfo
{
    internal const string Guid = "han.elinplugins.promotion";
    internal const string Name = "PromotionMod";
    internal const string Version = "1.0.0";
}

/// <summary>
///     TODO LIST (Excluding the ones in each Promotion Class)
///     TODO (P1) Implement acquisition method for Promotion Manuals. This likely includes Lailah's quest.
///     TODO (P2) Implement Rapiers: Flurry and Pursuit inherent.
///     TODO (P2) Implement Lances: Unique Weapon Mod that increases damage based on speed.
///     TODO (P2) Implement Sniper Rifles: Increased Crit / Accuracy. Low Mag size. High Effective Range.
///     TODO (P2) Implement other Melee Weapons: Chakrams, Daggers, more Lances, Axes.
///     TODO (P2) Implement Brave Weapons: Come with High Flurry
///     TODO (P2) Implement Killer Weapons: Come with Chance to Triple Damage.
///     TODO (P2) Implement Maia Race (Spawns of Lantern Bearers and Darklings, Patches for the leveling mechanic,
///     abilities)
/// </summary>
[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class PromotionMod : BaseUnityPlugin
{

    internal const bool Debug = true;
    internal static PromotionMod? Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Harmony harmony = new Harmony(ModInfo.Guid);
        harmony.PatchAll();
    }

    internal static void Log(object payload)
    {
        Instance!.Logger.LogInfo(payload);
    }
}