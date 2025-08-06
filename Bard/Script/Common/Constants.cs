namespace BardMod.Common;

public class Constants
{

    public enum BardMotif
    {
        // None
        None,

        // Elemental Motifs
        Wind,
        Water,
        Lightning,
        Flame,
        Flower,
        Vibration,
        Light,
        Darkness,

        // Unique Motifs
        Eternalism,
        Ethereal,
        Revenant,
        Tempest,
        Starry,
        Ephemeral,
        Moonchill
    }

    public enum BardSongTarget
    {
        Friendly,
        Enemy,
        Both,
        Self
    }

    public enum BardSongType
    {
        None,
        Verse,
        Chorus,
        Finale
    }

    // Song Durations
    public const int VerseSongDuration = 20;
    public const int ChorusSongDuration = 10;

    // Element strings
    public const int EleFire = 910;
    public const int EleCold = 911;
    public const int EleLightning = 912;
    public const int EleDarkness = 913;
    public const int EleMind = 914;
    public const int EleNether = 916;
    public const int EleSound = 917;
    public const int EleHoly = 919;
    public const int EleMagic = 921;
    public const int EleImpact = 925;
    public const int EleVoid = 926;

    public const int MusicSkill = 241;
    public const int ChaAttribute = 77;

    internal const int MaxBardPowerBuff = 3000;
    internal const float BardPowerSlope = 1.2f;

    // Chara Ids
    internal const string NiyonCharaId = "bard_niyon";
    internal const string SelenaCharaId = "bard_selena";
    internal const string PhantomFlutistCharaId = "bard_phantomflutist";
    internal const string WaterDancerCharaId = "bard_waterdancer";
    internal const string MalevolentReflectionCharaId = "bard_malevolentreflection";
    internal const string StarHandCharaId = "bard_starhand";

    // Feats
    internal const int FeatBardId = 89400;
    internal const int FeatSoulsingerId = 89401;
    internal const int FeatDuetPartner = 89402;
    internal const int FeatHarvin = 89403;
    internal const int FeatConstruct = 89404;
    internal const int FeatMysticMusician = 89405;
    internal const int FeatTimelessSong = 89406;

    // Verses
    internal const int BardStrengthSongId = 89101;
    internal const int BardSpeedSongId = 89102;
    internal const int BardHealingSongId = 89103;
    internal const int BardMagicSongId = 89104;
    internal const int BardGuardSongId = 89105;
    internal const int BardDishearteningSongId = 89106;
    internal const int BardChaosSongId = 89107;
    internal const int BardDisorientationSongId = 89108;
    internal const int BardWitheringSongId = 89109;
    internal const int BardSleepSongId = 89110;
    internal const int BardLuckSongId = 89111;
    internal const int BardVigorSongId = 89112;
    internal const int BardMirrorSongId = 89113;
    internal const int BardShellSongId = 89114;
    internal const int BardDisruptionSongId = 89115;
    internal const int BardScathingSongId = 89116;
    internal const int BardDrowningSongId = 89117;
    internal const int BardWitchHuntSongId = 89118;

    // Verses - Niyon
    internal const int BardNinnaNannaSongId = 89119;
    internal const int BardDefenduSongId = 89120;
    internal const int BardQualiaSongId = 89121;

    // Chorus
    internal const int BardPuritySongId = 89200;
    internal const int BardSlashSongId = 89201;
    internal const int BardKnockbackSongId = 89202;
    internal const int BardEchoSongId = 89203;
    internal const int BardDispelSongId = 89204;
    internal const int BardCheerSongId = 89205;
    internal const int BardTuningSongId = 89206;
    internal const int BardElementalSongId = 89207;

    // Finale
    internal const int BardFinaleSongId = 89100;

