using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells;

public class ActBardDisorientationSong : ActBardSong
{

    protected override BardSongData SongData => new BardDisorientationSong();

    public class BardDisorientationSong : BardSongData
    {
        public override string SongName => Constants.BardDisorientationSongName;
        public override int SongId => Constants.BardDisorientationSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConDazedSong>(power);
            target.PlayEffect("curse");
        }
    }
}