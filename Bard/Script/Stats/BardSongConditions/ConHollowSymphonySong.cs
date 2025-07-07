using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

// Basic stat buff finale.
public class ConHollowSymphonySong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public override int EvaluatePower(int calcPower)
    {
        return (int)HelperFunctions.SigmoidScaling(calcPower, Constants.MaxBardPowerBuff, 5, P2*10, Constants.BardPowerSlope);
    }
}