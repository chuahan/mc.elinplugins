using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
namespace BardMod.Elements.Abilities.Selena;

public class ActFlamingChord : BardAbility
{
    public override bool CanPerform()
    {
        if (Act.TC == null)
        {
            return false;
        }
        return ACT.Melee.CanPerform();
    }
    
    public override bool Perform()
    {
        // Play Effect
        int damage = HelperFunctions.SafeDice(Constants.FlamingChordName, this.GetPower(CC));
        if (TC.HasCondition<ConFreeze>()) damage = HelperFunctions.SafeMultiplier(damage, 1.5f);
        
        // Modified Effect if Selena has enough Rhythm - Turns into a 3 strike multihit.
        bool playerRhythm = CC.Evalue(Constants.FeatTimelessSong) > 0 && CC.IsPCParty && EClass.pc.HasCondition<ConRhythm>();
        ConRhythm? rhythm = CC.GetCondition<ConRhythm>();
        bool specialEffect = false;
        if (playerRhythm)
        {
            specialEffect = true;
        }
        else if (rhythm != null)
        {
            if (rhythm.GetStacks() >= 10)
            {
                rhythm.ModStacks(-10);
                specialEffect = true;
            }
        }

        int strikes = specialEffect ? 3 : 1;
        for (int i = 0; i < strikes; i++)
        {
            TC.DamageHP(damage, Constants.EleFire, eleP: 100, AttackSource.MagicSword, CC);
            TC.PlaySound("ab_magicsword");
            TC.PlayEffect("hit_slash").SetScale(1f);
        }

        // If it's not the PC, add Rhythm.
        if (!CC.IsPC)
        {
            rhythm ??= CC.AddCondition<ConRhythm>() as ConRhythm;
            rhythm?.ModStacks(3);   
        }

        return true;
    }
}