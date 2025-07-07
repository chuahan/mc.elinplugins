using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * True dodge effect.
 * Melee attacks start at 25% miss chance, scale up to 50% chance.
 * Ranged attacks start out at 75% miss chance, scales up to 100%.
 * If the bard follows Lulwy, on dodging an attack, Lulwy will retaliate by attacking and stunning the enemy.
 *
 * Boost+:
 * Percentage Based Boost.
 * Starts at 100% Speed Multiplier up to 200%.
 * Evasion and Perfect Evasion.
 */
public class ConLulwyStepSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public override int EvaluatePower(int calcPower)
    {
        return EClass.curve(calcPower, 400, 100);
    }
    
    public int GetRetaliationPower()
    {
        return (int)HelperFunctions.SigmoidScaling(base.power, Constants.MaxBardPowerBuff, 10, P2 * 10, Constants.BardPowerSlope);
    }
    
    public int CalcRangedDodge()
    {
        switch (this.P2)
        {
            case 2:
                return 75;
            case 3:
                return 100;
        }

        return 50;
    }

    public int CalcMeleeDodge()
    {
        switch (this.P2)
        {
            case 2:
                return 30;
            case 3:
                return 50;
        }

        return 10;
    }
    
    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintLulwyStep1".lang(this.CalcRangedDodge().ToString()));
        list.Add("hintLulwyStep2".lang(this.CalcMeleeDodge().ToString()));
    }
}