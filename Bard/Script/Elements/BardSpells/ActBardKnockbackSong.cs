using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using BardMod.Source;
namespace BardMod.Elements.BardSpells;

public class ActBardKnockbackSong : ActBardSong
{

    protected override BardSongData SongData => new BardKnockbackSong();

    public class BardKnockbackSong : BardSongData
    {
        public override string SongName => Constants.BardKnockbackSongName;
        public override int SongId => Constants.BardKnockbackSongId;
        public override float SongRadius => 4f;
        public override int SongLength => Constants.ChorusSongDuration;
        public override Constants.BardSongType SongType => Constants.BardSongType.Chorus;
        public override Constants.BardSongTarget SongTarget => Constants.BardSongTarget.Enemy;

        public override void PlayEffects(Chara bard)
        {
            Effect spellEffect = Effect.Get("Element/ball_Impact");
            spellEffect.Play(bard.pos);
            bard.PlaySound("spell_ball");
        }

        public override void ApplyEnemyEffect(Chara bard, Chara target, int power, int rhythmStacks, bool godBlessed)
        {
            int damage = HelperFunctions.SafeDice(Constants.BardKnockbackSongName, power);
            if (target.isChara && !target.HasCondition<ConGravity>() && target.ExistsOnMap && !target.isRestrained)
            {
                Card.MoveResult num6 = target.Chara.TryMoveFrom(bard.pos);
                if (num6 == Card.MoveResult.Success)
                {
                    target.renderer.SetFirst(first: true);
                    target.PlaySound("wave_hit_small");
                    target.AddCondition<ConParalyze>(20, true);
                }
            }
            BardCardPatches.CachedInvoker.Invoke(
                target,
                new object[] { damage, Constants.EleSound, 100, AttackSource.Shockwave, bard }
            );
        }
    }
}