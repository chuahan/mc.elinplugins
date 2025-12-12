using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Stats.Justicar;

public class StanceFlamesOfJudgement : BaseStance
{
    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            // Not allowed in regions.
            Kill();
        }
        
        if (owner.hp <= (owner.MaxHP * 0.3F))
        {
            // TODO Text: Stance off.
            this.Kill();
        }

        // Get Karma Scores for the Player.
        // NPCs will be considered 0 Karma.
        bool positiveKarma = true, negativeKarma = true;
        if (CC.IsPCFactionOrMinion || CC.IsPC)
        {
            // For PC Faction, use PC's Karma.
            positiveKarma = player.karma >= 0;
            negativeKarma = player.karma <= 0;
        }

        int firePower = (int)(owner.MaxHP * 0.3F);
        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(owner.pos, 2F, owner, true);

        // Damage enemies. Negative Karma will inflict Fire Break.
        foreach (Chara target in enemies)
        {
            HelperFunctions.ProcSpellDamage(power, firePower, owner, target, AttackSource.None, Constants.EleFire, 50);
            if (negativeKarma) target.AddCondition(SubPoweredCondition.Create(nameof(ConFireBreak), power, 10));
        }

        // Do self-damage.
        HelperFunctions.ProcSpellDamage(power, firePower, owner, owner, AttackSource.None, Constants.EleFire, 50);

        // If Positive Karma, heal allies.
        if (positiveKarma)
        {
            foreach (Chara target in friendlies.Where(target => target != owner))
            {
                target.HealHP(firePower, HealSource.Magic);
            }
        }
    }
}