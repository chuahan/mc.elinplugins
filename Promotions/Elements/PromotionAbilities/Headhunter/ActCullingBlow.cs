using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Headhunter;

/// <summary>
/// Headhunter Ability
/// Single grievous attack with a 1/3 chance of failing.
/// If successful, applies damage, then culls the enemy if it's lower than 20% HP.
/// </summary>
public class ActCullingBlow : ActMelee
{
    public float CullThreshold = 25f;
    public override float BaseDmgMTP => 1.2f;

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHeadhunter) == 0) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        base.Perform();
        if (TC.isChara)
        {
            if (EClass.rnd(3) == 0)
            {
                if (TC.MaxHP * CullThreshold > TC.hp)
                {
                    CC.PlaySound("hit_finish");
                    CC.Say("finish");
                    CC.Say("finish2", CC, TC);
                    TC.DamageHP(TC.MaxHP, AttackSource.Finish, CC);
                }
            }
        }

        CC.AddCooldown(Constants.ActCullingBlowId, 10);
        return true;
    }
}