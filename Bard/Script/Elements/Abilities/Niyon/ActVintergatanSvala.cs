using System.Collections.Generic;
using System.Linq;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats.BardSongConditions;
namespace BardMod.Elements.Abilities.Niyon;

public class ActVintergatanSvala : BardAbility
{
    public override bool Perform()
    {
        // Play Effect
        // TODO: CUSTOM EFFECT?
        //Effect spellEffect = Effect.Get("Element/ball_Cold");
        //spellEffect.Play(CC.pos);
        //CC.PlaySound("spell_ball");
        int damage = HelperFunctions.SafeDice(Constants.VintergatanSvalaName, this.GetPower(CC));
        TC.DamageHP(damage, Constants.EleCold, eleP: 100, AttackSource.MagicSword, CC);
        TC.PlaySound("ab_magicsword");
        TC.PlayEffect("hit_slash").SetScale(1f);
        
        // All allies nearby refresh bard songs.
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 4f, CC, true, false);
        foreach (Chara target in targets)
        {
            List<ConBardSong> bardConditions = target.conditions.OfType<ConBardSong>().Where(c=> c.SongType != Constants.BardSongType.Finale).ToList();
            foreach (ConBardSong condition in bardConditions)
            {
                condition.RefreshBardSong();
            }
            target.PlayEffect("buff");
        }
        
        // Summon a Star Hand ally if there already does not exist one on the battlefield.
        if (CC.currentZone.FindChara(Constants.StarHandCharaId) == null && (CC.currentZone.CountMinions(CC) <= CC.MaxSummon))
        {
            Point summonPoint = CC.pos.GetNearestPoint(allowBlock: false, allowChara: false);
            Chara summon = CharaGen.Create(Constants.StarHandCharaId);
            summon.c_summonDuration = 10;
            summon.isSummon = true;
            summon.SetLv(this.GetPower(CC));
            summon.interest = 0;
            summon.isBerserk = true;
            summon.Chara.PlayEffect("teleport");
            summon.Chara.MakeMinion(CC);
            CC.currentZone.AddCard(summon, summonPoint);
        }
        
        return true;
    }
}