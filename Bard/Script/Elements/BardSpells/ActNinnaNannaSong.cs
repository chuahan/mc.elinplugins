using BardMod.Common;
using BardMod.Elements.BardSpells;
using BardMod.Source;
using BardMod.Stats;
namespace BardMod.Elements.Spells;

public class ActNinnaNannaSong : ActBardSong
{
    public class BardNinnaNannaSong : BardSongData
    {
        public override string SongName => Constants.BardNinnaNannaSongName;
        public override int SongId => Constants.BardNinnaNannaSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConComatose>(force: true);
        }
    }
    
    protected override BardSongData SongData => new BardNinnaNannaSong();
}