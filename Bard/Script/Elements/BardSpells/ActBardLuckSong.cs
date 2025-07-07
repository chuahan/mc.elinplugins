using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardLuckSong : ActBardSong
{
    public class BardLuckSong : BardSongData
    {
        public override string SongName => Constants.BardLuckSongName;
        public override int SongId => Constants.BardLuckSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int luckPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 50, 300, Constants.BardPowerSlope);
            target.AddCondition<ConLuckSong>(luckPower);
        }
    }

    protected override BardSongData SongData => new BardLuckSong();
}