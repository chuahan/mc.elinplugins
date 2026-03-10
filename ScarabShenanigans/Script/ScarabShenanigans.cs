using BepInEx;
using HarmonyLib;

namespace ScarabShenanigans
{
    internal static class ModInfo
    {
        internal const string Guid = "han.stopscarab.mod";
        internal const string Name = "Cease thy shenanigans scarab";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class ScarabShenanigans : BaseUnityPlugin
    {
        internal static ScarabShenanigans? Instance { get; private set; }

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