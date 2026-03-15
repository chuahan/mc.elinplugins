using System.Collections.Generic;
using System.Linq;
namespace PromotionMod.Common;

public class Constants
{

    internal const int SigmoidScalingMax = 3000;
    internal const float SigmoidScalingPowerSlope = 1.2f;

    public const int PromotionLevelRequirement = 20;

    #region CWL Flags

    public const string PromotionFeatFlag = "promo";
    public const string IsPlayerFactionTrapFlag = "pcTrap";
    public const string JeneiAttunementFlag = "jeAtt";

    public const string PhantomPromotionUnlockedFlag = "phUL";
    public const string EtoilePromotionUnlockedFlag = "etUL";
    public const string HeroPromotionUnlockedFlag = "heUL";
    public const string LusterPromotionUnlockedFlag = "luUL";

    public const string ArtificerGolemCreated = "madeGol";
    public const string AdvancedCombatSkillFlag = "advSkill";
    public const string UnlockedEliteEnemiesFlag = "eliteEnemiesUL";
    public const string IsEliteEnemyFlag = "eliteEnemy";

    #endregion

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

    public const int FaithId = 306;

    public static readonly Dictionary<int, string> ElementAliasLookup = new Dictionary<int, string>
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

    public static readonly Dictionary<string, int> ElementIdLookup =
            ElementAliasLookup.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

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
    public const int FeatHolyKnight = 891022;
    public const int FeatHeadhunter = 891023;
    public const int FeatHarbinger = 891024;
    public const int FeatDreadKnight = 891025;
    public const int FeatRuneKnight = 891026;
    public const int FeatHexer = 891027;
    public const int FeatArtificer = 891028;

    // Unimplemented. Future update
    public const int FeatPhantom = 1;
    public const int FeatEtoile = 1;
    public const int FeatHero = 1;
    public const int FeatLuster = 1;

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
    public const string HolyKnightId = "holyknight";
    public const string HeadhunterId = "headhunter";
    public const string HarbingerId = "harbinger";
    public const string DreadKnightId = "dreadknight";
    public const string RuneKnightId = "runeknight";
    public const string HexerId = "hexer";
    public const string ArtificerId = "artificer";

    public const string PhantomId = "phantom";
    public const string EtoileId = "etoile";
    public const string HeroId = "hero";
    public const string LusterId = "luster";

    #endregion

    #region Unique charas

    public const string LailahCharaId = "lailah";
    public const string MinariCharaId = "minari";
    public const string VyersCharaId = "vyers";
    public const string RurasCharaId = "ruras";
    public const string SenaCharaId = "sena";
    public const string AzaleaCharaId = "azalea";

    // Quest Characters
    public const string WatcherKnightCharaId = "watcher_knight";
    public const string EscortWagonCharaId = "escort_wagon";
    public const string MonsterSpawnerCharaId = "spawner_monster";
    public const string HumanSpawnerCharaId = "spawner_human";
    public const string EvilSpawnerCharaId = "spawner_evil";

    public const string BasicGuardCharaId = "armed_guard";
    public const string EliteGuardCharaId = "elite_armed_guard";

    public const string CouncilSaroCharaId = "council_saro";
    public const string CouncilCademCharaId = "council_cadem";
    public const string CouncilDorenCharaId = "council_doren";
    public const string CouncilWyndinCharaId = "council_wyndin";
    public const string CouncilArthurCharaId = "council_arthur";

    // Knightcaller Commanders
    public const string ValeroCharaId = "knight_valero";
    public const string DinatogCharaId = "knight_dinatog";
    public const string ArkunCharaId = "knight_arkun";
    public const string AlestieCharaId = "knight_alestie";
    public const string EctoleCharaId = "knight_ectole";
    public const string RolingerCharaId = "knight_rolinger";
    public const string DiasCharaId = "knight_dias";

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

    // Trickster Summon
    public const string PhantomTricksterCharaId = "phantom_trickster";

    // Alternate Bits
    public const string NormalBitCharaId = "bit";
    public const string ShieldBitCharaId = "shield_funnel";
    public const string SwordBitCharaId = "sword_funnel";
    public const string PhantomBitCharaId = "phantom_funnel";

