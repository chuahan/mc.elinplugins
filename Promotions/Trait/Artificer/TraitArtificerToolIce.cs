using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolIce : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_iceaxe";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        float powerMulti = 1f + (cc.Evalue(102) / 2F + cc.Evalue(101)) / 50f;
        int scaledPower = (int)(power * powerMulti);

        List<Point> targetTiles = new List<Point>
        {
            pos
        };
        pos.ForeachNeighbor(delegate(Point p)
        {
            if (!p.Equals(pos))
            {
                targetTiles.Add(p.Copy());
            }
        });

        foreach (Point point in targetTiles)
        {
            point.PlayEffect("hit_slash");
            point.PlaySound("ab_magicsword");
            foreach (Card tc in point.ListCards().Copy())
            {
                if (!cc.IsAliveInCurrentZone)
                {
                    break;
                }

                if (tc.trait.CanBeAttacked || tc.isChara && tc.Chara.IsHostile(cc))
                {
                    if (tc.Chara.HasCondition<ConFreeze>())
                    {
                        int damage = HelperFunctions.SafeDice(ArtificerToolId, power, true);
                        HelperFunctions.ProcSpellDamage(scaledPower, damage, cc, tc.Chara, AttackSource.MagicSword, Constants.EleCold);
                    }
                    else
                    {
                        int damage = HelperFunctions.SafeDice(ArtificerToolId, power);
                        HelperFunctions.ProcSpellDamage(scaledPower, damage, cc, tc.Chara, AttackSource.MagicSword, Constants.EleCold);
                    }
                    ActEffect.ProcAt(EffectId.Debuff, scaledPower, BlessedState.Normal, Act.CC, tc.Chara, tc.Chara.pos, true, new ActRef
                    {
                        origin = Act.CC.Chara,
                        n1 = nameof(ConFreeze)
                    });
                }
            }
        }
        return true;
    }
}