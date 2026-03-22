using System.Linq;
using UnityEngine;
namespace PromotionMod.Stats.Adventurer;

public class ConSenseDanger : Timebuff
{
    public override ConditionType Type => ConditionType.Buff;

    public override bool ShouldRefresh => true;

    public override void OnRefresh()
    {
        owner.hasTelepathy = true;
    }

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        foreach (Card card in owner.currentZone.map.Cards.Where(card => card.trait is TraitSwitch))
        {
            card.isHidden = false;
        }
        
        base.Tick();
    }
}