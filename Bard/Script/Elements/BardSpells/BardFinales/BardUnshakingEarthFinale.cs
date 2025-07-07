using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;

namespace BardMod.Elements.BardSpells.BardFinales;

public class BardUnshakingEarthFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleUnshakingEarthName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 10f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override string? AffiliatedReligion => "earth";

    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        int damage =  HelperFunctions.SafeDice(Constants.BardFinaleUnshakingEarthName, power);
        if (godBlessed) HelperFunctions.SafeMultiplier(damage, 1.5f);
        this.EchoSlam(bard, bard, bard.pos, this.SongRadius, damage, new HashSet<Chara>());
    }

    private void EchoSlam(Chara caster, Chara origin, Point originPosition, float radius, int damage, HashSet<Chara> originalTargets)
    {
        // Play Earthquake effect:
        Effect effect = null;
        Point from = originPosition;
        
        if (EClass.rnd(4) == 0 && from.IsSync)
        {
            effect = Effect.Get("smoke_earthquake");
        }
        float num3 = 0.06f * (float)originPosition.Distance(from);
        Point pos = from.Copy();
        
        TweenUtil.Tween(num3, null, delegate
        {
            pos.Animate(AnimeID.Quake, animeBlock: true);
        });
        if (effect != null)
        {
            effect.SetStartDelay(num3);
        }
        caster.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");

        if (origin == null)
        {
            EClass.Wait(1f, origin);
        }
        
        if (radius < 1.0f) return; // Stop recursing when radius hits 1.
        
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(originPosition, radius, caster,false, false);

        foreach (Chara target in targets)
        {
            int damageWithModifier = damage;
            if (target.HasCondition<ConGravity>()) damage *= 2;
            if (target.IsLevitating) damage /= 2;
            
            target.DamageHP(
                damageWithModifier,
                Constants.EleImpact,
                100,
                AttackSource.None,
                caster);

            float newRadius = radius / 2.0f;
            int newDamage = damage / 2;

            // Recurse from each target
            EchoSlam(caster, target, target.pos, newRadius, newDamage, originalTargets);

            // If the target was *not* in the original list, recurse again from their location with half the new radius
            if (!originalTargets.Contains(target))
            {
                originalTargets.Add(target); // Prevent redundant processing
                EchoSlam(caster, target, target.pos, newRadius / 2.0f, newDamage, originalTargets);
            }
        }
    }
}