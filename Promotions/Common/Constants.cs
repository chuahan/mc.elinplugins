using System;
using System.Collections.Generic;
namespace PromotionMod.Common;

public class Constants
{

    internal const int SigmoidScalingMax = 3000;
    internal const float SigmoidScalingPowerSlope = 1.2f;
    
    public const int FeatMaiaEnlightened = 1;
    public const int FeatMaiaCorrupted = 1;

    public const int PromotionLevelRequirement = 20;
    public const string PromotionFeatFlag = "promoted";
    
    #region CWL Flags
    public const string IsPlayerFactionTrapFlag = "isPlayerFactionTrap";
    #endregion

    public const string DruidWarmSowTag = "druid_sow_warm";
    public const string DruidWrathSowTag = "druid_sow_wrath";

    #region Element Ids

    public const int EleFire = 910;
    public const int EleCold = 911;
    public const int EleLightning = 912;
    public const int EleDarkness = 913;
    public const int EleMind = 914;
    public const int ElePoison = 915;
    public const int EleNether = 916;
    public const int EleSound = 917;
    public const int EleNerve = 918;
    public const int EleHoly = 919;
    public const int EleChaos = 920;
    public const int EleMagic = 921;
    public const int EleEther = 922;
    public const int EleAcid = 923;
    public const int EleCut = 924;
    public const int EleImpact = 925;
    public const int EleVoid = 926;

    public static Dictionary<int, string> ElementAliasLookup = new Dictionary<int, string>
    {
        {
            EleFire, "eleFire"
        },
        {
            EleCold, "eleCold"
        },
        {
            EleLightning, "eleLightning"
        },
        {
            EleDarkness, "eleDarkness"
        },
        {
            EleMind, "eleMind"
        },
        {
            ElePoison, "elePoison"
        },
        {
            EleNether, "eleNether"
        },
        {
            EleSound, "eleSound"
        },
        {
            EleNerve, "eleNerve"
        },
        {
            EleHoly, "eleHoly"
        },
        {
            EleChaos, "eleChaos"
        },
        {
            EleMagic, "eleMagic"
        },
        {
            EleEther, "eleEther"
        },
        {
            EleAcid, "eleAcid"
        },
        {
            EleCut, "eleCut"
        },
        {
            EleImpact, "eleImpact"
        },
        {
            EleVoid, "eleVoid"
        }
    };

    #endregion

    #region Promotion Classes

    public const int FeatSentinel = 891000;
    public const int FeatBerserker = 891001;
    public const int FeatHermit = 891002;
    public const int FeatTrickster = 891003;
    public const int FeatElementalist = 891004;
    public const int FeatNecromancer = 891005;
    public const int FeatJenei = 891006;
    public const int FeatDruid = 891007;
    public const int FeatSniper = 891008;
    public const int FeatRanger = 891009;
    public const int FeatBattlemage = 891010;
    public const int FeatSpellblade = 891011;
    public const int FeatAdventurer = 891012;
    public const int FeatDancer = 891013;
    public const int FeatKnightcaller = 891014;
    public const int FeatSaint = 891015;
    public const int FeatWarCleric = 891016;
    public const int FeatSharpshooter = 891017;
    public const int FeatMachinist = 891018;
    public const int FeatWitchHunter = 891019;
    public const int FeatJusticar = 891020;
    public const int FeatSovereign = 891021;
    public const int FeatLuminary = 891022;
    public const int FeatHeadhunter = 891023;
    public const int FeatHarbinger = 891024;
    public const int FeatPhantom = 891025;
    public const int FeatRuneknight = 891026;
    public const int FeatHexer = 891027;
    public const int FeatArtificer = 891028;

    public const string SentinelId = "sentinel";
    public const string BerserkerId = "berserker";
    public const string HermitId = "hermit";
    public const string TricksterId = "trickster";
    public const string ElementalistId = "elementalist";
    public const string NecromancerId = "necromancer";
    public const string JeneiId = "jenei";
    public const string DruidId = "druid";
    public const string SniperId = "sniper";
    public const string RangerId = "ranger";
    public const string BattlemageId = "battlemage";
    public const string SpellbladeId = "spellblade";
    public const string AdventurerId = "adventurer";
    public const string DancerId = "dancer";
    public const string KnightcallerId = "knightcaller";
    public const string SaintId = "saint";
    public const string WarClericId = "warcleric";
    public const string SharpshooterId = "sharpshooter";
    public const string MachinistId = "machinist";
    public const string WitchHunterId = "witchhunter";
    public const string JusticarId = "justicar";
    public const string SovereignId = "sovereign";
    public const string LuminaryId = "luminary";
    public const string HeadhunterId = "headhunter";
    public const string HarbingerId = "harbinger";
    public const string PhantomId = "phantom";
    public const string RuneknightId = "runeknight";
    public const string HexerId = "hexer";
    public const string ArtificerId = "artificer";

