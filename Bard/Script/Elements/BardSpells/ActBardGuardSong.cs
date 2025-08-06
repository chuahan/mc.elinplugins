using BardMod.Common;
using BardMod.Source;
using BardMod.Stats;
namespace BardMod.Elements.BardSpells;

public class ActBardGuardSong : ActBardSong
{

    protected override BardSongData SongData => new BardGuardSong();

    public class BardGuardSong : BardSongData
    {
        public override string SongName => Constants.BardGuardSongName;
        public override int SongId => Constants.BardGuardSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int overguardAmount = ConOverguard.CalcOverguardAmount(power);
            target.AddCondition<ConOverguard>(overguardAmount);
            target.PlaySound("shield_bash");
        }
    }
}