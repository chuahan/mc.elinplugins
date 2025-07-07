using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * Gains 100% Crit chance. Gains half armor penetration.
 * Character gain % retaliate. On taking damage, % of that is done as damage to the attacker first.
 * Even if character dies the damage is dealt first.
 */
public class ConEndlessBlossomsSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public float CalcRetaliatePercent()
    {
        return HelperFunctions.SigmoidScaling(base.power, Constants.MaxBardPowerBuff, 5, 10 + P2 * 10, Constants.BardPowerSlope);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintEndlessBloom".lang(this.CalcRetaliatePercent().ToString(CultureInfo.InvariantCulture)));
    }
}