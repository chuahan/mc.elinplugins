using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
namespace BardMod.Stats.BardSongConditions;

public class ConEndlessBlossomsSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    public float CalcRetaliatePercent()
    {
        return HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 5, 10 + P2 * 10, Constants.BardPowerSlope);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintEndlessBloom".lang(CalcRetaliatePercent().ToString(CultureInfo.InvariantCulture)));
    }
}