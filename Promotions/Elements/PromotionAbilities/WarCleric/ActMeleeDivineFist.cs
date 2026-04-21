namespace PromotionMod.Elements.PromotionAbilities.Gambler;

public class ActMeleeDivineFist : ActMelee
{
    public float DamageMultiOverride { get; set; }

    public override float BaseDmgMTP => DamageMultiOverride;
}