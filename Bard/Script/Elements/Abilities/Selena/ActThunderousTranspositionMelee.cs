using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
namespace BardMod.Elements.Abilities.Selena;

public class ActThunderousTranspositionMelee : ActMelee
{

    public bool SpecialActive;
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;

    public override int GetPower(Card bard)
    {
        return HelperFunctions.GetBardPower(base.GetPower(bard), bard);
    }

    public override bool Perform()
    {
        // Modified Effect if Selena has enough Rhythm - Inflicts Freeze on all targets.
        // Deal Lightning damage to all nearby enemies.
        long damage = HelperFunctions.SafeDice(Constants.ThunderousTranspositionName, GetPower(CC));
        if (TC.HasCondition<ConFreeze>()) damage = (long)(damage * 1.5f);
        TC.DamageHP(dmg: damage, ele: Constants.EleLightning, eleP: 100, attackSource: AttackSource.Melee, origin: CC);
        if (SpecialActive && TC.isChara) TC.Chara.AddCondition<ConFreeze>(force: true);
        return true;
    }
}