using BardMod.Common;
using BardMod.Source;
namespace BardMod.Elements.BardSpells;

public class ActBardPuritySong : ActBardSong
{

    protected override BardSongData SongData => new BardPuritySong();

    public class BardPuritySong : BardSongData
    {
        public override string SongName => Constants.BardPuritySongName;
        public override int SongId => Constants.BardPuritySongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            foreach (Condition item5 in target.conditions.Copy())
            {
                if (item5.Type == ConditionType.Debuff &&
                    !item5.IsKilled &&
                    EClass.rnd(power * 2) > EClass.rnd(item5.power) &&
                    item5 is not ConWrath && // Don't purge Wrath of God.
                    item5 is not ConDeathSentense) // Don't purge Death Sentence.
                {
                    CC.Say("removeHex", TC, item5.Name.ToLower());
                    item5.Kill();
                }
            }
        }
    }
}