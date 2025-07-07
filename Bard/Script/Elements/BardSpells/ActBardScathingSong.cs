using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardScathingSong : ActBardSong
{
    private class BardScathingSong : BardSongData
    {
        public override string SongName => Constants.BardScathingSongName;
        public override int SongId => Constants.BardScathingSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.VerseSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Verse;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            // Inflicts luck down + does mind damage in AOE.
            int scaledPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 100, 400, Constants.BardPowerSlope);
            target.AddCondition<ConScathingSong>(scaledPower);
            
            int damage = HelperFunctions.SafeDice(Constants.BardScathingSongName, power);
            target.DamageHP(damage, Constants.EleMind, 100, AttackSource.None, bard);
            
        }
    }

    protected override BardSongData SongData => new BardScathingSong();
}