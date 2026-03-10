using Newtonsoft.Json;
using PromotionMod.Source.QuestEventSystem.QuestTypes;
namespace PromotionMod.Source.QuestEventSystem.QuestZones;

public class ZoneEventHeist : ZoneEventQuest
{
    [JsonProperty] public int ValuableUID;
    public Thing Valuable => this.zone.map.FindThing(ValuableUID);
    
    public QuestCovertOpHeist QuestHeist => base.quest as QuestCovertOpHeist;

    public override void OnVisit()
    {
        
    }
    
    public override void OnLeaveZone()
    {
        if (EClass.pc.things.Find(this.ValuableUID) != null)
        {
//            this.QuestHeist.
        }
        if (EClass._zone.instance.status == ZoneInstance.Status.Running)
        {
            EClass._zone.instance.status = OnReachTimeLimit();
        }
        
        
    }
}