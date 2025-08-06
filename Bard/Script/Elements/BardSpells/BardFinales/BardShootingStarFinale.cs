using System;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Creates up to 5 Phantom Flutists to assist you in battle.
 */
public class BardShootingStarFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleShootingStarsName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        int phantomCount = (int)Math.Max(3, Math.Sqrt(rhythmStacks));
        int phantomPower = (int)HelperFunctions.SigmoidScaling(power, Constants.MaxBardPowerBuff, 1, 10, Constants.BardPowerSlope);
        int phantomLevelCalculated = HelperFunctions.SafeMultiplier(target.LV, phantomPower + phantomCount);
        for (int i = 0; i < phantomCount; i++)
        {
            Point summonPoint = target.pos.GetNearestPoint(false, false);
            Chara phantom = CharaGen.Create(Constants.PhantomFlutistCharaId);
            phantom.c_summonDuration = rhythmStacks / 3;
            phantom.isSummon = true;
            phantom.SetLv(Math.Max(power, phantomLevelCalculated));
            phantom.interest = 0;
            target.currentZone.AddCard(phantom, summonPoint);
            phantom.PlayEffect("teleport");
            phantom.MakeMinion(target);

            ConInvulnerable invulnerable = new ConInvulnerable
            {
                value = 300
            };
            phantom.AddCondition(invulnerable);
        }
    }
}