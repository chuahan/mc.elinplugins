using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Phantom;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// A graceful symphony of sword, gun, and spell. The Phantom is an unrelenting tide of sword and spell.
/// Phantoms focus on combinations of melee and magical attacks.
/// They specialize in high mobility and repositioning, with extravagant finisher attacks.
/// Skill - Wolkenkratzer - AOE Single Slam Attack. 25 Stam
/// Skill - Verbrechen - Rush at a point, deploying Phantom Bits as you move. Reaching the destination, fire a railgun shot at every nearby target doing magic damage. 25 Stam.
/// Skill - Schwarze Katze - Strike an enemy twice, then retreat away from the enemy. 30 Stam
/// Passive - Phantom Mark and Finisher - When you or your Phantom Bits hit enemies, they gain stacks of phantom mark, up to 10.
///     When you hit an enemy with one of your attack skills with Maxed Phantom Marks, the phantom marks are consumed, and you trigger a Phantom Finisher.
///     You can trigger multiple Phantom Finishers in a single skill proc.
///     Phantom Finishers will add additional effects to the Skill. 
///     Phantom Finishers will restore stamina and MP.
/// </summary>
public class FeatPhantom : PromotionFeat
{
    public override string PromotionClassId => Constants.PhantomId;
    public override int PromotionClassFeatId => Constants.FeatPhantom;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActVerbrechenId,
        Constants.ActWolkenkratzerId,
        Constants.ActSchwarzeKatze
    };

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "swordsage";
    }

    protected override void ApplyInternal()
    {
        // weaponScythe - 110
        // weaponSword
        // weaponGun - 105
        // casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }

    public static void AddPhantomMarks(Chara target, int count)
    {
        ConPhantomMark? mark = target.GetCondition<ConPhantomMark>() ?? target.AddCondition<ConPhantomMark>(force:true) as ConPhantomMark;
        mark?.AddStacks(count);
    }
    
    public static void SpawnPhantomBit(int power, Chara caster, Point pos)
    {
        int levelOverride = power / 15;
        if (caster.IsPC) levelOverride = Math.Max(EClass.player.stats.deepest, levelOverride);
        Chara summonedBit = CharaGen.Create("swordbit");
        summonedBit.SetMainElement("eleMagic", elemental: true);
        summonedBit.SetSummon(20 + power / 20 + EClass.rnd(10));
        summonedBit.SetLv(levelOverride);
        summonedBit.interest = 0;
        EClass._zone.AddCard(summonedBit, pos.GetNearestPoint(allowBlock: false, allowChara: false));
        summonedBit.PlayEffect("teleport");
        summonedBit.MakeMinion(caster);
    }
}