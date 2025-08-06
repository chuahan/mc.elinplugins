using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using BardMod.Stats;
namespace BardMod.Elements.Abilities.Selena;

public class ActFlamingChord : Ability
{
    public override Cost GetCost(Chara c)
    {
        Cost result2 = default(Cost);
        result2.type = CostType.MP;

        int num = EClass.curve(Value, 50, 10);
        result2.cost = source.cost[0] * (100 + (!source.tag.Contains("noCostInc") ? num * 3 : 0)) / 100;

        // Higher Music skill will reduce mana costs.
        if (c != null)
        {
            int musicSkill = c.Chara.Evalue(Constants.MusicSkill);
            result2.cost *= 100 / (100 + musicSkill);
        }

        if ((c == null || !c.IsPC) && result2.cost > 2)
        {
            result2.cost /= 2;
        }

        return result2;
    }

    public override int GetPower(Card bard)
    {
        return HelperFunctions.GetBardPower(base.GetPower(bard), bard);
    }

    public override bool CanPerform()
    {
        if (TC == null)
        {
            return false;
        }
        return ACT.Melee.CanPerform();
    }

    public override bool Perform()
    {
        // Play Effect
        int damage = HelperFunctions.SafeDice(Constants.FlamingChordName, GetPower(CC));
        if (TC.HasCondition<ConFreeze>()) damage = HelperFunctions.SafeMultiplier(damage, 1.5f);

        // Modified Effect if Selena has enough Rhythm - Turns into a 3 strike multihit.
        bool playerRhythm = CC.Evalue(Constants.FeatTimelessSong) > 0 && CC.IsPCParty && pc.HasCondition<ConRhythm>();
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
            // TC.DamageHP(dmg: damage, ele: Constants.EleFire, eleP: 100, attackSource: AttackSource.MagicSword, origin: CC);
            BardCardPatches.CachedInvoker.Invoke(
                TC,
                new object[] { damage, Constants.EleFire, 100, AttackSource.MagicSword, CC }
            );
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