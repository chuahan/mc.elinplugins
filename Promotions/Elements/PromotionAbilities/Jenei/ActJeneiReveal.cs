using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiReveal : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatJenei;
    public override string PromotionString => Constants.JeneiId;
    public override int AbilityId => Constants.ActJeneiRevealId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (TC is not { isChara: true }) return false;
        return true;
    }

    public override bool Perform()
    {
        Chara target = TC.Chara;
        if (CC.IsPC)
        {
            if (!target.IsHostile(CC) && !target.IsPCFaction)
            {
                if (!target.knowFav && target.isSynced)
                {
                    Msg.Say("noteFav", target);
                    target.knowFav = true;
                }
            }
            ui.AddLayer<LayerChara>().SetChara(target);
        }

        if (target.IsHostile(CC))
        {
            TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), GetPower(CC), 5));
        }
        //Msg.Nerun(TC.GetFlagValue(Constants.PromotionFeatFlag).ToString());
        return true;
    }
}