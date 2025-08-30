using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Impact. 5% chance of instantly killing target.
/// </summary>
public class ActCharon : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.3F;
    
    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX: Nether ball.
            Effect spellEffect = Effect.Get("Element/ball_Nether");
            spellEffect.Play(CC.pos, 0f, targets[i].pos);

            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, element: Constants.EleImpact);
            
            // 1/20 chance of Instakill
            if (EClass.rnd(20) == 0 && targets[i].IsAliveInCurrentZone)
            {
                CardDamageHPPatches.CachedInvoker.Invoke(
                    targets[i], targets[i].MaxHP, Constants.EleNether, 100, AttackSource.Finish, CC);
            }
        }
        
        return true;
    }
}