using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Battlemage;

public class SpElementalCannon : Spell
{
    public override bool CanAutofire => true;

    public override bool CanPressRepeat => true;

    public override bool CanRapidFire => true;

    public override float RapidDelay => 0.3f;
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatBattlemage) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.BattlemageId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Render an arrow going at the target.
        ElementRef elementRef = setting.elements[this._source.aliasRef];
        Effect elementArrow = Effect.Get("spell_arrow");
        elementArrow.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = elementArrow.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;
        elementArrow.Play(CC.pos, 0f, TC.pos);
        
        // Blow up the area around the target.
        ActRef cannonSpell = new ActRef
        {
            act = this,
            aliasEle = this._source.aliasRef,
            origin = CC,
        };
        Element element = Element.Create(cannonSpell.aliasEle, this.GetPower(CC) / 10);
        List<Point> targetArea = EClass._map.ListPointsInCircle(TC.pos, 5, false, false);
        ActEffect.DamageEle(CC, EffectId.Ball, this.GetPower(CC), element, targetArea, cannonSpell, "spell_ball");
        return true;
    }
}