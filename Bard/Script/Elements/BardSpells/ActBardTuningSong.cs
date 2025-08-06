using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells;

public class ActBardTuningSong : ActBardSong
{

    protected override BardSongData SongData => new BardTuningSong();

    private class BardTuningSong : BardSongData
    {
        public override string SongName => Constants.BardTuningSongName;
        public override int SongId => Constants.BardTuningSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            float statReduction = HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 1, 5, Constants.BardPowerSlope);

            int hexCount = 0;
            foreach (Condition item5 in target.conditions.Copy())
            {
                if (item5.Type == ConditionType.Debuff && !item5.IsKilled)
                {
                    hexCount++;
                }
            }

            int tuneAmount = (int)(statReduction * hexCount);
            target.AddCondition<ConTuningSong>(tuneAmount, true);
        }
    }
}