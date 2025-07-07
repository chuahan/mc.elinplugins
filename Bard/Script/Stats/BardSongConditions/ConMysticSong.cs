using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * This spell restores % MP every 5 ticks. Starts at 2% and goes till 15% per tick.
 * This spell also boosts spell enhance and EDR by up to 40/25 respectively.
 */
public class ConMysticSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Buff;
    
    public int GetRestorationPercent()
    {
        int maxMP = owner.mana.max;
        float powerPercent = HelperFunctions.SigmoidScaling(base.power, Constants.MaxBardPowerBuff, 2, 15, Constants.BardPowerSlope);
        return (int)(maxMP * (powerPercent / 100));
    }
    
    public override void Tick()
    {
        if (this.value % 5 == 0)
        {
            owner.mana.Mod(this.GetRestorationPercent());
        }
        Mod(-1);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintBardMysticSong".lang(HelperFunctions.SigmoidScaling(base.power, Constants.MaxBardPowerBuff, 2, 15, Constants.BardPowerSlope).ToString(CultureInfo.InvariantCulture)));
    }
}