    public const string HolyBannerCharaId = "holy_banner";

    #endregion

    #region Spells/Abilities Element Ids

    public const int ActSenseDangerId = 891034;
    public const int ActThisWayId = 891035;
    public const int ActImprovisedBrewId = 891036;
    public const int ActSimpleSynthesisId = 891037;
    public const int StManaShieldId = 891038;
    public const int StManaFocusId = 891039;
    public const int ActBloodlustId = 891040;
    public const int ActLifebreakId = 891041;
    public const int ActSunderId = 891042;
    public const int ActDaggerIllusionId = 891043;
    public const int ActDancePartnerId = 891044;
    public const int ActSwordFouetteId = 891045;
    public const int ActWildPirouetteId = 891046;
    public const int StEnergyDanceId = 891047;
    public const int StFreedomDanceId = 891048;
    public const int StHealingDanceId = 891049;
    public const int StMistDanceId = 891050;
    public const int StSwiftDanceId = 891051;
    public const int ActSowWarmSeedsId = 891052;
    public const int ActSowWrathSeedsId = 891053;
    public const int ActLivingArmorId = 891054;
    public const int SpSummonTreeEntId = 891055;
    public const int ActElementalFuryId = 891056;
    public const int ActElementalExtinctionId = 891057;
    public const int ActAccursedTouchId = 891058;
    public const int ActEndlessMistsId = 891059;
    public const int StGloomId = 891060;
    public const int ActExecuteId = 891061;
    public const int ActReapId = 891062;
    public const int ActAssassinateId = 891063;
    public const int ActMarkForDeathId = 891064;
    public const int ActShadowShroudId = 891065;
    public const int ActBloodCurseId = 891066;
    public const int ActTraumatizeId = 891067;
    public const int ActRevengeId = 891068;
    public const int ActSpiritSummonId = 891069;
    public const int ActJeneiBlazeId = 891070;
    public const int ActJeneiDelugeId = 891071;
    public const int ActJeneiDragonFumeId = 891072;
    public const int ActJeneiMotherGaiaId = 891073;
    public const int ActJeneiMoveId = 891074;
    public const int ActJeneiPlyId = 891075;
    public const int ActJeneiRevealId = 891076;
    public const int ActJeneiShinePlasmaId = 891077;
    public const int ActCondemnId = 891078;
    public const int ActIntimidateId = 891079;
    public const int ActSubdueId = 891080;
    public const int StFlamesOfJudgementId = 891081;
    public const int ActSpiritRageId = 891082;
    public const int ActSpiritRallyId = 891083;
    public const int ActSpiritMobilizeId = 891084;
    public const int ActSummonKnightId = 891085;
    public const int ActSpearheadId = 891086;
    public const int ActDeflectionId = 891087;
    public const int StVanguardId = 891088;
    public const int ActLoadUpId = 891089;
    public const int ActOverclockId = 891090;
    public const int StHeavyarmsId = 891091;
    public const int SpSummonTurretId = 891092;
    public const int ActBeckonOfTheDeadId = 891093;
    public const int ActBlessingOfTheDeadId = 891094;
    public const int ActCorpseExplosionId = 891095;
    public const int SpSummonSkeletonId = 891096;
    public const int ActSchwarzeKatzeId = 891097;
    public const int ActVerbrechenId = 891098;
    public const int ActWolkenkratzerId = 891099;
    public const int ActGimmickCoatingId = 891100;
    public const int ActThrowTrapId = 891101;
    public const int StRangersCantoId = 891102;
    public const int ActRuneEtchingId = 891103;
    public const int ActRunicGuardId = 891104;
    public const int ActSpinningSlashId = 891105;
    public const int ActBlessingId = 891106;
    public const int ActHandOfGodId = 891107;
    public const int ActShieldSmiteId = 891108;
    public const int ActShoutId = 891109;
    public const int StRageId = 891110;
    public const int StRestraintId = 891111;
    public const int ActChargedShotId = 891112;
    public const int ActMarkHostilesId = 891113;
    public const int StOverwatchId = 891114;
    public const int ActSpreadShotId = 891115;
    public const int ActTargetHandId = 891116;
    public const int ActTargetHeadId = 891117;
    public const int ActTargetLegsId = 891118;
    public const int ActFormationOrderId = 891119;
    public const int ActMoraleOrderId = 891120;
    public const int ActStrategyOrderId = 891121;
    public const int StChaosModeId = 891122;
    public const int StLawModeId = 891123;
    public const int ActCrushingStrikeId = 891124;
    public const int ActMyriadFlecheId = 891125;
    public const int ActSiphoningBladeId = 891126;
    public const int ActArcaneTrapId = 891127;
    public const int ActDetonateTrapId = 891128;
    public const int ActDiversionId = 891129;
    public const int ActReversalId = 891130;
    public const int ActDeploySanctuaryId = 891131;
    public const int ActDivineDescentId = 891132;
    public const int ActDivineFistId = 891133;
    public const int ActMagicReflectId = 891134;
    public const int ActManaBreakId = 891135;
    public const int ActNullZoneId = 891136;
    public const int StCommanderAuraId = 891137;
    public const int ActTrampleId = 891138;
    public const int ActBlessedArmamentsId = 891139;
    public const int ActHolyBannerId = 891140;
    public const int ActManaStarterId = 891141;
    public const int StLifeIgnitionId = 891142;
    public const int ActDarkBurstId = 891143;
    public const int ActDarkBarrierId = 891144;
    public const int ActSteamlightId = 891145;
    public const int ActInvigorateId = 891167;


