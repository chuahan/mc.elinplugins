using BepInEx.Configuration;
using LivestockTweaks;
namespace LivestockTweaks;

public static class LivestockTweaksConfig
{
    internal static ConfigEntry<bool>? StayStill { get; private set; }
    internal static ConfigEntry<bool>? UseEquipment { get; private set; }
    internal static ConfigEntry<bool>? NoSleepBesideMe { get; private set; }

    internal static void Load(ConfigFile config)
    {
        StayStill = config.Bind(
            ModInfo.Name,
            "DefaultStayStillOnBaby",
            false,
            "Whether or not newly hatched characters will automatically stay still on creation.");
        UseEquipment = config.Bind(
            ModInfo.Name,
            "DefaultUseEquipmentOnBaby",
            false,
            "Whether or not newly hatched characters will automatically be set to fetch shared equipment.");
        NoSleepBesideMe = config.Bind(
            ModInfo.Name,
            "NoSleepBesideMe",
            false,
            "Prevents characters with the sleepBeside tag from automatically teleporting onto you to sleep.");
        
    }
}