using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardHealingSong : ActBardSong
{
    public class BardHealingSong : BardSongData
    {
        public override string SongName => Constants.BardHealingSongName;
        public override int SongId => Constants.BardHealingSongId;
        
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;
        
        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConHealingSong>(power);
        }
    }

    protected override BardSongData SongData => new BardHealingSong();
}