    public const string ActTrampleAlias = "ActTrample";

    public const string TraumatizeAlias = "ActTraumatize";

    public const string CondemnAlias = "ActCondemn";

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

    public const string TricksterArcaneTrapAlias = "trickster_arcane_trap";
    public const string TricksterReversalAlias = "ActReversal";
    public const string WarClericDivineDescentAlias = "ActDivineDescent";

    #endregion

    #region Artificer Stuff

    public const int FeatHarpyGolemVisionId = 891029;
    public const int FeatSirenGolemSpeedId = 891030;
    public const int FeatSirenGolemMagicId = 891031;
    public const int FeatTitanGolemId = 891032;
    public const int FeatArtificerGolemUpgradeId = 891033;

    public const string ArtificerGolem_MemoryChipId = "artificer_golem_memorychip";
    public const string ArtificerGolem_ComponentChipId = "artificer_golem_componentchip";
    public const string MimGolemCharaId = "golem_mim";
    public const string HarpyGolemCharaId = "golem_harpy";
    public const string SirenGolemCharaId = "golem_siren";
    public const string TitanGolemCharaId = "golem_titan";

    public const string ArtificerSteamlightItem = "artificer_steamlight";

    #endregion

    #region Adv Combat Skills

    public const int EliteEnemiesSpawnLevel = 200;

    public const int FeatAegisId = 891147;
    public const int FeatPaviseId = 891148;
    public const int FeatAstraId = 891149;
    public const int FeatLunaId = 891150;
    public const int FeatSolId = 891151;
    public const int FeatGaleforceId = 891152;
    public const int FeatNihilId = 891153;
    public const int FeatLethalityId = 891154;
    public const int FeatRendHeavenId = 891155;
    public const int FeatVengeanceId = 891156;
    public const int FeatDeadeyeId = 891157;
    public const int FeatVantageId = 891158;

    // Enemy Only
    public const int FeatAegisPlusId = 891159;
    public const int FeatPavisePlusId = 891160;
    public const int FeatLunaPlusId = 891161;
    public const int FeatVantagePlusId = 891162;
    public const int FeatMetalBreakerId = 891163;

    #endregion

    #region Alraune

    public const int FeatAlraune = 891164;
    public const int StSweetScentId = 891165;
    public const int ActWildGrowthId = 891166;

    #endregion

    #region Guild Stuff

    public const string AluenaFactionId = "promotionmod_aluena";
    public const string AdvGuildFactionId = "promotionmod_guild_adv";
    public const string InfoGuildFactionId = "promotionmod_guild_info";

    #endregion

}