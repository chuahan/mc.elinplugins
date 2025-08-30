using PromotionMod.Common;
using PromotionMod.Stats.Headhunter;
namespace PromotionMod.Elements.PromotionAbilities.Headhunter;

/// <summary>
/// Headhunter Ability
/// Melee attack that will instantly kill any enemies at or below 25% HP.
/// </summary>
public class ActExecute : Ability
{
    public float CullThreshold = 25f;

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHeadhunter) == 0) return false;
        if (CC.HasCooldown(Constants.ActExecuteId)) return false;
        if (Act.TC == null) return false;
        return ACT.Melee.CanPerform();
    }
    
    // Cost is reduced by 10% per Headhunter stack are active.
    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        if (CC != null && CC.HasCondition<ConHeadhunter>())
        {
            int reduction = (int)(cost.cost * 0.1); 
            cost.cost -= CC.GetCondition<ConHeadhunter>().power * reduction;
        }
        return cost;
    }

    public override bool Perform()
    {
        // Perform Melee Attack
        new ActMelee().Perform(Act.CC, Act.TC);
        
        // Cull enemy if possible.
        if (TC.MaxHP * CullThreshold > TC.hp)
        {
            CC.PlaySound("hit_finish");
            CC.Say("finish");
            CC.Say("finish2", CC, TC);
            TC.DamageHP(TC.MaxHP, AttackSource.Finish, CC);
            
            // If the cull was successful, do not add a cooldown and refund stamina cost.
            CC.stamina.Mod(this.GetCost(CC).cost);
            return true;
        }
        
        CC.AddCooldown(Constants.ActExecuteId, 10);
        return true;
    }
}