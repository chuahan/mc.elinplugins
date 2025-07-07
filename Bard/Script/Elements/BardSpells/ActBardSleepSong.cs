using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardSleepSong : ActBardSong
{
    public class BardSleepSong : BardSongData
    {
        public override string SongName => Constants.BardSleepSongName;
        public override int SongId => Constants.BardSleepSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConSleep>(power, force: true);
        }
    }

    protected override BardSongData SongData => new BardSleepSong();
}