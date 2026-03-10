using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolEarth : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_earthgauntlet";

    public override float EffectRadius => 5;

    public virtual void MarkMapHighlights(bool shouldHighlight, Point target)
    {
        EClass._map.ForeachSphere(target.x, target.z, EffectRadius, delegate(Point p)
        {
            if (!p.HasBlock && shouldHighlight)
            {
                p.SetHighlight(8);
            }
        });
    }
    
    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        float powerMulti = 1f + (cc.Evalue(100) / 2F + cc.Evalue(132)) / 50f;
        int scaledPower = (int)(power * powerMulti);

        int damage = HelperFunctions.SafeDice(ArtificerToolId, scaledPower);
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(pos, EffectRadius, false, false))
        {
            int distance = tile.Distance(pos);
            foreach (Chara target in tile.ListCharas())
            {
                if (!targetsHit.Contains(target))
                {
                    if (target.IsHostile(cc))
                    {
                        HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleImpact);
                        // Apply Gravity and Speed Down.
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef
                        {
                            origin = Act.CC.Chara,
                            n1 = nameof(ConGravity)
                        });
                        
                        ActEffect.ProcAt(EffectId.DebuffStats, power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef
                        {
                            origin = Act.CC.Chara,
                            n1 = "SPD"
                        });
                    }
                }

                targetsHit.Add(target);
            }

            Effect spellEffect = Effect.Get("Element/ball_Impact");
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > cc.pos.x);
        }
        return true;
    }
}