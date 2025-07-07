using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
using Cwl.Helper.Extensions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardAbyssReflectionFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleAbyssReflectionName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

    public override string? AffiliatedReligion => "eyth";

    // Creates a malevolent reflection of an enemy that is operating at 70% strength, but takes 200% damage.
    // If God blessed, creates two reflections instead.
    public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        int phantomDuration = rhythmStacks;
        int reflectionCount = godBlessed ? 3 : 1;
        for (int i = 0; i <= reflectionCount; i++)
        {
            Point summonPoint = target.pos.GetNearestPoint(allowBlock: false, allowChara: false);
            Chara reflection = CharaGen.Create(Constants.MalevolentReflectionCharaId);
            reflection.c_summonDuration = phantomDuration;
            reflection.isSummon = true;
            reflection.SetLv(target.LV);
            reflection.interest = 0;
            reflection.isBerserk = true;
            bard.currentZone.AddCard(reflection, summonPoint);
            reflection.Chara.PlayEffect("teleport");
            reflection.Chara.MakeMinion(bard);
            reflection.Chara.AddCondition<ConAbyssalReflection>(power, force: true);
        }

        // Add Fear to the Enemy.
        target.AddCondition<ConFear>(30 + phantomDuration, force: true);
    }
}