using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardLulwyStepFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleLulwyStepName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override string AffiliatedReligion => "wind";
    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConLulwyStepSong bardBuff = ConBardSong.Create(nameof(ConLulwyStepSong), power, rhythmStacks, godBlessed, bard) as ConLulwyStepSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("boost");
    }
}