using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
using UnityEngine;

namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActDivineFist : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatWarCleric) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.WarClericId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActDivineFistId)) return false;
        return base.CanPerform();
    }
    
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        // Divine Descent Removes cost from this ability
        if (CC.HasCondition<ConDivineDescent>()) convertToMp.cost = 0;
        convertToMp.type = CostType.MP;
        return convertToMp;
    }
    
    public override bool Perform()
    {
        bool divineDescentActive = CC.HasCondition<ConDivineDescent>(); 
        // Divine Descent Removes cooldown from this ability and increases damage from the melee attack by 30%.
        Condition solBlade = CC.AddCondition<ConSolBlade>();
        // Perform a melee attack against the enemy.
        float dmgMulti = 1.0F;
        if (divineDescentActive) dmgMulti = 1.3F;
        Attack(dmgMulti);
        solBlade.Kill();
        // Render Holy Arrows flying at nearby targets
        int boltCount = 4;
        ElementRef elementRef = setting.elements["eleHoly"];
        Effect arrowEffect = Effect.Get("spell_arrow");
        arrowEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = arrowEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;

        int power = this.GetPower(CC);
        int boltDamage = HelperFunctions.SafeDice("warcleric_divine_fist", power);
        foreach (Chara target in EClass.pc.currentZone.map.ListCharasInCircle(TC.pos, 3F, true))
        {
            // Doesn't proc on user or original target.
            if (target == TC || target == CC) continue;
            if (target.IsHostile(CC))
            {
                HelperFunctions.ProcSpellDamage(power, boltDamage, CC, target, ele:Constants.EleHoly, eleP: 50);
            }
            else
            {
                target.HealHP(boltDamage, HealSource.Magic);   
            }
            arrowEffect.Play(CC.pos, 0f, target.pos);
        }
        
        if (!divineDescentActive)
        {
            CC.AddCooldown(Constants.ActDivineFistId, 5);   
        }
        return true;
    }
}