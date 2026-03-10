using BepInEx;
using HarmonyLib;

namespace WildCleanup
{
    internal static class ModInfo
    {
        internal const string Guid = "han.WildCleanup.mod";
        internal const string Name = "Temp Cleanup Mod";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class WildCleanup : BaseUnityPlugin
    {
        internal static WildCleanup? Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            var harmony = new Harmony(ModInfo.Guid);
            harmony.PatchAll();
        }

        internal static void Log(object payload)
        {
            Instance!.Logger.LogInfo(payload);
        }
    }
}