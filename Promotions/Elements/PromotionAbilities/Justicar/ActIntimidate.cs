using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Justicar;

/// <summary>
///     Justicar Ability
///     The Justicar inflicts Armor Break and Excommunication on the target.
///     All enemies near the target are afflicted with fear.
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
        int breakAmount = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 10, 25);
        TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), GetPower(CC), breakAmount));

        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TC.pos, 5F, CC, false, false))
        {
            // Inflict AOE Bane and Fear
            ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = nameof(ConBane),
            });
            
            ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
            {
                origin = Act.CC.Chara,
                n1 = nameof(ConFear),
            });
        }

        CC.TalkRaw("justicarIntimidate".langList().RandomItem());
        return true;
    }
}