using System.Collections.Generic;
using BardMod.Common;
using BardMod.Source;
using BardMod.Stats;
namespace BardMod.Elements.BardSpells.BardFinales;

public class ActBardFinale : ActBardSong
{

    protected override BardSongData SongData => GetFinale();
    public BardSongData GetFinale()
    {
        ConRhythm rhythm = owner != null ? owner.GetCondition<ConRhythm>() : null;
        if (rhythm != null)
        {
            Constants.BardMotif motif = rhythm.Motif;
            if (motif == Constants.BardMotif.None) return new BardHollowSymphonyFinale();

            Dictionary<Constants.BardMotif, BardSongData> finales = new Dictionary<Constants.BardMotif, BardSongData>
            {
                {
                    Constants.BardMotif.Wind, new BardLulwyStepFinale()
                },
                {
                    Constants.BardMotif.Water, new BardAlluringRaindropsFinale()
                },
                {
                    Constants.BardMotif.Lightning, new BardClearThunderFinale()
                },
                {
                    Constants.BardMotif.Flame, new BardFarewellFlamesFinale()
                },
                {
                    Constants.BardMotif.Flower, new BardEndlessBlossomsFinale()
                },
                {
                    Constants.BardMotif.Vibration, new BardUnshakingEarthFinale()
                },
                {
                    Constants.BardMotif.Light, new BardLonelyTearsFinale()
                },
                {
                    Constants.BardMotif.Darkness, new BardAbyssReflectionFinale()
                },
                {
                    Constants.BardMotif.Eternalism, new BardPrismaticBridgeFinale()
                },
                {
                    Constants.BardMotif.Ethereal, new BardShimmeringDewFinale()
                },
                {
                    Constants.BardMotif.Revenant, new BardHeavensFallFinale()
                },
                {
                    Constants.BardMotif.Tempest, new BardAfterTempestFinale()
                },
                {
                    Constants.BardMotif.Starry, new BardShootingStarFinale()
                },
                {
                    Constants.BardMotif.Ephemeral, new BardEphemeralFlowersFinale()
                },
                {
                    Constants.BardMotif.Moonchill, new BardMoonlitFlightFinale()
                }
            };

            return finales[motif];
        }

        return new BardHollowSymphonyFinale();
    }

    public override bool CanPerform()
    {
        if (CC != null)
        {
            ConRhythm rhythm = CC.GetCondition<ConRhythm>();
            if (rhythm != null)
            {
                return rhythm.GetStacks() >= 10;
            }
        }

        return false;
    }
}