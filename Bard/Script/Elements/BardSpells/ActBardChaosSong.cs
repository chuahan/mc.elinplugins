using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells;

public class ActBardChaosSong : ActBardSong
{

    protected override BardSongData SongData => new BardChaosSong();

    private class BardChaosSong : BardSongData
    {
        public override string SongName => Constants.BardChaosSongName;
        public override int SongId => Constants.BardChaosSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            // TODO: Add SFX
            // TODO: Add FX
            target.AddCondition<ConChaosSong>(power, true);
            target.PlayEffect("curse");
        }
    }
}