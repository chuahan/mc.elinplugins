using BepInEx;
using HarmonyLib;
namespace PostalOptions;

internal static class ModInfo
{
    internal const string Guid = "han.postaloptions.mod";
    internal const string Name = "Postal Options";
    internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class PostalOptions : BaseUnityPlugin
{
    internal static PostalOptions? Instance { get; private set; }

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