    // Enchantments
    internal const int BardWindSongEnc = 89300;
    internal const int BardWaterSongEnc = 89301;
    internal const int BardLightningSongEnc = 89302;
    internal const int BardFlameSongEnc = 89303;
    internal const int BardBlossomSongEnc = 89304;
    internal const int BardImpactSongEnc = 89305;
    internal const int BardLightSongEnc = 89306;
    internal const int BardDarkSongEnc = 89307;
    internal const int BardTravelerSongEnc = 89308;
    internal const int BardEternalEnc = 89309;
    internal const int BardRevenantEnc = 89310;
    internal const int BardTempestEnc = 89311;
    internal const int BardCapriccioEnc = 89312;
    internal const int BardPianissimoEnc = 89313;
    internal const int BardSolacetuneEnc = 89314;

    // Song Names
    internal const string BardStrengthSongName = "rise_and_roar";
    internal const string BardSpeedSongName = "dash_and_dance";
    internal const string BardHealingSongName = "vim_and_vitalize";
    internal const string BardMagicSongName = "weave_and_wield";
    internal const string BardGuardSongName = "shield_and_shelter";

    internal const string BardLuckSongName = "beckon_and_bestow";
    internal const string BardVigorSongName = "rouse_and_renew";
    internal const string BardMirrorSongName = "shine_and_shroud";
    internal const string BardShellSongName = "curve_and_cloak";

    internal const string BardDishearteningSongName = "diminish_and_dread";
    internal const string BardChaosSongName = "shatter_and_scatter";
    internal const string BardDisorientationSongName = "distort_and_daze";
    internal const string BardWitheringSongName = "whisper_and_wither";
    internal const string BardSleepSongName = "charm_and_cradle";

    internal const string BardDisruptionSongName = "flutter_and_falter";
    internal const string BardScathingSongName = "mock_and_maim";
    internal const string BardDrowningSongName = "sink_and_stifle";
    internal const string BardWitchHuntSongName = "sunder_and_suppress";

    internal const string BardNinnaNannaSongName = "ninna_nanna";
    internal const string BardDefenduSongName = "defendu";
    internal const string BardQualiaSongName = "qualia";

    internal const string BardPuritySongName = "chorus_purity";
    internal const string BardSlashSongName = "chorus_slash";
    internal const string BardKnockbackSongName = "chorus_shake";
    internal const string BardEchoSongName = "chorus_echoes";
    internal const string BardDispelSongName = "chorus_dispel";
    internal const string BardCheerSongName = "chorus_cheer";
    internal const string BardTuningSongName = "chorus_tune";
    internal const string BardElementalSongName = "chorus_wave";

    internal const string BardFinaleHollowSymphonyName = "hollow_symphony";
    internal const string BardFinaleLulwyStepName = "wind_goddess";
    internal const string BardFinaleAlluringDanceName = "alluring_dance";
    internal const string BardFinaleClearThunderName = "clear_thunder";
    internal const string BardFinaleFarewellFlamesName = "farewell_flames";
    internal const string BardFinaleEndlessBlossomsName = "endless_blossoms";
    internal const string BardFinaleUnshakingEarthName = "unshaking_earth";
    internal const string BardFinaleLonelyTearsName = "lonely_tears";
    internal const string BardFinaleAbyssReflectionName = "abyss_reflection";
    internal const string BardFinalePrismaticBridgeName = "prismatic_bridge";
    internal const string BardFinaleShimmeringDewName = "shimmering_dew";
    internal const string BardFinaleHeavensFallName = "heavens_fall";
    internal const string BardFinaleAfterTempestName = "after_tempest";
    internal const string BardFinaleShootingStarsName = "shooting_stars";
    internal const string BardFinaleEphemeralFlowersName = "ephemeral_flowers";
    internal const string BardFinaleMoonlitFlightName = "moonlit_flight";

    // Other Spells.
    internal const string NebulosaFrojdName = "nebulosa_frojd";
    internal const string VintergatanSvalaName = "vintergatan_svala";
    internal const string FlamingChordName = "flaming_chord";
    internal const string ThunderousTranspositionName = "thunderous_transposition";
    internal const string RhythmicPiercingName = "rhythmic_piercing";
}