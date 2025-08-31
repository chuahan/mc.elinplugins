using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
/// Justicar Ability
/// The Justicar inflicts Attack Break, Suppress, and Excommunication on the target.
/// </summary>
public class ActSubdue : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJusticar) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JusticarId.lang()));
            return false;
        }
        return ACT.Melee.CanPerform();
    }

    public override bool Perform()
    {
        int attackBreakAmount = (int)HelperFunctions.SigmoidScaling(this.GetPower(CC), 10, 25);
        TC.Chara.AddCondition<ConAttackBreak>(attackBreakAmount);
        TC.Chara.AddCondition<ConSupress>(this.GetPower(CC));
        TC.Chara.AddCondition<ConExcommunication>(this.GetPower(CC));
        
        CC.TalkRaw("justicarIntimidate".langList().RandomItem());
        return true;
    }
}