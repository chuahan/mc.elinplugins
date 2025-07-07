using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardDispelSong : ActBardSong
{
    public class BardDispelSong : BardSongData
    {
        public override string SongName => Constants.BardDispelSongName;
        public override int SongId => Constants.BardDispelSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            foreach (Condition item5 in target.conditions.Copy())
            {
                // Removes a single buff.
                if (item5.Type == ConditionType.Buff &&
                    !item5.IsKilled &&
                    EClass.rnd(power * 2) > EClass.rnd(item5.power) &&
                    item5 is not ConRebirth) // Don't purge Rebirth.
                {
                    CC.Say("removeBuff", TC, item5.Name.ToLower());
                    item5.Kill();
                    return;
                }
            }
            target.PlayEffect("curse");
        }
    }

    protected override BardSongData SongData => new BardDispelSong();
}