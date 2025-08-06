using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells;

public class ActBardVigorSong : ActBardSong
{

    protected override BardSongData SongData => new BardVigorSong();

    public class BardVigorSong : BardSongData
    {
        public override string SongName => Constants.BardVigorSongName;
        public override int SongId => Constants.BardVigorSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int vigorPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 25, 200, Constants.BardPowerSlope);
            target.AddCondition<ConVigorSong>(vigorPower);
        }
    }
}