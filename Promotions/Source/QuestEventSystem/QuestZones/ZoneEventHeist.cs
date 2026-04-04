using Newtonsoft.Json;
using PromotionMod.Source.QuestEventSystem.QuestTypes;
namespace PromotionMod.Source.QuestEventSystem.QuestZones;

public class ZoneEventHeist : ZoneEventQuest
{
    [JsonProperty] public int ValuableUID;
    public Thing Valuable => zone.map.FindThing(ValuableUID);

    public QuestCovertOpHeist QuestHeist => quest as QuestCovertOpHeist;

    public override void OnVisit()
    {

    }

    public override void OnLeaveZone()
    {
        if (pc.things.Find(ValuableUID) != null)
        {
//            this.QuestHeist.
        }
        if (_zone.instance.status == ZoneInstance.Status.Running)
        {
            _zone.instance.status = OnReachTimeLimit();
        }
    }
}