using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardMoonlitFlightFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleMoonlitFlightName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

    public override string? AffiliatedReligion => "moonshadow";

    public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConMoonlitFlightSong bardBuff = ConBardSong.Create(nameof(ConMoonlitFlightSong), power, rhythmStacks, godBlessed, bard) as ConMoonlitFlightSong;
        bardBuff.Stacks = rhythmStacks / 3;
        target.AddCondition(bardBuff);
        target.PlayEffect("curse");
    }
}