using System.Collections.Generic;
using Cwl.Helper.Extensions;
namespace PromotionMod.Common;

/// <summary>
/// Some Promotion classes can convert spellbooks into spells specific for their class.
/// Druids can convert any summoning book into Summon Tree Ent.
/// Luminaries can convert any intonation spell into Holy Intonation.
/// Battlemage can convert any elemental books into Hammer or Cannon spells of the same element
/// Necromancers can convert any summoning books into Summon Skeleton.
/// Jenei can convert summon any basic elemental books into Fire/Cold/Lightning/Impact.
/// Saints can convert any basic elemental books into Holy Element.
/// Machinists can convert any summoning book into Summon Turret.
/// </summary>
public static class SpellbookConversionManager
{
    private static readonly Dictionary<int, SpellbookConversion> Conversions = new Dictionary<int, SpellbookConversion>
    {
        {
            Constants.FeatDruid, new DruidSpellbookConversion()
        },
        {
            Constants.FeatLuminary, new LuminarySpellbookConversion()
        },
        {
            Constants.FeatBattlemage, new BattlemageSpellbookConversion()
        },
        {
            Constants.FeatNecromancer, new NecromancerSpellbookConversion()
        },
        {
            Constants.FeatJenei, new JeneiSpellbookConversion()
        },
        {
            Constants.FeatSaint, new SeerSpellbookConversion()
        },
        {
            Constants.FeatSpellblade, new SpellbladeSpellbookConversion()
        },
        {
            Constants.FeatMachinist, new MachinistSpellbookConversion()
        },
    };

    public static bool CanConvertSpellbook(Chara chara, TraitSpellbook spellbook)
    {
        int promotionClassFeatId = chara.GetFlagValue(Constants.PromotionFeatFlag);
        if (promotionClassFeatId == 0) return false; // Unpromoted.
        if (chara.Evalue(promotionClassFeatId) == 0) return false; // Sanity Check
        return Conversions.TryGetValue(promotionClassFeatId, out SpellbookConversion? converter) && converter.CanConvert(spellbook.owner.refVal);
    }

    public static bool TryConvert(Chara chara, TraitSpellbook spellbook)
    {
        int promotionClassFeatId = chara.GetFlagValue(Constants.PromotionFeatFlag);
        if (promotionClassFeatId == 0) return false;
        if (Conversions.TryGetValue(promotionClassFeatId, out SpellbookConversion? converter) &&
            converter.CanConvert(spellbook.owner.refVal))
        {
            converter.ConvertSpellbook(ref spellbook);
            return true;
        }

        return false;
    }
}

/// <summary>
///     Class that manages Spellbook Conversion for a given Promotion Class.
/// </summary>
public abstract class SpellbookConversion
{
    public virtual int PromotionFeatId { get; }
    public virtual HashSet<int> ConvertableBooks { get; }
    public bool CanConvert(int ele)
    {
        return ConvertableBooks.Contains(ele);
    }

    public abstract void ConvertSpellbook(ref TraitSpellbook spellbook);
}

public class DruidSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatDruid;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        9000, // Summon Animal
        9001, // Summon UYS
        9004, // Summon Monster
        9005, // Summon Pawn 
        9050 // Summon Shadow
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        spellbook.owner.refVal = Constants.SpSummonTreeEntId;
        spellbook.owner.elements.SetBase(759, 10);
    }
}

public class LuminarySpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatLuminary;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        50800, // weapon_Fire
        50801, // weapon_Cold
        50802, // weapon_Lightning
        50803, // weapon_Darkness
        50804, // weapon_Mind
        50805, // weapon_Poison
        50806, // weapon_Nether
        50807, // weapon_Sound
        50808, // weapon_Nerve
        50810, // weapon_Chaos
        50811, // weapon_Magic
        50812, // weapon_Ether
        50813, // weapon_Acid
        50814, // weapon_Cut
        50815 // weapon_Impact
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        spellbook.owner.refVal = 50809; // weapon_Holy
        spellbook.owner.elements.SetBase(759, 10);
    }
}

public class BattlemageSpellbookConversion : SpellbookConversion
{

