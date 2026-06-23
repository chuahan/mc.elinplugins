using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats;

public class ConSerpentConstriction : BaseDebuff
{
    // Similar to being suffocated, Prevent Regeneration while being constricted. 
    public override bool PreventRegen => true;
    
    // Realistically, a Naga can only constrict a single target at a time.
    // Using a JsonProperty, I am able to modify the condition to point towards a Unique Identifier.
    // This way the condition can keep track of what character is being choked.
    // Make sure when we add this condition that we set this property.
    [JsonProperty(PropertyName = "L")] public int LinkedUID;
    
    public override ConditionType Type => ConditionType.Debuff;
    
    // Since this is more of a "Toggle" ability, this hides the number that normally would show the duration.
    public override string TextDuration => "";
    
    // When this Condition is removed, we want to remove the linked condition on the character doing the constricting.
    public override void OnRemoved()
    {
        Chara? linkedChara = HelperFunctions.FindLinkedConditionCarrier(owner, LinkedUID);
        linkedChara?.RemoveCondition<ConSerpentConstricting>();
    }

    public override void Tick()
    {
        // You're probably not going to be choking someone in the world map, so I'll end this condition if the owner is in the world map.
        if (EClass._zone.IsRegion)
        {
            this.Kill();
            return;
        }
        
        // I want to give the victim a chance to escape.
        // First, find the Naga currently constricting the target. We know this condition can only be applied in melee range, so I can simply look into the neighboring tiles.
        Chara? constrictor = HelperFunctions.FindLinkedConditionCarrier(owner, LinkedUID);
        
        // If the victim is being choked by a nonexistent Naga, go ahead and remove this condition.
        if (constrictor is not { IsAliveInCurrentZone: true })
        {
            this.Kill();
            return;
        }
        
        // EClass.rnd simply returns a random number between 0 and the provided number
        // EClass.rndHalf first divides the value in half, then adds a random number between 0 and the halved amount to the original half, guaranteeing half of the value at least.
        // So this section basically pits the victim's strength against the constrictor's strength, where the constrictor has an advantage.
        // Then, if if the victim has lower overall strength than their constrictor, they must win a 25% roll to escape.
        if (EClass.rnd(owner.Evalue(SKILL.STR)) > EClass.rndHalf(constrictor.Evalue(SKILL.STR)) &&
            (owner.Evalue(SKILL.STR) > constrictor.Evalue(SKILL.STR) || EClass.rnd(4) == 0))
        {
            // They have successfully broken free! End this condition.
            CC.Say("serpentine_constriction_escape".langGame(owner.NameSimple, constrictor.NameSimple));
            this.Kill();
        }
        else
        {
            // Whoops, looks like they're still being choked. Real Shame.
            // Force Apply ConEntangle to the victim.
            owner.AddCondition<ConEntangle>(this.power, force: true);
            CC.Say("serpentine_constriction_damage".langGame(owner.NameSimple));
            // They will take choking damage relevant to their Max HP.
            owner.DamageHP(10 + owner.MaxHP / 20, AttackSource.Condition);   
        }
    }
}