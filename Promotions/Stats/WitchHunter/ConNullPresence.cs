using UnityEngine;
namespace PromotionMod.Stats.WitchHunter;

public class ConNullPresence : BaseDebuff
{
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    public override bool CanManualRemove => false;
}