    private static readonly Dictionary<int, (int hammerId, int cannonId)> ElementToSpells = new Dictionary<int, (int hammerId, int cannonId)>
    {
        {
            0, (Constants.SpFireHammer, Constants.SpFireCannon)
        },
        {
            1, (Constants.SpColdHammer, Constants.SpColdCannon)
        },
        {
            2, (Constants.SpLightningHammer, Constants.SpLightningCannon)
        },
        {
            3, (Constants.SpDarknessHammer, Constants.SpDarknessCannon)
        },
        {
            4, (Constants.SpMindHammer, Constants.SpMindCannon)
        },
        {
            5, (Constants.SpPoisonHammer, Constants.SpPoisonCannon)
        },
        {
            6, (Constants.SpNetherHammer, Constants.SpNetherCannon)
        },
        {
            7, (Constants.SpSoundHammer, Constants.SpSoundCannon)
        },
        {
            8, (Constants.SpNerveHammer, Constants.SpNerveCannon)
        },
        {
            9, (Constants.SpHolyHammer, Constants.SpHolyCannon)
        },
        {
            10, (Constants.SpChaosHammer, Constants.SpChaosCannon)
        },
        {
            11, (Constants.SpMagicHammer, Constants.SpMagicCannon)
        },
        {
            12, (Constants.SpEtherHammer, Constants.SpEtherCannon)
        },
        {
            13, (Constants.SpAcidHammer, Constants.SpAcidCannon)
        },
        {
            14, (Constants.SpCutHammer, Constants.SpCutCannon)
        },
        {
            15, (Constants.SpImpactHammer, Constants.SpImpactCannon)
        }
    };

    // ball, breathe, hand, weapon, puddle, sword
    private static readonly HashSet<int> HammerPrefixes = new HashSet<int>
    {
        501,
        502,
        504,
        508,
        509,
        510
    };

    // bolt, arrow, funnel, miasma
    private static readonly HashSet<int> CannonPrefixes = new HashSet<int>
    {
        503,
        505,
        506,
        507
    };
    public override int PromotionFeatId => Constants.FeatBattlemage;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        50100, //ball_Fire
        50200, //breathe_Fire
        50300, //bolt_Fire
        50400, //hand_Fire
        50500, //arrow_Fire
        50600, //funnel_Fire
        50700, //miasma_Fire
        50800, //weapon_Fire
        50900, //puddle_Fire
        51000, //sword_Fire

        50101, //ball_Cold
        50201, //breathe_Cold
        50301, //bolt_Cold
        50401, //hand_Cold
        50501, //arrow_Cold
        50601, //funnel_Cold
        50701, //miasma_Cold
        50801, //weapon_Cold
        50901, //puddle_Cold
        51001, //sword_Cold

        50102, //ball_Lightning
        50202, //breathe_Lightning
        50302, //bolt_Lightning
        50402, //hand_Lightning
        50502, //arrow_Lightning
        50602, //funnel_Lightning
        50702, //miasma_Lightning
        50802, //weapon_Lightning
        50902, //puddle_Lightning
        51002, //sword_Lightning

        50103, //ball_Darkness
        50203, //breathe_Darkness
        50303, //bolt_Darkness
        50403, //hand_Darkness
        50503, //arrow_Darkness
        50603, //funnel_Darkness
        50703, //miasma_Darkness
        50803, //weapon_Darkness
        50903, //puddle_Darkness
        51003, //sword_Darkness

        50104, //ball_Mind
        50204, //breathe_Mind
        50304, //bolt_Mind
        50404, //hand_Mind
        50504, //arrow_Mind
        50604, //funnel_Mind
        50704, //miasma_Mind
        50804, //weapon_Mind
        50904, //puddle_Mind
        51004, //sword_Mind

        50105, //ball_Poison
        50205, //breathe_Poison
        50305, //bolt_Poison
        50405, //hand_Poison
        50505, //arrow_Poison
        50605, //funnel_Poison
        50705, //miasma_Poison
        50805, //weapon_Poison
        50905, //puddle_Poison
        51005, //sword_Poison

        50106, //ball_Nether
        50206, //breathe_Nether
        50306, //bolt_Nether
        50406, //hand_Nether
        50506, //arrow_Nether
        50606, //funnel_Nether
        50706, //miasma_Nether
        50806, //weapon_Nether
        50906, //puddle_Nether
        51006, //sword_Nether

        50107, //ball_Sound
        50207, //breathe_Sound
        50307, //bolt_Sound
        50407, //hand_Sound
        50507, //arrow_Sound
        50607, //funnel_Sound
        50707, //miasma_Sound
        50807, //weapon_Sound
        50907, //puddle_Sound
        51007, //sword_Sound

