using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
/// Justicar Ability
/// The Justicar inflicts Armor Break and Excommunication on the target.
/// All enemies near the target are afflicted with fear.
/// </summary>
public class ActIntimidate : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJusticar) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JusticarId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConArmorBreak>(this.GetPower(CC));
        TC.Chara.AddCondition<ConExcommunication>(this.GetPower(CC));
        
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TC.pos, 5F, CC, false, false))
        {
            target.AddCondition<ConFear>(this.GetPower(CC));
        }
        
        CC.TalkRaw("justicarIntimidate".langList().RandomItem());
        return true;
    }
}