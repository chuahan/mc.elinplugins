using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardHeavensFallFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleHeavensFallName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override string? AffiliatedReligion => "strife";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConHeavensFallSong bardBuff = ConBardSong.Create(nameof(ConHeavensFallSong), power, rhythmStacks, godBlessed, bard) as ConHeavensFallSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("holyveil");
    }
}