using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiReveal : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0) return false;
        if (!TC.isChara) return false;
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
        else
        {
            TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), GetPower(CC), 5));
        }
        return true;
    }
}