using Newtonsoft.Json;
using PromotionMod.Source.QuestEventSystem.QuestZones;
namespace PromotionMod.Source.QuestEventSystem.QuestTypes;

/// <summary>
///     Covert Ops: Heist.
///     The player is teleported to a Heist zone.
///     There will be a plethora of enemy guards placed in the area.
///     The player must reach the "treasure room" and steal the valuable.
///     The moment the item is taken from the original container, this will aggro all the tagged defenders in the zone.
///     While carrying the item, the player cannot use teleport spells or any teleporters.
///     The player then must make it to the extraction zone with the item in hand to complete the quest.
/// </summary>
public class QuestCovertOpHeist : QuestInstance
{

    public int MapVariant;
    [JsonProperty] public bool ValuableStolen;
    [JsonProperty] public int ValuableWorth;

    public override int RangeDeadLine => 30;

    public override int KarmaOnFail => 0;

    public override bool FameContent => true;

    public override int BaseMoney => source.money + EClass.curve(DangerLv, 20, 20) * 10;

    public override int FameOnComplete => base.FameOnComplete * 2;

    public override string IdZone => "instance_heist";

    public override string GetTextProgress()
    {
        return ValuableStolen ? "covertops_heist_progress_1".langGame() : "covertops_heist_progress_2".langGame();
    }

    public override void OnInit()
    {
        MapVariant = EClass.rnd(4);
    }

    public override ZoneEventQuest CreateEvent()
    {
        return new ZoneEventHeist();
    }

    public override void OnBeforeComplete()
    {
        // The player will get a cut of the valuable. The cut they get is based on their ranking in the Information Guild.
        //if ()
        bonusMoney += ValuableWorth;
    }
}