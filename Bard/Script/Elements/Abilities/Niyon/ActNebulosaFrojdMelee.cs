using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Elements.Abilities;
using BardMod.Stats;
namespace BardMod.Elements.Abilities.Niyon;

public class ActNebulosaFrojdMelee : ActMelee
{
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;
    
    public override bool Perform()
    {
        // Deal Impact damage
        // Apply Charmed
        // TODO: CUSTOM EFFECT?
        int damage = HelperFunctions.SafeDice(Constants.NebulosaFrojdName, this.GetPower(CC));
        TC.DamageHP(damage, Constants.EleImpact, 100, AttackSource.None, CC);
        if (TC.isChara) TC.Chara.AddCondition<ConCharmed>(force: true);
        
        return true;
    }
}