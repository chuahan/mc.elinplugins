
using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitBookForgetCombat : TraitScroll
{
    public override bool CanRead(Chara c)
    {
        return c.GetInt(Constants.AdvancedCombatSkillFlag, 0) != 0 && base.CanRead(c);
    }

    public override void OnRead(Chara c)
    {
        ForgetCombatSkill(c);
    }


    public void ForgetCombatSkill(Chara c)
    {
        // Get the Adv Combat Skill Feat from the Feat Flag.
        int combatSkillFeat = c.GetInt(Constants.AdvancedCombatSkillFlag, 0);
        c.SetFeat(combatSkillFeat, 0, true);
        c.SetInt(Constants.AdvancedCombatSkillFlag, 0);
        Msg.Say("combatskillforgotten".lang(c.NameSimple, sources.elements.map[combatSkillFeat].GetName()));

        c.PlaySound("curse");
        c.PlayEffect("aura_heaven");
        c.Say("spellbookCrumble", owner.Duplicate(1));
        owner.ModNum(-1);
    }
}