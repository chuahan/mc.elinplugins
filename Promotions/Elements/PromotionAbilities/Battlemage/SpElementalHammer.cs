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
        ActEffect.ProcAt(EffectId.Sword, GetPower(CC), BlessedState.Normal, CC, TC, TP, true, new ActRef
        {
            act = this,
            aliasEle = _source.aliasRef,
            origin = CC
        });
        List<Point> cleaveTiles = new List<Point>();
        TP.ForeachNeighbor(delegate(Point p)
        {
            if (!p.Equals(TP))
            {
                cleaveTiles.Add(p.Copy());
            }
        });

        foreach (Point item2 in cleaveTiles)
        {
            foreach (Card item3 in item2.ListCards().Copy())
            {
                if (!CC.IsAliveInCurrentZone)
                {
                    break;
                }

                if (item3.trait.CanBeAttacked || item3.isChara && item3.Chara.IsHostile(CC))
                {
                    ActEffect.ProcAt(EffectId.Sword, GetPower(CC), BlessedState.Normal, CC, TC, TP, true, new ActRef
                    {
                        act = this,
                        aliasEle = _source.aliasRef,
                        origin = CC
                    });
                }
            }
        }
        return true;
    }
}