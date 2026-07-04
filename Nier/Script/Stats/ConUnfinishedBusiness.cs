using UnityEngine;
namespace NierMod.Stats;

internal class ConUnfinishedBusiness : BaseDebuff
{
    public override bool AllowMultipleInstance => false;

    public override ConditionType Type => ConditionType.Debuff;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override int GetPhase()
    {
        return 0;
    }

    public override bool CanStack(Condition c)
    {
        return false;
    }
}