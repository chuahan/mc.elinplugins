using BepInEx;
using HarmonyLib;

namespace CursedIndulgence
{
    internal static class ModInfo
    {
        internal const string Guid = "han.CursedIndulgence.mod";
        internal const string Name = "Cursed Indulgence Letters";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class CursedIndulgence : BaseUnityPlugin
    {
        internal static CursedIndulgence? Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            var harmony = new Harmony(ModInfo.Guid);

            CursedIndulgenceConfig.Load(Config);
            harmony.PatchAll();
        }

        internal static void Log(object payload)
        {
            Instance!.Logger.LogInfo(payload);
        }
    }
}