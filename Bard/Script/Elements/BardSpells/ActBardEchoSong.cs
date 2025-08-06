using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells;

public class ActBardEchoSong : ActBardSong
{

    protected override BardSongData SongData => new BardEchoSong();

    public class BardEchoSong : BardSongData
    {
        public override string SongName => Constants.BardEchoSongName;
        public override int SongId => Constants.BardEchoSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            List<ConBardSong> bardConditions = target.conditions.OfType<ConBardSong>().Where(c => c.SongType != Constants.BardSongType.Finale).ToList();
            foreach (ConBardSong condition in bardConditions)
            {
                condition.RefreshBardSong();
            }
            target.PlayEffect("buff");
        }
    }
}