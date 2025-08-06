using BardMod.Common;
using BardMod.Elements.BardSpells;
using BardMod.Source;
using BardMod.Stats;
namespace BardMod.Elements.Spells;

public class ActBardQualiaSong : ActBardSong
{

    protected override BardSongData SongData => new BardQualiaSong();

    public class BardQualiaSong : BardSongData
    {
        public override string SongName => Constants.BardQualiaSongName;
        public override int SongId => Constants.BardQualiaSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConQualiaSong>(power);
        }
    }
}