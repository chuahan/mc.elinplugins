using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
namespace BardMod.Stats.BardSongConditions;

// This spell heals % per tick. Starts at 2% and goes till 15% per tick.
public class ConHealingSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
    public override ConditionType Type => ConditionType.Buff;

    public int GetHealingPercent()
    {
        int maxHp = owner.MaxHP;
        float powerPercent = HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 2, 15, Constants.BardPowerSlope);
        return (int)(maxHp * (powerPercent / 100));
    }

    public override void Tick()
    {
        owner.HealHP(GetHealingPercent(), HealSource.HOT);
        Mod(-1);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintBardHealingSong".lang(HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 2, 15, Constants.BardPowerSlope).ToString(CultureInfo.InvariantCulture)));
    }
}