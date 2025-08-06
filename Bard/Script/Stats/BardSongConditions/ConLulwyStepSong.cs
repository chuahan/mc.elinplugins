using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
namespace BardMod.Stats.BardSongConditions;

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
        return (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 10, P2 * 10, Constants.BardPowerSlope);
    }

    public int CalcRangedDodge()
    {
        switch (P2)
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
        switch (P2)
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
        list.Add("hintLulwyStep1".lang(CalcRangedDodge().ToString()));
        list.Add("hintLulwyStep2".lang(CalcMeleeDodge().ToString()));
    }
}