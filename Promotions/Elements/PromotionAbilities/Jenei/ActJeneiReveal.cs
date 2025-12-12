using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

public class ActJeneiReveal : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatJenei) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.JeneiId.lang()));
            return false;
        }
        if (TC == null || !TC.isChara) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
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
                ui.AddLayer<LayerChara>().SetChara(target);
            }
            else
            {
                TC.Chara.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), GetPower(CC), 5));
            }
        }
        return true;
    }
}