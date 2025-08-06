using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * All allies gain massive elemental resistances across the board.
 * Grants up to 5 charges of Charged Strikes which double instances of outgoing damage.
 *
 * Ehekatl Blessed - 1/6 chance to not consume a charged strike. 1/36 Chance to instead add 5 charged strikes.
 */
public class BardPrismaticBridgeFinale : BardSongData
{
    public override string SongName => Constants.BardFinalePrismaticBridgeName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Friendly;

    public override string? AffiliatedReligion => "luck";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConPrismaticBridgeSong bardBuff = ConBardSong.Create(nameof(ConPrismaticBridgeSong), power, rhythmStacks, godBlessed, bard) as ConPrismaticBridgeSong;
        if (bardBuff != null)
        {
            switch (rhythmStacks / 10)
            {
                case 1:
                    bardBuff.Stacks = 1;
                    break;
                case 2:
                    bardBuff.Stacks = 3;
                    break;
                case 3:
                    bardBuff.Stacks = 5;
                    break;
            }
        }

        target.AddCondition(bardBuff);
        target.PlayEffect("boost");
    }
}