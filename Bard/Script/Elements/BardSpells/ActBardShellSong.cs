using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardShellSong : ActBardSong
{
    public class BardShellSong : BardSongData
    {
        public override string SongName => Constants.BardShellSongName;
        public override int SongId => Constants.BardShellSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int scaledPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 10, 25, Constants.BardPowerSlope);
            target.AddCondition<ConShellSong>(scaledPower);
            target.PlaySound("shield_bash");
        }
    }

    protected override BardSongData SongData => new BardShellSong();
}