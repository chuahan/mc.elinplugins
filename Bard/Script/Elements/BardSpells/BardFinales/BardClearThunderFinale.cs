using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardClearThunderFinale: BardSongData
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
        /*
         * - Offensive thunderstrike song.
         * - Adds buffs to you that picks random enemies every tick and drops a lightning bolt on them.
         * - Every bolt inflicts lightning res down.
         * - If worshipping Itzpalt, can target up to 3 enemies at per tick.
         * - Drops 3 bolts, doing damage twice, then the last hit also does % hp damage and inflicts paralysis.
         */
        ConClearThunderSong bardBuff = ConBardSong.Create(nameof(ConClearThunderSong), power, rhythmStacks, godBlessed, bard) as ConClearThunderSong;
        target.AddCondition(bardBuff);
    }
}