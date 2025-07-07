using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardLonelyTearsFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleLonelyTearsName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 10f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Both;

    public override string? AffiliatedReligion => "healing";

    public override void PlayEffects(Chara bard)
    {
        Effect lightningBallEffect = Effect.Get("Element/ball_Holy");
        lightningBallEffect.Play(bard.pos);
        bard.PlaySound("heal");
    }

    /*
     * Massive heal on allies.
     * Add Overguard based on Healing Amount.
     * Purge debuffs.
     * Buffed Healing Song applied.
     * Allies gain second chance. If they have more than 33% hp take a grievous strike that is more than that, they will survive with 10% hp.
     */
    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        int healing = HelperFunctions.SafeDice(Constants.BardFinaleLonelyTearsName, power);
        target.HealHP(healing, HealSource.Magic);
        (target.AddCondition<ConOverguard>(1) as ConOverguard)?.AddOverGuard(healing);
        target.AddCondition<ConLonelyTearsSong>(power);
        
        // Purge all debuffs if max Rhythm.
        bool forcePurge = (rhythmStacks >= 30);
        foreach (Condition item5 in target.conditions.Copy())
        {
            if (item5.Type == ConditionType.Debuff &&
                !item5.IsKilled &&
                ((EClass.rnd(power * 2) > EClass.rnd(item5.power)) || forcePurge) &&
                item5 is not ConWrath && // Don't purge Wrath of God.
                item5 is not ConDeathSentense) // Don't purge Death Sentence.
            {
                bard.Say("removeHex", target, item5.Name.ToLower());
                item5.Kill();
            }
        }
        
        target.PlayEffect("heal_tick");
    }
    
    // If worshipping Jure, enemies nearby take heavy holy damage. undead enemies take double damage.
    public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        if (!godBlessed) return;
        int damage = HelperFunctions.SafeDice(Constants.BardFinaleLonelyTearsName, power);
        if (target.race.IsUndead) damage = HelperFunctions.SafeMultiplier(damage, 2);
        target.DamageHP(damage, Constants.EleHoly, 100, AttackSource.None, bard);
    }
}