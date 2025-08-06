using BardMod.Common;
using BardMod.Elements.BardSpells;
using BardMod.Source;
using BardMod.Stats;
namespace BardMod.Elements.Spells;

public class ActBardDefenduSong : ActBardSong
{

    protected override BardSongData SongData => new BardDefenduSong();

    public class BardDefenduSong : BardSongData
    {
        public override string SongName => Constants.BardDefenduSongName;
        public override int SongId => Constants.BardDefenduSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            // Teammates Gain Overguard + Defendu.
            int overguardAmount = ConOverguard.CalcOverguardAmount(2000 + power);
            target.AddCondition<ConOverguard>(overguardAmount);
            target.AddCondition<ConDefenduSong>();
            target.PlaySound("shield_bash");
        }
    }
}