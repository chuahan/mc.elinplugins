using BepInEx.Configuration;
namespace RerollPlatMerchant
{
    public static class RerollPlatMerchantConfig
    {
        internal static ConfigEntry<int>? RerollAmount { get; private set; }


        internal static void Load(ConfigFile config)
        {
            RerollAmount = config.Bind(
                ModInfo.Name,
                "RerollAmount",
                5,
                "How much influence is needed to reroll");
        }
    }
}