        50108, //ball_Nerve
        50208, //breathe_Nerve
        50308, //bolt_Nerve
        50408, //hand_Nerve
        50508, //arrow_Nerve
        50608, //funnel_Nerve
        50708, //miasma_Nerve
        50808, //weapon_Nerve
        50908, //puddle_Nerve
        51008, //sword_Nerve

        50109, //ball_Holy
        50209, //breathe_Holy
        50309, //bolt_Holy
        50409, //hand_Holy
        50509, //arrow_Holy
        50609, //funnel_Holy
        50709, //miasma_Holy
        50809, //weapon_Holy
        50909, //puddle_Holy
        51009, //sword_Holy

        50110, //ball_Chaos
        50210, //breathe_Chaos
        50310, //bolt_Chaos
        50410, //hand_Chaos
        50510, //arrow_Chaos
        50610, //funnel_Chaos
        50710, //miasma_Chaos
        50810, //weapon_Chaos
        50910, //puddle_Chaos
        51010, //sword_Chaos

        50111, //ball_Magic
        50211, //breathe_Magic
        50311, //bolt_Magic
        50411, //hand_Magic
        50511, //arrow_Magic
        50611, //funnel_Magic
        50711, //miasma_Magic
        50811, //weapon_Magic
        50911, //puddle_Magic
        51011, //sword_Magic

        50112, //ball_Ether
        50212, //breathe_Ether
        50312, //bolt_Ether
        50412, //hand_Ether
        50512, //arrow_Ether
        50612, //funnel_Ether
        50712, //miasma_Ether
        50812, //weapon_Ether
        50912, //puddle_Ether
        51012, //sword_Ether

        50113, //ball_Acid
        50213, //breathe_Acid
        50313, //bolt_Acid
        50413, //hand_Acid
        50513, //arrow_Acid
        50613, //funnel_Acid
        50713, //miasma_Acid
        50813, //weapon_Acid
        50913, //puddle_Acid
        51013, //sword_Acid

        50114, //ball_Cut
        50214, //breathe_Cut
        50314, //bolt_Cut
        50414, //hand_Cut
        50514, //arrow_Cut
        50614, //funnel_Cut
        50714, //miasma_Cut
        50814, //weapon_Cut
        50914, //puddle_Cut
        51014, //sword_Cut

        50115, //ball_Impact
        50215, //breathe_Impact
        50315, //bolt_Impact
        50415, //hand_Impact
        50515, //arrow_Impact
        50615, //funnel_Impact
        50715, //miasma_Impact
        50815, //weapon_Impact
        50915, //puddle_Impact
        51015 //sword_Impact
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int id = spellbook.owner.refVal;
        int prefix = id / 100;
        int element = id % 100;

        if (!ElementToSpells.TryGetValue(element, out (int hammerId, int cannonId) targets)) return;

        spellbook.owner.refVal = HammerPrefixes.Contains(prefix)
                ? targets.hammerId
                : targets.cannonId;

        spellbook.owner.elements.SetBase(759, 10);
    }
}

public class NecromancerSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatNecromancer;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        9000, // Summon Animal
        9001, // Summon UYS
        9004, // Summon Monster
        9005, // Summon Pawn 
        9050 // Summon Shadow
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        spellbook.owner.refVal = Constants.SpSummonSkeleton;
        spellbook.owner.elements.SetBase(759, 10);
    }
}

public class JeneiSpellbookConversion : SpellbookConversion
{

    private static readonly HashSet<int> JeneiElements = new HashSet<int>
    {
        0,
        1,
        2,
        15
    };
    public override int PromotionFeatId => Constants.FeatJenei;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        50103, //ball_Darkness
        50203, //breathe_Darkness
        50303, //bolt_Darkness
        50403, //hand_Darkness
        50503, //arrow_Darkness
        50603, //funnel_Darkness
        50703, //miasma_Darkness
        50803, //weapon_Darkness
        50903, //puddle_Darkness
        51003, //sword_Darkness

        50104, //ball_Mind
        50204, //breathe_Mind
        50304, //bolt_Mind
        50404, //hand_Mind
        50504, //arrow_Mind
        50604, //funnel_Mind
        50704, //miasma_Mind
        50804, //weapon_Mind
        50904, //puddle_Mind
        51004, //sword_Mind

        50105, //ball_Poison
        50205, //breathe_Poison
        50305, //bolt_Poison
        50405, //hand_Poison
        50505, //arrow_Poison
        50605, //funnel_Poison
        50705, //miasma_Poison
        50805, //weapon_Poison
        50905, //puddle_Poison
        51005, //sword_Poison

