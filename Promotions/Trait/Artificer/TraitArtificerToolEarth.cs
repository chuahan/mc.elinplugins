using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolEarth : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_earthgauntlet";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        float powerMulti = 1f + (cc.Evalue(100) / 2F + cc.Evalue(132)) / 50f;
        int scaledPower = (int)(power * powerMulti);

        int damage = HelperFunctions.SafeDice(ArtificerToolId, scaledPower);
        Effect spellEffect = Effect.Get("Element/ball_Impact");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(pos, 5f, false, false))
        {
            int distance = tile.Distance(pos);
            foreach (Chara target in tile.ListCharas())
            {
                if (!targetsHit.Contains(target))
                {
                    if (target.IsHostile(cc))
                    {
                        HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleImpact);
                    }
                }

                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion,
            float delay = distance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(tile, 0f, tile);
            });
        }
        owner.c_ammo--;
        return true;
    }
}