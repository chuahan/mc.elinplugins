using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Every turn you will create Water Clones to attack your enemies.
 * You will generate one clone every tick.
 *
 * Kizuami Blessing: Generate three clones instead of one every turn.
 */
public class BardAlluringRaindropsFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleAlluringDanceName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override string? AffiliatedReligion => "trickery";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        ConAlluringRaindropsSong bardBuff = ConBardSong.Create(nameof(ConAlluringRaindropsSong), power, rhythmStacks, godBlessed, bard) as ConAlluringRaindropsSong;
        target.AddCondition(bardBuff);

        // Summon Clones to start
        int rhythmPower = rhythmStacks / 10;
        Point summonPoint = bard.pos.GetNearestPoint(false, false);
        for (int i = 0; i < rhythmPower; i++)
        {
            Chara phantom = CharaGen.Create(Constants.WaterDancerCharaId);
            phantom.c_summonDuration = 10;
            phantom.isSummon = true;
            phantom.SetLv(bard.LV + power);
            phantom.interest = 0;
            bard.currentZone.AddCard(phantom, summonPoint);
            phantom.PlayEffect("teleport");
            phantom.MakeMinion(bard);
        }
    }
}