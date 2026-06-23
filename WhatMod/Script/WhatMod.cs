using BepInEx;
using HarmonyLib;
namespace WhatMod
{
    internal static class ModInfo
    {
        internal const string Guid = "han.whatmod.mod";
        internal const string Name = "What Mod";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class WhatMod : BaseUnityPlugin
    {
        internal static WhatMod? Instance { get; private set; }

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