        50106, //ball_Nether
        50206, //breathe_Nether
        50306, //bolt_Nether
        50406, //hand_Nether
        50506, //arrow_Nether
        50606, //funnel_Nether
        50706, //miasma_Nether
        50806, //weapon_Nether
        50906, //puddle_Nether
        51006, //sword_Nether

        50107, //ball_Sound
        50207, //breathe_Sound
        50307, //bolt_Sound
        50407, //hand_Sound
        50507, //arrow_Sound
        50607, //funnel_Sound
        50707, //miasma_Sound
        50807, //weapon_Sound
        50907, //puddle_Sound
        51007, //sword_Sound

        50108, //ball_Nerve
        50208, //breathe_Nerve
        50308, //bolt_Nerve
        50408, //hand_Nerve
        50508, //arrow_Nerve
        50608, //funnel_Nerve
        50708, //miasma_Nerve
        50808, //weapon_Nerve
        50908, //puddle_Nerve
        51008, //sword_Nerve

        50109, //ball_Holy
        50209, //breathe_Holy
        50309, //bolt_Holy
        50409, //hand_Holy
        50509, //arrow_Holy
        50609, //funnel_Holy
        50709, //miasma_Holy
        50809, //weapon_Holy
        50909, //puddle_Holy
        51009, //sword_Holy

        50110, //ball_Chaos
        50210, //breathe_Chaos
        50310, //bolt_Chaos
        50410, //hand_Chaos
        50510, //arrow_Chaos
        50610, //funnel_Chaos
        50710, //miasma_Chaos
        50810, //weapon_Chaos
        50910, //puddle_Chaos
        51010, //sword_Chaos

        50111, //ball_Magic
        50211, //breathe_Magic
        50311, //bolt_Magic
        50411, //hand_Magic
        50511, //arrow_Magic
        50611, //funnel_Magic
        50711, //miasma_Magic
        50811, //weapon_Magic
        50911, //puddle_Magic
        51011, //sword_Magic

        50112, //ball_Ether
        50212, //breathe_Ether
        50312, //bolt_Ether
        50412, //hand_Ether
        50512, //arrow_Ether
        50612, //funnel_Ether
        50712, //miasma_Ether
        50812, //weapon_Ether
        50912, //puddle_Ether
        51012, //sword_Ether

        50113, //ball_Acid
        50213, //breathe_Acid
        50313, //bolt_Acid
        50413, //hand_Acid
        50513, //arrow_Acid
        50613, //funnel_Acid
        50713, //miasma_Acid
        50813, //weapon_Acid
        50913, //puddle_Acid
        51013, //sword_Acid

        50114, //ball_Cut
        50214, //breathe_Cut
        50314, //bolt_Cut
        50414, //hand_Cut
        50514, //arrow_Cut
        50614, //funnel_Cut
        50714, //miasma_Cut
        50814, //weapon_Cut
        50914, //puddle_Cut
        51014 //sword_Cut
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int prefix = spellbook.owner.refVal / 100;
        if (prefix < 501 || prefix > 510) return; // Sanity Check
        spellbook.owner.refVal = prefix * 100 + JeneiElements.RandomItem();
    }
}

