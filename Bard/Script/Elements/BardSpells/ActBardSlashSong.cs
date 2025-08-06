using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using BardMod.Source;
namespace BardMod.Elements.BardSpells;

public class ActBardSlashSong : ActBardSong
{

    protected override BardSongData SongData => new BardSlashSong();

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
            BardCardPatches.CachedInvoker.Invoke(
                target,
                new object[] { damage, Constants.EleSound, 100, AttackSource.Shockwave, bard }
            );
        }
    }
}