using BepInEx;
using HarmonyLib;

namespace LivestockTweaks
{
    internal static class ModInfo
    {
        internal const string Guid = "han.LivestockTweaks.mod";
        internal const string Name = "Additional Livestock Configurations";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class LivestockTweaks : BaseUnityPlugin
    {
        internal static LivestockTweaks? Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            var harmony = new Harmony(ModInfo.Guid);

            LivestockTweaksConfig.Load(Config);
            harmony.PatchAll();
        }

        internal static void Log(object payload)
        {
            Instance!.Logger.LogInfo(payload);
        }
    }
}