using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
namespace BardMod.Stats;

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
            HelperFunctions.DamageHpWrapper(owner, damage, Constants.EleNether, 100, AttackSource.MagicSword, Caster);
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
            HelperFunctions.DamageHpWrapper(owner, damage, Constants.EleNether, 100, AttackSource.MagicSword, Caster);
        }

        base.OnRemoved();
    }
}