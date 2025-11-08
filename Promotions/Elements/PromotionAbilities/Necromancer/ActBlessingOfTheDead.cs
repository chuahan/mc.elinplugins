using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities;

public class ActBlessingOfTheDead : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatNecromancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.NecromancerId.lang()));
            return false;
        }
        // Must be a minion of the caster and must be undead.
        if (!TC.isChara || !TC.IsMinion || TC.Chara.master != CC || !TC.Chara.HasTag(CTAG.undead)) return false;
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
        CC.TalkRaw("necromancer_blessing_dead".langList().RandomItem());
        if (!TC.isChara || !TC.IsMinion || TC.Chara.master != CC || !TC.Chara.HasTag(CTAG.undead) || !TC.IsAliveInCurrentZone) return false;
        if (TC.Chara.id == "sister_undead")
        {
            Msg.Say("jure_angry".langGame());
            player.ModKarma(-1);
        }
        // Target's Remaining HP is scaled down.
        int healthPercent = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 40, 75);
        int healthValue = TC.Chara.hp * (healthPercent / 100);
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            // Heal HP
            int missingHP = target.MaxHP - target.hp;
            if (missingHP >= healthValue)
            {
                target.HealHP(healthValue, HealSource.Magic);
                continue;
            }
            // Overheal is converted into Protection
            target.HealHP(missingHP);
            int protection = healthValue - missingHP;
            if (target.HasCondition<ConProtection>())
            {
                target.GetCondition<ConProtection>().AddProtection(protection);
            }
            else
            {
                target.AddCondition<ConProtection>(protection);
            }
        }
        return true;
    }
}