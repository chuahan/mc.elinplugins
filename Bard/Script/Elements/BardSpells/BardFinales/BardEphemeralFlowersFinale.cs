using System;
using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells.BardFinales;

/*
 * Ephemeral Flowers like Flames
 * Your notes bloom transiently, lost like ephemeral flowers at daybreak.
 * Every missing 10-5% hp is worth 1 stack. (max 19 stacks)
 * Rhythm / 5 is added as stacks. (max 6 stacks)
 * Stacks is multiplied against power for damage.
 * Total stacks is done as Ice damage to all enemies nearby.
 * Enemies are afflicted with brittle which increases damage taken from physical and impact and are slowed.
 * If they take lethal damage, they explode when they die, first re-inflicting brittle, then dealing % of max hp as death as ice damage.
 */
public class BardEphemeralFlowersFinale: BardSongData
{
    public override string SongName => Constants.BardFinaleEphemeralFlowersName;
    public override int SongId => Constants.BardFinaleSongId;
    public override float SongRadius => 4f;
    public override int SongLength => Constants.VerseSongDuration;
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

    public override string? AffiliatedReligion => "harvest";

    public override void PlayEffects(Chara bard)
    {
        // TODO: Pick better SFX. target.PlaySound("ab_magicsword");
        // TODO: Play FX.
        Effect spellEffect = Effect.Get("Element/ball_Fire");
        spellEffect.Play(bard.pos);
        bard.PlaySound("spell_ball");
    }
    public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
    {
        // Calculate Damage.
        int damageBase = HelperFunctions.SafeDice(Constants.BardFinaleEphemeralFlowersName, power);
        int damageChunk = rhythmStacks switch
        {
            30 => 5,
            >= 20 => 10,
            _ => 20
        };

        int missingChunks = (bard.MaxHP - bard.hp) / damageChunk;
        int multiplier = missingChunks + rhythmStacks / 5;
        int damage = HelperFunctions.SafeMultiplier(damageBase, multiplier);
        
        List<Chara> potentialTargets = HelperFunctions.GetCharasWithinRadius(bard.pos, SongRadius, bard,false, true);
        if (potentialTargets.Count != 0)
        {
            foreach (Chara enemy in potentialTargets)
            {
                ConEphemeralFlowersSong bardDebuff = ConBardSong.Create(nameof(ConEphemeralFlowersSong), power, rhythmStacks, godBlessed, bard) as ConEphemeralFlowersSong;
                enemy.AddCondition(bardDebuff);
                enemy.DamageHP(damage, Constants.EleFire, 100, AttackSource.Shockwave, bard);
                enemy.AddCondition<ConFreeze>(rhythmStacks, true);
            }
        }
    }
}