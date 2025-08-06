using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
namespace BardMod.Stats.BardSongConditions;

/*
 * Heals all allies.
 * Applies 5-20% of Max Regen of healing per turn.
 * All allies gain second chance: If they suffer a fatal blow when they have more than 33% HP
 * they will survive with 10% HP.
 * If worshipping Jure, smite nearby enemies with holy damage. Doubled against undead.
 */
public class ConLonelyTearsSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    public int GetHealingPercent()
    {
        int maxHp = owner.MaxHP;
        float powerPercent = HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 15, 5 + P2 * 10, Constants.BardPowerSlope);
        return (int)(maxHp * (powerPercent / 100));
    }

    public override void Tick()
    {
        owner.HealHP(GetHealingPercent(), HealSource.HOT);
        Mod(-1);
    }

    public override void OnWriteNote(List<string> list)
    {
        list.Add("hintBardHealingSong".lang(GetHealingPercent().ToString(CultureInfo.InvariantCulture)));
    }
}