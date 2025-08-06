using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Battlemage;

public class SpElementalHammer : Spell
{
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
        ActEffect.ProcAt(EffectId.Sword, this.GetPower(CC), BlessedState.Normal, CC, TC, TP, true,  new ActRef
        {
            act = this,
            aliasEle = this._source.aliasRef,
            origin = CC,
        });
        List<Point> cleaveTiles = new List<Point>();
        Act.TP.ForeachNeighbor(delegate(Point p)
        {
            if (!p.Equals(Act.TP))
            {
                cleaveTiles.Add(p.Copy());
            }
        });
        
        foreach (Point item2 in cleaveTiles)
        {
            foreach (Card item3 in item2.ListCards().Copy())
            {
                if (!Act.CC.IsAliveInCurrentZone)
                {
                    break;
                }
                
                if (item3.trait.CanBeAttacked || (item3.isChara && item3.Chara.IsHostile(Act.CC)))
                {
                    ActEffect.ProcAt(EffectId.Sword, this.GetPower(CC), BlessedState.Normal, CC, TC, TP, true,  new ActRef
                    {
                        act = this,
                        aliasEle = this._source.aliasRef,
                        origin = CC,
                    });
                }
            }
        }
        return true;
    }
}