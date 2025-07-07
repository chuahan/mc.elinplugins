using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardDisruptionSong : ActBardSong
{
    private class BardDisruptionSong : BardSongData
    {
        public override string SongName => Constants.BardDisruptionSongName;
        public override int SongId => Constants.BardDisruptionSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            // Adds status to hamper spellcasting abilities.
            target.AddCondition<ConDisruptionSong>(power);
            target.PlayEffect("curse");
        }
    }

    protected override BardSongData SongData => new BardDisruptionSong();
}