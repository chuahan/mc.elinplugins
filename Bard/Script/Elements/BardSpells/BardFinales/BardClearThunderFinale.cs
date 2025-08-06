using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Offensive thunderstrike song.
 * Adds buffs to you that picks up to 3 random enemies around every tick and drops a lightning bolt on them.
 * Every bolt inflicts lightning res down.
 * The last lightning bolt will also do % hp damage and inflicts paralysis.
 *
 * Itzpalt Blessing: Can target the same enemy with each bolt if there are fewer than 3 enemies nearby.
 */
public class BardClearThunderFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleClearThunderName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override string? AffiliatedReligion => "element";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConClearThunderSong bardBuff = ConBardSong.Create(nameof(ConClearThunderSong), power, rhythmStacks, godBlessed, bard) as ConClearThunderSong;
        target.AddCondition(bardBuff);
    }
}