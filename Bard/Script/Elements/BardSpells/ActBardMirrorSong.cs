using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardMirrorSong : ActBardSong
{
    public class BardMirrorSong : BardSongData
    {
        public override string SongName => Constants.BardMirrorSongName;
        public override int SongId => Constants.BardMirrorSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConMirrorSong>(power);
            target.PlaySound("shield_bash");
        }
    }

    protected override BardSongData SongData => new BardMirrorSong();
}