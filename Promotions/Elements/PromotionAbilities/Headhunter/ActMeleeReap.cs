using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Headhunter;

public class ActMeleeReap : ActMelee
{
    public override float BaseDmgMTP
    {
        get
        {
            float damageMulti = 1.0f;
            if (TC != null && TC.isChara)
            {
                if (TC.Chara.conditions.FirstOrDefault(con => con.Type == ConditionType.Bad) != null) damageMulti += 0.25F;
                if (TC.Chara.MaxHP == TC.Chara.hp) damageMulti += 1F;
                
            }
            return damageMulti;
        }
    }
}