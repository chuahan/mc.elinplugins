using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardEndlessBlossomsFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleEndlessBlossomsName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    // Do I want this to be Kumiromi affiliated? It's more Yevan.
    
    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        // TODO: Do I want to play Blossom Weather?
        ConEndlessBlossomsSong bardBuff = ConBardSong.Create(nameof(ConEndlessBlossomsSong), power, rhythmStacks, godBlessed, bard) as ConEndlessBlossomsSong;
        target.AddCondition(bardBuff);
    }
}