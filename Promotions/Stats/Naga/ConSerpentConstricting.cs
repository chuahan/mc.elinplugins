using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConSerpentConstricting : BaseDebuff
{
    // Realistically, a Naga can only constrict a single target at a time.
    // Using a JsonProperty, I am able to modify the condition to point towards a Unique Identifier.
    // This way the condition can keep track of what character is being choked.
    // Make sure when we add this condition that we set this property.
    [JsonProperty(PropertyName = "L")] public int LinkedUID;

    // The user can simply choose to stop constricting the target.
    public override bool CanManualRemove => true;

    public override ConditionType Type => ConditionType.Debuff;
    
    public override string TextDuration => "";

    // When this Condition is removed, we want to remove the linked condition on the Victim.
    public override void OnRemoved()
    {
        Chara? linkedChara = HelperFunctions.FindLinkedConditionCarrier(owner, LinkedUID);
        linkedChara?.RemoveCondition<ConSerpentConstriction>();
    }
    
    public override void Tick()
    {
        // You're probably not going to be choking someone in the world map, so I'll end this condition if the owner is in the world map.
        if (EClass._zone.IsRegion)
        {
            this.Kill();
        }
        
        // Make sure the victim is still neighboring the Naga and hasn't teleported away or something.
        Chara? victim = HelperFunctions.FindLinkedConditionCarrier(owner, LinkedUID);
        
        // If the Naga is choking a nonexistent or dead character, go ahead and remove this condition.
        if (victim is not { IsAliveInCurrentZone: true })
        {
            this.Kill();
            return;
        }
        
        // Let's give the Naga a little big of exp in the Constricting Skill for continually choking someone.
        owner.elements.ModExp(891179, 20f);
    }
}