public class SeerSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatSaint;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        50100, //ball_Fire
        50200, //breathe_Fire
        50300, //bolt_Fire
        50400, //hand_Fire
        50500, //arrow_Fire
        50600, //funnel_Fire
        50700, //miasma_Fire
        50800, //weapon_Fire
        50900, //puddle_Fire
        51000, //sword_Fire
        50101, //ball_Cold
        50201, //breathe_Cold
        50301, //bolt_Cold
        50401, //hand_Cold
        50501, //arrow_Cold
        50601, //funnel_Cold
        50701, //miasma_Cold
        50801, //weapon_Cold
        50901, //puddle_Cold
        51001, //sword_Cold
        50102, //ball_Lightning
        50202, //breathe_Lightning
        50302, //bolt_Lightning
        50402, //hand_Lightning
        50502, //arrow_Lightning
        50602, //funnel_Lightning
        50702, //miasma_Lightning
        50802, //weapon_Lightning
        50902, //puddle_Lightning
        51002, //sword_Lightning
        50103, //ball_Darkness
        50203, //breathe_Darkness
        50303, //bolt_Darkness
        50403, //hand_Darkness
        50503, //arrow_Darkness
        50603, //funnel_Darkness
        50703, //miasma_Darkness
        50803, //weapon_Darkness
        50903, //puddle_Darkness
        51003, //sword_Darkness
        50104, //ball_Mind
        50204, //breathe_Mind
        50304, //bolt_Mind
        50404, //hand_Mind
        50504, //arrow_Mind
        50604, //funnel_Mind
        50704, //miasma_Mind
        50804, //weapon_Mind
        50904, //puddle_Mind
        51004, //sword_Mind
        50105, //ball_Poison
        50205, //breathe_Poison
        50305, //bolt_Poison
        50405, //hand_Poison
        50505, //arrow_Poison
        50605, //funnel_Poison
        50705, //miasma_Poison
        50805, //weapon_Poison
        50905, //puddle_Poison
        51005, //sword_Poison
        50106, //ball_Nether
        50206, //breathe_Nether
        50306, //bolt_Nether
        50406, //hand_Nether
        50506, //arrow_Nether
        50606, //funnel_Nether
        50706, //miasma_Nether
        50806, //weapon_Nether
        50906, //puddle_Nether
        51006, //sword_Nether
        50107, //ball_Sound
        50207, //breathe_Sound
        50307, //bolt_Sound
        50407, //hand_Sound
        50507, //arrow_Sound
        50607, //funnel_Sound
        50707, //miasma_Sound
        50807, //weapon_Sound
        50907, //puddle_Sound
        51007, //sword_Sound
        50108, //ball_Nerve
        50208, //breathe_Nerve
        50308, //bolt_Nerve
        50408, //hand_Nerve
        50508, //arrow_Nerve
        50608, //funnel_Nerve
        50708, //miasma_Nerve
        50808, //weapon_Nerve
        50908, //puddle_Nerve
        51008, //sword_Nerve
        50110, //ball_Chaos
        50210, //breathe_Chaos
        50310, //bolt_Chaos
        50410, //hand_Chaos
        50510, //arrow_Chaos
        50610, //funnel_Chaos
        50710, //miasma_Chaos
        50810, //weapon_Chaos
        50910, //puddle_Chaos
        51010, //sword_Chaos
        50111, //ball_Magic
        50211, //breathe_Magic
        50311, //bolt_Magic
        50411, //hand_Magic
        50511, //arrow_Magic
        50611, //funnel_Magic
        50711, //miasma_Magic
        50811, //weapon_Magic
        50911, //puddle_Magic
        51011, //sword_Magic
        50112, //ball_Ether
        50212, //breathe_Ether
        50312, //bolt_Ether
        50412, //hand_Ether
        50512, //arrow_Ether
        50612, //funnel_Ether
        50712, //miasma_Ether
        50812, //weapon_Ether
        50912, //puddle_Ether
        51012, //sword_Ether
        50113, //ball_Acid
        50213, //breathe_Acid
        50313, //bolt_Acid
        50413, //hand_Acid
        50513, //arrow_Acid
        50613, //funnel_Acid
        50713, //miasma_Acid
        50813, //weapon_Acid
        50913, //puddle_Acid
        51013, //sword_Acid
        50114, //ball_Cut
        50214, //breathe_Cut
        50314, //bolt_Cut
        50414, //hand_Cut
        50514, //arrow_Cut
        50614, //funnel_Cut
        50714, //miasma_Cut
        50814, //weapon_Cut
        50914, //puddle_Cut
        51014, //sword_Cut
        50115, //ball_Impact
        50215, //breathe_Impact
        50315, //bolt_Impact
        50415, //hand_Impact
        50515, //arrow_Impact
        50615, //funnel_Impact
        50715, //miasma_Impact
        50815, //weapon_Impact
        50915, //puddle_Impact
        51015 //sword_Impact
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int prefix = spellbook.owner.refVal / 100;
        if (prefix < 501 || prefix > 510) return; // Sanity Check
        spellbook.owner.refVal = prefix * 100 + 9;
    }
}

