using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.WitchHunter;

public class ActManaBreak : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWitchHunter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WitchHunterId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActManaBreakId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.SP,
            cost = 10
        };
    }

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

        CC.AddCooldown(Constants.ActManaBreakId, 10);
        return true;
    }
}