using System.Collections.Generic;
namespace PromotionMod.Source.QuestEventSystem.SpawnerConfigurations;

/// <summary>
///     This class sets up what kind of enemies will spawn in a quest.
/// </summary>
public abstract class SpawnSetup
{
    // What Kind of Spawners this quest will have.
    // spawner_monster - Spawns Orks/Yeeks/Goblins/Hounds/Sahagins/Minotaurs
    // spawner_human - Spawns humanoid enemies
    // spawner_evil - Spawns Demons and Undead.
    public virtual string SpawnerType => "";
    public virtual List<string> SpawnerResults => new List<string>();
}

public class FantasyBaddiesSetup : SpawnSetup
{
    public override string SpawnerType => "spawner_monster";

    public override List<string> SpawnerResults => new List<string>
    {
        "orc_warrior",
        "orc",
        "orc_goda",
        "goblin",
        "goblin_warrior",
        "goblin_wizard",
        "goblin_shaman",
        "zealot_fire",
        "yeek",
        "yeek_warrior",
        "yeek_archer",
        "yeek_kamikaze",
        "yeek_master",
        "giant",
        "cyclops",
        "hound",
        "dragon"
    };
}

public class MechanicalHordeSetup : SpawnSetup
{
    public override string SpawnerType => "spawner_human";

    public override List<string> SpawnerResults => new List<string>
    {
        "blade_alpha",
        "blade",
        "blade_beta",
        "mech_basher",
        "mech_scarab",
        "turret",
        "trooper",
        "robot",
        "mech_angel",
        "mech_death",
        "bit2",
        "drone"
    };
}

public class MercenarySetup : SpawnSetup
{
    public override string SpawnerType => "spawner_human";

    public override List<string> SpawnerResults => new List<string>
    {
        "merc_archer",
        "merc_mage",
        "merc_warrior"
    };
}

public class HumanoidFantasyArmySetup : SpawnSetup
{
    public override string SpawnerType => "spawner_human";

    public override List<string> SpawnerResults => new List<string>
    {
        "merc_archer",
        "merc_mage",
        "merc_warrior"
    };
}

public class UndeadHordeSetup : SpawnSetup
{
    public override string SpawnerType => "spawner_evil";

    public override List<string> SpawnerResults => new List<string>
    {
        "messenger_death",
        "reaper",
        "lich",
        "lich_master",
        "lich_demi",
        "vampire",
        "geist",
        "skeleton_warrior",
        "skeleton_berserker",
        "skeleton_spartoi"
    };
}

public class DemonIncursionSetup : SpawnSetup
{
    public override string SpawnerType => "spawner_evil";

    public override List<string> SpawnerResults => new List<string>
    {
        "living_armor",
        "mass_steel",
        "armor_death",
        "armor_gold",
        "handmaiden",
        "gagu",
        "deathgaze",
        "beholder",
        "evileye",
        "asura",
        "mitra",
        "varuna"
    };
}