public class SpellbladeSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatSpellblade;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        50100, //ball_Fire
        50200, //breathe_Fire
        50300, //bolt_Fire
        50400, //hand_Fire
        50500, //arrow_Fire
        50600, //funnel_Fire
        50700, //miasma_Fire
        50800, //weapon_Fire
        50900, //puddle_Fire
        50101, //ball_Cold
        50201, //breathe_Cold
        50301, //bolt_Cold
        50401, //hand_Cold
        50501, //arrow_Cold
        50601, //funnel_Cold
        50701, //miasma_Cold
        50801, //weapon_Cold
        50901, //puddle_Cold
        50102, //ball_Lightning
        50202, //breathe_Lightning
        50302, //bolt_Lightning
        50402, //hand_Lightning
        50502, //arrow_Lightning
        50602, //funnel_Lightning
        50702, //miasma_Lightning
        50802, //weapon_Lightning
        50902, //puddle_Lightning
        50103, //ball_Darkness
        50203, //breathe_Darkness
        50303, //bolt_Darkness
        50403, //hand_Darkness
        50503, //arrow_Darkness
        50603, //funnel_Darkness
        50703, //miasma_Darkness
        50803, //weapon_Darkness
        50903, //puddle_Darkness
        50104, //ball_Mind
        50204, //breathe_Mind
        50304, //bolt_Mind
        50404, //hand_Mind
        50504, //arrow_Mind
        50604, //funnel_Mind
        50704, //miasma_Mind
        50804, //weapon_Mind
        50904, //puddle_Mind
        50105, //ball_Poison
        50205, //breathe_Poison
        50305, //bolt_Poison
        50405, //hand_Poison
        50505, //arrow_Poison
        50605, //funnel_Poison
        50705, //miasma_Poison
        50805, //weapon_Poison
        50905, //puddle_Poison
        50106, //ball_Nether
        50206, //breathe_Nether
        50306, //bolt_Nether
        50406, //hand_Nether
        50506, //arrow_Nether
        50606, //funnel_Nether
        50706, //miasma_Nether
        50806, //weapon_Nether
        50906, //puddle_Nether
        50107, //ball_Sound
        50207, //breathe_Sound
        50307, //bolt_Sound
        50407, //hand_Sound
        50507, //arrow_Sound
        50607, //funnel_Sound
        50707, //miasma_Sound
        50807, //weapon_Sound
        50907, //puddle_Sound
        50108, //ball_Nerve
        50208, //breathe_Nerve
        50308, //bolt_Nerve
        50408, //hand_Nerve
        50508, //arrow_Nerve
        50608, //funnel_Nerve
        50708, //miasma_Nerve
        50808, //weapon_Nerve
        50908, //puddle_Nerve
        50110, //ball_Chaos
        50210, //breathe_Chaos
        50310, //bolt_Chaos
        50410, //hand_Chaos
        50510, //arrow_Chaos
        50610, //funnel_Chaos
        50710, //miasma_Chaos
        50810, //weapon_Chaos
        50910, //puddle_Chaos
        50111, //ball_Magic
        50211, //breathe_Magic
        50311, //bolt_Magic
        50411, //hand_Magic
        50511, //arrow_Magic
        50611, //funnel_Magic
        50711, //miasma_Magic
        50811, //weapon_Magic
        50911, //puddle_Magic
        50112, //ball_Ether
        50212, //breathe_Ether
        50312, //bolt_Ether
        50412, //hand_Ether
        50512, //arrow_Ether
        50612, //funnel_Ether
        50712, //miasma_Ether
        50812, //weapon_Ether
        50912, //puddle_Ether
        50113, //ball_Acid
        50213, //breathe_Acid
        50313, //bolt_Acid
        50413, //hand_Acid
        50513, //arrow_Acid
        50613, //funnel_Acid
        50713, //miasma_Acid
        50813, //weapon_Acid
        50913, //puddle_Acid
        50114, //ball_Cut
        50214, //breathe_Cut
        50314, //bolt_Cut
        50414, //hand_Cut
        50514, //arrow_Cut
        50614, //funnel_Cut
        50714, //miasma_Cut
        50814, //weapon_Cut
        50914, //puddle_Cut
        50115, //ball_Impact
        50215, //breathe_Impact
        50315, //bolt_Impact
        50415, //hand_Impact
        50515, //arrow_Impact
        50615, //funnel_Impact
        50715, //miasma_Impact
        50815, //weapon_Impact
        50915, //puddle_Impact
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int elementId = spellbook.owner.refVal % 100; // Extracts Element.
        if (spellbook.owner.refVal is < 50100 or > 50999) return; // Sanity Check
        int newSpell = 51000 + elementId; // Construct Sword Element.
        spellbook.owner.refVal = newSpell;
    }
}

public class MachinistSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatMachinist;

    public override HashSet<int> ConvertableBooks => new HashSet<int>
    {
        9000, // Summon Animal
        9001, // Summon UYS
        9004, // Summon Monster
        9005, // Summon Pawn 
        9050 // Summon Shadow
    };

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        spellbook.owner.refVal = Constants.SpSummonTurretId;
        spellbook.owner.elements.SetBase(759, 10);
    }
}