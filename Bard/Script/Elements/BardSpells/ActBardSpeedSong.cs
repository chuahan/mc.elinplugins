using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
using UnityEngine;

namespace BardMod.Elements.BardSpells;

public class ActBardSpeedSong : ActBardSong
{
    public class BardSpeedSong : BardSongData
    {
        public override string SongName => Constants.BardSpeedSongName;
        public override int SongId => Constants.BardSpeedSongId;
        
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;
        
        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int scaledPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 10, 30, Constants.BardPowerSlope);
            target.AddCondition<ConSpeedSong>(scaledPower);
        }
    }

    protected override BardSongData SongData => new BardSpeedSong();
}