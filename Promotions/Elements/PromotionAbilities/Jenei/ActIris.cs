using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Fire. Massive solar explosion that does full damage. Revives all allies. Restores all allies to full health.
/// </summary>
public class ActIris : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.4F;
    
    public override bool Perform()
    {
        // Revive all fallen allies if Iris is allied to PC.
        if (CC.IsPCPartyMinion)
        {
            List<KeyValuePair<int, Chara>> deadAllies = EClass.game.cards.globalCharas.Where((KeyValuePair<int, Chara> c) => c.Value.isDead && c.Value.faction == EClass.pc.faction && !c.Value.isSummon && c.Value.c_wasInPcParty).ToList();
            foreach (KeyValuePair<int, Chara> c in deadAllies)
            {
                c.Value.Chara.GetRevived();
            }
        }
        Effect spellEffect = Effect.Get("Element/ball_Fire");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(CC.pos, 5f, mustBeWalkable: false, los:false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas())
            {
                if (!targetsHit.Contains(target))
                {
                    if (target.IsHostile(CC))
                    {
                        // Damage Target
                        int damage = this.CalculateDamage(this.GetPower(CC), distance, target);
                        HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, element: Constants.EleFire);
                    }
                    else
                    {
                        // Heal any allies
                        target.HealHP(target.MaxHP, HealSource.Magic);
                    }   
                }
                        
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