namespace PromotionMod.Elements.PromotionAbilities.Gambler;

/// <summary>
/// </summary>
public class ActMeleeDiceStrike : ActMelee
{
    public float DamageMultiOverride { get; set; }
    public bool ShouldCrit { get; set; }

    public override bool AllowCounter => !ShouldCrit;
    public override bool AllowParry => !ShouldCrit;
    public override bool ShouldRollMax => ShouldCrit;
    public override float BaseDmgMTP => DamageMultiOverride;
}