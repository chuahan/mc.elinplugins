using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class ActMarkHostiles : PromotionCombatAbility
{

    private float _effectRadius = 3F;
    public override int PromotionId => Constants.FeatSharpshooter;
    public override string PromotionString => Constants.SharpshooterId;
    public override int AbilityId => Constants.ActMarkHostilesId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        int manaRestore = (int)(CC.mana.max * 0.05F);
        List<Chara> targetsHit = new List<Chara>();
        ElementRef colorRef = setting.elements["eleCut"];
        foreach (Point tile in _map.ListPointsInCircle(TC.pos, 3f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target)))
            {
                // Damage Hostiles and apply Fear
                if (target.IsHostile(CC))
                {
                    target.AddCondition<ConMarked>();
                    CC.mana.Mod(manaRestore);
                }
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect = Effect.Get("Element/ball_Holy");
            spellEffect.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > CC.pos.x);
        }

        return true;
    }
}