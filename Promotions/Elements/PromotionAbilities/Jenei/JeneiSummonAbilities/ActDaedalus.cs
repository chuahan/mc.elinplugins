using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Fire. Fires missiles at all targets. Count as min distance always.
/// </summary>
public class ActDaedalus : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.15F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 5F, cc, false, true);
        float delay = 0f;
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX - How do I render rockets?
            TweenUtil.Delay(delay, delegate
            {
                cc.PlayEffect("laser").GetComponent<SpriteBasedLaser>().Play(targets[i].pos.PositionCenter());
                cc.PlaySound("bullet_drop");
                cc.PlayEffect("bullet").Emit(1);
            });
            if (delay < 1f)
            {
                delay += 0.07f;
            }

            cc.PlayEffect("laser").GetComponent<SpriteBasedLaser>().Play(targets[i].pos.PositionCenter());
            // Do Damage.
            int damage = CalculateDamage(power, 0, targets[i]);
            HelperFunctions.ProcSpellDamage(power, damage, cc, targets[i], ele: Constants.EleFire);
        }

        return true;
    }
}