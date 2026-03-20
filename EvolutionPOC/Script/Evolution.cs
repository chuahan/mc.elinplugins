using BepInEx;
using HarmonyLib;

namespace Evolution
{
    internal static class ModInfo
    {
        internal const string Guid = "han.evolution.mod";
        internal const string Name = "Evolution Framework";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class Evolution : BaseUnityPlugin
    {
        internal static Evolution? Instance { get; private set; }

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