using UnityEngine;
namespace PromotionMod.Stats.WitchHunter;

public class ConNullPresence : BaseDebuff
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
    public override bool CanManualRemove => false;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}