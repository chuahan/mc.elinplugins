using BepInEx;
using HarmonyLib;
namespace SpiritWeapons;

internal static class ModInfo
{
    internal const string Guid = "han.spiritweapons.mod";
    internal const string Name = "Spirit Weapons";
    internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class SpiritWeapons : BaseUnityPlugin
{
    internal static SpiritWeapons? Instance { get; private set; }

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