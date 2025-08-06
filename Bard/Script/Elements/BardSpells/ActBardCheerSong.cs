using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells;

public class ActBardCheerSong : ActBardSong
{

    protected override BardSongData SongData => new BardCheerSong();

    public class BardCheerSong : BardSongData
    {
        public override string SongName => Constants.BardCheerSongName;
        public override int SongId => Constants.BardCheerSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int cheerPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 10, 25, Constants.BardPowerSlope);
            target.AddCondition<ConCheerSong>(cheerPower);
        }
    }
}