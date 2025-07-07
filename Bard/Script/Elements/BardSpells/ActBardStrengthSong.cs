using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardStrengthSong : ActBardSong
{
    private class BardStrengthSong : BardSongData
    {
        public override string SongName => Constants.BardStrengthSongName;
        public override int SongId => Constants.BardStrengthSongId;
    
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;
    
        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int scaledPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 10, 30, Constants.BardPowerSlope);
            target.AddCondition<ConStrengthSong>(scaledPower);
        }
    }

    protected override BardSongData SongData => new BardStrengthSong();
}