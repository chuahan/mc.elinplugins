using BepInEx;
using HarmonyLib;
namespace NierMod;

internal static class ModInfo
{
    internal const string Guid = "mc.elinplugins.nier";
    internal const string Name = "Misery's Beloved Nier";
    internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class NierMod : BaseUnityPlugin
{
    internal static NierMod? Instance { get; private set; }

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