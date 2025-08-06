using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
namespace BardMod.Stats.BardSongConditions;

public class ConMoonlitFlightSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Debuff;

    public override void OnRemoved()
    {
        // TODO: Add SFX
        // TODO: Add FX
        int damage = HelperFunctions.SafeDice(Constants.BardFinaleMoonlitFlightName, power);
        damage = HelperFunctions.SafeMultiplier(damage, Stacks);
        if (GodBlessed)
        {
            // owner.DamageHP(dmg: damage, ele: Constants.EleNether, eleP: 100, attackSource: AttackSource.MagicSword, origin: Caster);
            BardCardPatches.CachedInvoker.Invoke(
                owner,
                new object[] { damage, Constants.EleNether, 100, AttackSource.MagicSword, Caster }
            );
            // Check nearest adjacent character
            Chara jumpTarget = owner.pos.GetRandomPoint(1).Charas.FirstOrDefault();
            if (jumpTarget != null)
            {
                // If the jump target is also a hostile to the caster.
                if (jumpTarget.IsHostile(Caster))
                {
                    ConMoonlitFlightSong bardBuff = ConBardSong.Create(nameof(ConMoonlitFlightSong), power, RhythmStacks / 2, GodBlessed, Caster) as ConMoonlitFlightSong;
                    bardBuff.Stacks = Stacks;
                    jumpTarget.AddCondition(bardBuff);
                    jumpTarget.PlayEffect("curse");
                }
                // If the jump target is a friendly to the caster.
                else
                {
                    ConMoonlitFlightEncore bardBuff = ConBardSong.Create(nameof(ConMoonlitFlightEncore), Stacks, RhythmStacks, GodBlessed, Caster) as ConMoonlitFlightEncore;
                    jumpTarget.AddCondition(bardBuff);
                }
            }
        }
        else
        {
            // owner.DamageHP(dmg: damage, ele: Constants.EleSound, eleP: 100, attackSource: AttackSource.MagicSword);
            BardCardPatches.CachedInvoker.Invoke(
                owner,
                new object[] { damage, Constants.EleSound, 100, AttackSource.MagicSword, null }
            );
        }

        base.OnRemoved();
    }
}