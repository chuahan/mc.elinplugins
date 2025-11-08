using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActMeleeLifebreak : ActMelee
{
    public override bool AllowCounter => false;
    public override bool AllowParry => false;
    public override bool UseWeaponDist => true;
    
    public override bool Perform()
    {
        bool hasHit = AttackProcess.Current.Perform(1, false);
        if (hasHit)
        {
            int damage = CC.Chara.MaxHP - CC.Chara.hp;
            damage = HelperFunctions.SafeMultiplier(damage, 1.3F);
            TC.DamageHP(damage, AttackSource.Melee, CC);
        }
        return true;
    }
}