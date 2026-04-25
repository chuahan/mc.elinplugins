using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActManaBreak : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatWitchHunter;
    public override string PromotionString => Constants.WitchHunterId;
    public override int AbilityId => Constants.ActManaBreakId;

    public override bool Perform()
    {
        int missingMana = TC.Chara.mana.max - TC.Chara.mana.value;
        int damage = HelperFunctions.SafeMultiplier(missingMana, 1.3F);
        // Cause a magic explosion that also damages other enemies in range.
        List<Chara> targetsHit = new List<Chara>();
        Effect spellEffect = Effect.Get("Element/ball_Magic");
        foreach (Point tile in _map.ListPointsInCircle(TC.pos, 3f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(CC)))
            {
                HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleMagic);
                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion,
            float delay = distance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(tile, 0f, tile);
            });
        }
        return true;
    }
}