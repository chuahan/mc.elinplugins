namespace PromotionMod.Trait.QuestTraits;

public class TraitSnagBall : global::Trait
{
    public Chara CapturedTarget
    {
        get => owner.GetObj<Chara>(8);
        set => owner.SetObj(8, value);
    }

    public override bool IsThrowMainAction => CapturedTarget == null;

    public override ThrowType ThrowType => ThrowType.MonsterBall;

    public override EffectDead EffectDead => EffectDead.None;

    public override void OnCreate(int lv)
    {
        owner.SetLv(1 + EClass.rnd(lv + 10));
    }

    public override bool CanStackTo(Thing to)
    {
        return false;
    }

    public override void SetName(ref string text)
    {
        text = CapturedTarget == null ? "snagball_empty".lang() : "snagball_full".lang();
    }
}