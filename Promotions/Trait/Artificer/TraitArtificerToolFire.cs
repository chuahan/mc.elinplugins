using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolFire : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_firesword";

    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        int damage = HelperFunctions.SafeDice(ArtificerToolId, power);
        List<Chara> targets = pos.Charas;
        pos.PlayEffect("hit_slash");
        pos.PlaySound("ab_magicsword");
        foreach(Chara target in targets)
        {
            if (target.IsHostile(cc))
            {
                if (target.HasCondition<ConBurning>())
                {
                    damage = HelperFunctions.SafeMultiplier(damage, 1.5F);
                }
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, AttackSource.MagicSword, ele: Constants.EleFire);
                HelperFunctions.ApplyElementalBreak(Constants.EleFire, cc, target, power);
                ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
                {
                    origin = Act.CC.Chara,
                    n1 = nameof(ConBurning),
                });
            }
        }
        owner.c_ammo--;
        return true;
    }
}