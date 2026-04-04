using BepInEx;
using HarmonyLib;

namespace BuildingTools
{
    internal static class ModInfo
    {
        internal const string Guid = "han.buildtools.mod";
        internal const string Name = "Building Tools";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class BuildingTools : BaseUnityPlugin
    {
        internal static BuildingTools? Instance { get; private set; }

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