using System;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardHollowSymphonyFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleHollowSymphonyName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        int sigmoidPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 50, 100, Constants.BardPowerSlope);
        ConHollowSymphonySong bardBuff = ConBardSong.Create(nameof(ConHollowSymphonySong), sigmoidPower, rhythmStacks, godBlessed, bard) as ConHollowSymphonySong;
        target.AddCondition(bardBuff);
        
        target.Cure(CureType.Death);
        target.hp = target.MaxHP;
        target.mana.value = target.mana.max;
        target.Refresh();
        target.PlayEffect("heal_tick");
    }
}