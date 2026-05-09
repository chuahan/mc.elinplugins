using BepInEx;
using HarmonyLib;
namespace LandReroller;

internal static class ModInfo
{
    internal const string Guid = "han.landreroller.mod";
    internal const string Name = "Land Reroller";
    internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class LandReroller : BaseUnityPlugin
{
    internal static LandReroller? Instance { get; private set; }

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