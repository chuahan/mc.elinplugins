using BepInEx;
using HarmonyLib;

namespace RerollPlatMerchant
{
    internal static class ModInfo
    {
        internal const string Guid = "han.RerollPlatMerchant.mod";
        internal const string Name = "Reroll the Plat Merchant";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class RerollPlatMerchant : BaseUnityPlugin
    {
        internal static RerollPlatMerchant? Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            var harmony = new Harmony(ModInfo.Guid);
            
            RerollPlatMerchantConfig.Load(Config);
            harmony.PatchAll();
        }

        internal static void Log(object payload)
        {
            Instance!.Logger.LogInfo(payload);
        }
    }
}