using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
namespace BardMod.Elements.BardSpells;

public class ActBardWitchHuntSong : ActBardSong
{

    protected override BardSongData SongData => new BardWitchHuntSong();

    public class BardWitchHuntSong : BardSongData
    {
        public override string SongName => Constants.BardWitchHuntSongName;
        public override int SongId => Constants.BardWitchHuntSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            float amount = HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 25, 75, Constants.BardPowerSlope);
            int maxMana = target.mana.max;
            int manaDrain = -1 * (int)(maxMana * amount);
            target.mana.Mod(manaDrain);
            target.PlaySound("gravity");
        }
    }
}