    #endregion

    #region Unique charas

    public const string LailahCharaId = "lailah";

    // Knightcaller Commanders
    public const string ValeroCharaId = "knight_valero";
    public const string DinatogCharaId = "knight_dinatog";
    public const string ArkunCharaId = "knight_arkun";
    public const string AlestieCharaId = "knight_alestie";
    public const string EctoleCharaId = "knight_ectole";
    public const string RolingerCharaId = "knight_rolinger";

    // Knightcaller Knights
    public const string KnightArcherCharaId = "knight_archer";
    public const string KnightHermitCharaId = "knight_hermit";
    public const string KnightLancerCharaId = "knight_lancer";
    public const string KnightPriestessCharaId = "knight_priestess";
    public const string KnightDuelistCharaId = "knight_duelist";
    public const string KnightWarriorCharaId = "knight_warrior";
    public const string KnightWizardCharaId = "knight_wizard";

    // Druid Seeds
    public const string DruidWarriorEntCharaId = "druid_ent";
    public const string DruidEntangleFlowerCharaId = "druid_entangle";
    public const string DruidParalyticFlowerCharaId = "druid_paralytic";
    public const string DruidToxicFlowerCharaId = "druid_toxic";
    public const string DruidSoporificFlowerCharaId = "druid_soporific";
    public const string DruidSoothingBloomCharaId = "druid_soothing";
    public const string DruidWardingBloomCharaId = "druid_warding";
    public const string DruidSereneBloomCharaId = "druid_serene";
    public const string DruidNaturesWrathCharaId = "druid_wrath";
    public const string DruidNaturesWarmthCharaId = "druid_warmth";

    // Jenei Summons
    public const string JeneiCybeleCharaId = "jenei_cybele";
    public const string JeneiTiamatCharaId = "jenei_tiamat";
    public const string JeneiAtlantaCharaId = "jenei_atalanta";
    public const string JeneiBoreasCharaId = "jenei_boreas";
    public const string JeneiZaganCharaId = "jenei_zagan";
    public const string JeneiHauresCharaId = "jenei_haures";
    public const string JeneiCharonCharaId = "jenei_charon";
    public const string JeneiMegaeraCharaId = "jenei_megaera";
    public const string JeneiUlyssesCharaId = "jenei_ulysses";
    public const string JeneiDaedalusCharaId = "jenei_daedalus";
    public const string JeneiIrisCharaId = "jenei_iris";
    public const string JeneiFloraCharaId = "jenei_flora";
    public const string JeneiEclipseCharaId = "jenei_eclipse";
    public const string JeneiCatastropheCharaId = "jenei_catastrophe";
    public const string JeneiMolochCharaId = "jenei_moloch";
    public const string JeneiCoatlicueCharaId = "jenei_coatlicue";
    public const string JeneiAzulCharaId = "jenei_azul";
    
    // Machinist Turrets
    public const string MachinistRifleTurretCharaId = "machinist_rifle_turret";
    public const string MachinistRailgunTurretCharaId = "machinist_railgun_turret";
    public const string MachinistRocketTurretCharaId = "machinist_rocket_turret";
    
    // Necromancer Skeletons
    public const string NecromancerSkeletonWarriorCharaId = "necromancer_skeleton_warrior";
    public const string NecromancerSkeletonMageCharaId = "necromancer_skeleton_mage";
    public const string NecromancerDeathKnightCharaId = "necromancer_deathknight";

    #endregion

    #region Promoted Class Spells/Abilities

    // Adventurer
    public const int ActThisWayId = 1;
    public const int ActSenseDangerId = 1;
    
    // Artificer
    public const int ActImprovisedBrewId = 1;
    public const int ActSimpleSynthesisId = 1;

    // Berserker
    public const int ActBloodlustId = 1;
    public const int ActSunderId = 1;

    // Battlemage
    public const int SpFireCannon = 1;
    public const int SpFireHammer = 1;
    public const int SpColdCannon = 1;
    public const int SpColdHammer = 1;
    public const int SpLightningCannon = 1;
    public const int SpLightningHammer = 1;
    public const int SpDarknessCannon = 1;
    public const int SpDarknessHammer = 1;
    public const int SpMindCannon = 1;
    public const int SpMindHammer = 1;
    public const int SpPoisonCannon = 1;
    public const int SpPoisonHammer = 1;
    public const int SpNetherCannon = 1;
    public const int SpNetherHammer = 1;
    public const int SpSoundCannon = 1;
    public const int SpSoundHammer = 1;
    public const int SpNerveCannon = 1;
    public const int SpNerveHammer = 1;
    public const int SpHolyCannon = 1;
    public const int SpHolyHammer = 1;
    public const int SpChaosCannon = 1;
    public const int SpChaosHammer = 1;
    public const int SpMagicCannon = 1;
    public const int SpMagicHammer = 1;
    public const int SpEtherCannon = 1;
    public const int SpEtherHammer = 1;
    public const int SpAcidCannon = 1;
    public const int SpAcidHammer = 1;
    public const int SpCutCannon = 1;
    public const int SpCutHammer = 1;
    public const int SpImpactCannon = 1;
    public const int SpImpactHammer = 1;

