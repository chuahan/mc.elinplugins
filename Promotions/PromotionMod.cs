using BepInEx;
using HarmonyLib;
namespace PromotionMod;

internal static class ModInfo
{
    internal const string Guid = "han.elinplugins.promotion";
    internal const string Name = "PromotionMod";
    internal const string Version = "1.0.0";
}

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