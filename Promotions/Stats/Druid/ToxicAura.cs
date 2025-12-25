namespace PromotionMod.Stats.Druid;

public class ToxicAura : ConAura
{
    public override bool CanManualRemove => false;
    public override AuraType AuraTarget => AuraType.Foe;
    public override void ApplyFoe(Chara target)
    {
        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, CC, target, target.pos, true, new ActRef
        {
            origin = CC,
            n1 = nameof(ConPoison)
        });
    }
}