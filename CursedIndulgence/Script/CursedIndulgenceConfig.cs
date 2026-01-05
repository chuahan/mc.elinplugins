using BepInEx.Configuration;
namespace CursedIndulgence
{
    public static class CursedIndulgenceConfig
    {
        internal static ConfigEntry<int>? CursedAmount { get; private set; }
        internal static ConfigEntry<int>? DoomedAmount { get; private set; }
        internal static ConfigEntry<int>? BlessedAmount { get; private set; }

        internal static void Load(ConfigFile config)
        {
            CursedAmount = config.Bind(
                ModInfo.Name,
                "CursedAmount",
                -20,
                "How much karma to lose for cursed letters of indulgence");
            DoomedAmount = config.Bind(
                ModInfo.Name,
                "DoomedAmount",
                -40,
                "How much karma to lose for doomed letters of indulgence");
            BlessedAmount = config.Bind(
                ModInfo.Name,
                "BlessedAmount",
                40,
                "How much karma to gain for blessed letters of indulgence");
        }
    }
}

