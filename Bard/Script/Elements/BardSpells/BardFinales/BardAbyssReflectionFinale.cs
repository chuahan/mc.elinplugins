using System.Collections.Generic;
using BardMod.Common;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Creates a malevolent reflection of an enemy that is operating at 70% strength, but takes 200% damage.
 * Eyth Blessing: creates two reflections instead.
 */
public class BardAbyssReflectionFinale : BardSongData
{
    public override string SongName => Constants.BardFinaleAbyssReflectionName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

    public override string? AffiliatedReligion => "eyth";

    public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        int phantomDuration = rhythmStacks;
        int reflectionCount = godBlessed ? 3 : 1;
        for (int i = 0; i <= reflectionCount; i++)
        {
            Point summonPoint = target.pos.GetNearestPoint(false, false);
            Chara reflection = CharaGen.Create(Constants.MalevolentReflectionCharaId);
            reflection.c_summonDuration = phantomDuration;
            reflection.isSummon = true;
            reflection.SetLv(target.LV);
            reflection.interest = 0;

            // Copy target elements.
            //reflection.elements.dict.Clear();
            foreach (KeyValuePair<int, Element> element in target.elements.dict)
            {
                Element copiedElement = reflection.elements.GetOrCreateElement(element.Key);
                copiedElement.vBase = element.Value.vBase;
                copiedElement.vLink = element.Value.vLink;
                copiedElement.vSource = element.Value.vSource;
            }

            // Sync Metadata
            reflection.bio = target.bio;
            reflection._hobbies = target._hobbies;
            reflection._ability = target._ability;
            reflection._tactics = target._tactics;
            reflection._job = target._job;
            reflection._works = target._works;
            reflection.isBerserk = true;
            reflection.isCopy = true;
            reflection.c_altName = target.NameSimple;
            reflection._alias = "reflectionName".lang();

            // Make sure Reflection starts with full HP/MP
            reflection.hp = reflection.MaxHP;
            reflection.mana.Set(reflection.mana.max);

            bard.currentZone.AddCard(reflection, summonPoint);
            reflection.Chara.PlayEffect("teleport");
            reflection.Chara.MakeMinion(bard);
            reflection.Chara.AddCondition<ConAbyssalReflection>(power, true);
        }

        // Add Fear to the Enemy.
        target.AddCondition<ConFear>(30 + phantomDuration, true);
    }
}