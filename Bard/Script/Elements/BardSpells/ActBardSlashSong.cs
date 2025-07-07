using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardSlashSong : ActBardSong
{
    public class BardSlashSong : BardSongData
    {
        public override string SongName => Constants.BardSlashSongName;
        public override int SongId => Constants.BardSlashSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int damage = HelperFunctions.SafeDice(Constants.BardSlashSongName, power);
            target.PlaySound("ab_magicsword");
            target.DamageHP(damage, Constants.EleSound, eleP: 100, AttackSource.Shockwave, bard);
        }
    }

    protected override BardSongData SongData => new BardSlashSong();
}