    // Dancer
    public const int ActDancePartnerId = 1;
    public const int ActDaggerIllusionId = 1;
    public const int ActSwordFouetteId = 1;
    public const int ActWildPirouetteId = 1;
    public const int StanceEnergyDanceId = 1;
    public const int StanceFreedomDanceId = 1;
    public const int StanceHealingDanceId = 1;
    public const int StanceMistDanceId = 1;
    public const int StanceSwiftDanceId = 1;

    // Druid
    public const int ActSowWarmSeedsId = 1;
    public const int ActSowWrathSeedsId = 1;
    public const int SpSummonTreeEntId = 1;

    // Elementalist
    public const int ActElementalFuryId = 1;
    public const string ElementalFuryAlias = "elementalist_elementalfury";
    public const int ActFlareId = 1;
    public const string FlareAlias = "elementalist_flare";

    // Harbinger
    public const int ActEndlessMistsId = 1;
    public const int ActAccursedTouchId = 1;

    // Headhunter
    public const int ActExecuteId = 1;
    public const int ActReapId = 1;

    // Hermit
    public const int ActMarkForDeathId = 1;
    public const int ActShadowShroudId = 1;
    public const int ActAssassinateId = 1;

    // Hexer
    public const int ActTraumatizeId = 1;
    public const string TraumatizeAlias = "hexer_traumatize";
    public const int ActBloodCurseId = 1;
    
    // Jenei
    public const int ActSpiritSummonId = 1;

    // Justicar
    public const int ActIntimidateId = 1;
    public const int ActSubdueId = 1;
    public const int ActCondemnId = 1;
    public const string CondemnAlias = "justicar_condemn";

    // Knightcaller
    public const int ActSpiritRageId = 1;
    public const int ActSpiritRallyId = 1;
    public const int ActSummonKnightId = 1;

    // Luminary
    public const int VanguardStanceId = 1;
    public const int ActLightWaveId = 1;
    public const string LightwaveAlias = "luminary_lightwave";
    public const int ActParryId = 1;

    // Machinist
    public const int ActLoadUpId = 1;
    public const int ActOverclockId = 1;

    // Necromancer
    public const int ActBeckonOfTheDeadId = 1;
    public const int ActBlessingOfTheDeadId = 1;
    public const int ActCorpseExplosionId = 1;
    public const int SpSummonSkeleton = 1;

    // Phantom
    public const int ActWolkenkratzerId = 1;
    public const int ActSchwarzeKatze = 1;
    public const int ActVerbrechenId = 1;

    // Ranger
    public const int StanceRangersCantoId = 1;
    public const int ActGimmickCoatingId = 1;
    public const int ActSetTrapId = 1;
    public const string RangerBlastTrapAlias = "ranger_blast_trap";
    public const string RangerParalyzeTrapAlias = "ranger_paralyze_trap";
    public const string RangerPoisonTrapAlias = "ranger_poison_trap";
    public const string RangerPunjiTrapAlias = "ranger_punji_trap";
    public const string RangerSnareTrapAlias = "ranger_snare_trap";

    public enum RangerCoatings
    {
        ShatterCoating,
        HammerCoating,
        BladedCoating,
        PoisonCoating,
        ParalyticCoating
    }

    // Rune Knight
    public const int ActRunicGuardId = 1;
    public const int ActSpinningSlashId = 1;
    public const int ActRuneEtchingId = 1;

    // Seer
    public const int ActBestowLightId = 1;

    // Sentinel
    public const int ActShoutId = 1;
    public const int ActShieldSmiteId = 1;
    public const int StanceRageId = 1;
    public const int StanceRestraintId = 1;

    // Sharpshooter
    public const int StanceGoProneId = 1;
    public const int StanceOverwatchId = 1;

    // Sniper
    public const int ActTargetHeadId = 1;
    public const int ActTargetHandId = 1;
    public const int ActTargetLegId = 1;
    public const int ActTargetVitalsId = 1;

    // Sovereign
    public const int StanceLawFlag = 1;
    public const int StanceChaosFlag = 1;
    public const int ActBattleOrderId = 1;
    public const int ActFormationOrderId = 1;
    public const int ActStrategyOrderId = 1;

    // Spellblade
    public const int ActCrushingStrikeId = 1;
    public const int ActMyriadFlecheId = 1;

    // Trickster
    public const int ActDiversionId = 1;
    public const int ActArcaneTrapId = 1;
    public const int ActManifestNightmareId = 1;

    // War Cleric
    public const int ActRescueId = 1;
    public const int ActSolId = 1;

    // Witch Hunter
    public const int ActManaBreakId = 1;

    #endregion

}