using System;
using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Source;
using BardMod.Stats.BardSongConditions;

namespace BardMod.Elements.BardSpells;

public class ActBardElementalSong : ActBardSong
{
    public class BardElementalSong : BardSongData
    {
        public override string SongName => Constants.BardElementalSongName;
        public override int SongId => Constants.BardElementalSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Self;

        public override void PlayEffects(Chara bard)
        {
            Effect spellEffect = Effect.Get("Element/ball_Void");
            spellEffect.Play(bard.pos);
            bard.PlaySound("spell_ball");
        }
        public override void ApplyFriendlyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            // Strikes enemies around you with a random element.
            int damage = HelperFunctions.SafeDice(Constants.BardElementalSongName, power);
            int[] elements =
            {
                Constants.EleFire,
                Constants.EleCold,
                Constants.EleLightning,
                Constants.EleDarkness,
                Constants.EleHoly,
                Constants.EleSound,
                Constants.EleMagic,
            };
            int randElement = elements[EClass.rnd(5)];
            
            List<Chara> potentialTargets = HelperFunctions.GetCharasWithinRadius(bard.pos, SongRadius, bard,false, true);
            if (potentialTargets.Count != 0)
            {
                foreach (Chara enemy in potentialTargets)
                {
                    enemy.PlaySound("wave_hit");
                    enemy.DamageHP(damage, ele: randElement, eleP: 100, AttackSource.MagicSword, bard);
                }
            }
        }
    }

    protected override BardSongData SongData => new BardElementalSong();
}