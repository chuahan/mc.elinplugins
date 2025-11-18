using PromotionMod.Elements.PromotionFeats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActMeleeShieldSmite : ActMelee
{
    public override bool AllowCounter => false;
    public override bool AllowParry => false;
    public override bool UseWeaponDist => false;

    public override int PerformDistance => 1;

    public override bool Perform()
    {
        int shieldSkill = CC.Evalue(123);
        int basherEnc = CC.Evalue(381);
        bool hasHit = AttackProcess.Current.Perform(1, false);
        if (hasHit)
        {
            int power = GetPower(CC);
            long shieldPower = FeatSentinel.GetShieldPower(CC);
            shieldPower += power;
            shieldPower += (int)(TC.MaxHP * .125F);
            TC.DamageHP(shieldPower, AttackSource.Melee, CC);
            if (TC.IsAliveInCurrentZone && TC.isChara)
            {
                if (EClass.rnd(2) == 0)
                    TC.Chara.AddCondition<ConDim>(50 + (int)Mathf.Sqrt(shieldSkill) * 10);
                TC.Chara.AddCondition<ConParalyze>(EClass.rnd(2), true);
            }
            AttackProcess.ProcShieldEncs(CC, TC, 500 + basherEnc);
        }
        return true;
    }
}