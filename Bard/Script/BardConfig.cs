using BepInEx.Configuration;
namespace BardMod;

public class BardConfig
{
    internal static ConfigEntry<bool>? UseMusic { get; private set; }
    
    internal static void Load(ConfigFile config)
    {
        UseMusic = config.Bind(
            ModInfo.Name,
            "UseMusic",
            true,
            "Set this to false to not play any music when using bard abilities.");
    }
}