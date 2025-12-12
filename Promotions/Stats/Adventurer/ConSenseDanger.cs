using System.Linq;
namespace PromotionMod.Stats.Adventurer;

public class ConSenseDanger : Timebuff
{
    public override ConditionType Type => ConditionType.Buff;
    
    public override bool ShouldRefresh => true;

    public override void OnRefresh()
    {
        owner.hasTelepathy = true;
    }

    public override void Tick()
    {
        foreach (Card card in owner.currentZone.map.Cards.Where(card => card.trait is TraitSwitch))
        {
            card.isHidden = false;
        }
    }
}