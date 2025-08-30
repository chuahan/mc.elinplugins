using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Fire. Fires missiles at all targets. Count as min distance always.
/// </summary>
public class ActDaedalus : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.15F;
    
    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        float delay = 0f;
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX - How do I render rockets?
            TweenUtil.Delay(delay, delegate
            {
                CC.PlayEffect("laser").GetComponent<SpriteBasedLaser>().Play(targets[i].pos.PositionCenter());
                CC.PlaySound("bullet_drop");
                CC.PlayEffect("bullet").Emit(1);
            });
            if (delay < 1f)
            {
                delay += 0.07f;
            }
            
            CC.PlayEffect("laser").GetComponent<SpriteBasedLaser>().Play(targets[i].pos.PositionCenter());
            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), 0, targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, element: Constants.EleFire);
        }
        
        return true;
    }
}