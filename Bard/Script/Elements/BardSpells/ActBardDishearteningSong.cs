using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardDishearteningSong : ActBardSong
{
    private class BardDishearteningSong : BardSongData
    {
        public override string SongName => Constants.BardDishearteningSongName;
        public override int SongId => Constants.BardDishearteningSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            target.AddCondition<ConDisheartenSong>(power);
            target.PlayEffect("curse");
        }
    }

    protected override BardSongData SongData => new BardDishearteningSong();
}