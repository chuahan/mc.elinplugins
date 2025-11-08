using PromotionMod.Common;
using PromotionMod.Stats.Runeknight;
namespace PromotionMod.Elements.PromotionAbilities.Runeknight;

public class ActSpinningSlash : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatRuneknight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.RuneknightId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        // Fetch Elemental Attunement Condition.
        // Remove Existing Elemental Attunement.
        ConElementalAttunement existingAttunement = CC.GetCondition<ConElementalAttunement>();

        // Defaults to Fire.
        int element = Constants.EleFire;
        long damage = HelperFunctions.SafeDice("runeknight_spinningslash", GetPower(CC));
        if (existingAttunement != null)
        {
            damage += existingAttunement.StoredDamage;
            element = existingAttunement.AttunedElement;
            existingAttunement.StoredDamage = 0;
        }

        // Exhaust Elemental Attunement Condition.
        // Target all enemies nearby with Magic Sword of that element doing the stored damage.
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 3F, CC, false, true))
        {
            HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, target, AttackSource.MagicSword, element);
            target.PlayEffect("hit_slash");
            target.PlaySound("ab_magicsword");
        }
        return true;
    }
}