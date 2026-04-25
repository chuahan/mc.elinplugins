using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Saint;

public class ActHandOfGod : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatSaint;
    public override string PromotionString => Constants.SaintId;
    public override int AbilityId => Constants.ActHandOfGodId;

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, false))
        {
            int power = GetPower(CC) + CC.Evalue(SKILL.faith);
            target.HealHP(power, HealSource.Magic);
            target.Chara.AddCondition<ConGreaterRegen>(power);
        }
        return true;
    }
}