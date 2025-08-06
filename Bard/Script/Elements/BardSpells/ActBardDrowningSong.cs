using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
namespace BardMod.Elements.BardSpells;

public class ActBardDrowningSong : ActBardSong
{

    protected override BardSongData SongData => new BardDrowningSong();

    public class BardDrowningSong : BardSongData
    {
        public override string SongName => Constants.BardDrowningSongName;
        public override int SongId => Constants.BardDrowningSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int suffocationPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 200, 1000, Constants.BardPowerSlope);
            target.AddCondition<ConSuffocation>(suffocationPower, true);
            target.AddCondition<ConSilence>(power);
            target.PlayEffect("water");
        }
    }
}