using PromotionMod.Common;
using PromotionMod.Stats.Runeknight;
namespace PromotionMod.Elements.PromotionAbilities.Runeknight;

public class ActSpinningSlash : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatRuneKnight;
    public override string PromotionString => Constants.RuneKnightId;
    public override int AbilityId => Constants.ActSpinningSlashId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (CC.HasCondition<ConElementalAttunement>() == false)
        {
            if (CC.IsPC && verbose) Msg.Say("runeknight_spinningslash_noattunement".langGame());
            return false;
        }
        return true;
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
        existingAttunement.Kill();
        return true;
    }
}