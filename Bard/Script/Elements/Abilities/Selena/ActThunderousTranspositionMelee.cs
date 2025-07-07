using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Elements.Abilities.Selena;

public class ActThunderousTranspositionMelee : ActMelee
{
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;

    public bool SpecialActive;
    
    public override bool Perform()
    {
        // Modified Effect if Selena has enough Rhythm - Inflicts Freeze on all targets.
        // Deal Lightning damage to all nearby enemies.
        int damage = HelperFunctions.SafeDice(Constants.ThunderousTranspositionName, this.GetPower(CC));
        if (TC.HasCondition<ConFreeze>()) damage = HelperFunctions.SafeMultiplier(damage, 1.5f);
        TC.DamageHP(damage, Constants.EleLightning, 100, AttackSource.Melee, CC);
        if (SpecialActive && TC.isChara) TC.Chara.AddCondition<ConFreeze>(force: true);
        return true;
    }
}