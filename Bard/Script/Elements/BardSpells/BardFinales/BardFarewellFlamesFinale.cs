using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardFarewellFlamesFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleFarewellFlamesName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConFarewellFlamesSong bardBuff = ConBardSong.Create(nameof(ConFarewellFlamesSong), power, rhythmStacks, godBlessed, bard) as ConFarewellFlamesSong;
        target.AddCondition(bardBuff);
        target.PlayEffect("fire_step");
    }
}