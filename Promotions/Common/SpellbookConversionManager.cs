using System.Collections.Generic;
using Cwl.Helper.Extensions;
namespace PromotionMod.Common;

/// <summary>
///     Some Promotion classes can convert spellbooks into spells specific for their class.
///     Druids can convert any summoning book into Summon Tree Ent.
///     Luminaries can convert any intonation spell into Holy Intonation.
///     Battlemage can convert any elemental books into Flare spells of the same element
///     Necromancers can convert any summoning books into Summon Skeleton.
///     Jenei can convert summon any basic elemental books into Fire/Cold/Lightning/Impact.
///     Saints can convert any basic elemental books into Holy Element.
///     Machinists can convert any summoning book into Summon Turret.
///     Spellblades can convert any elemental spellbooks into Sword or Intonation spells of the same element.
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
            Constants.FeatSaint, new SaintSpellbookConversion()
        },
        {
            Constants.FeatSpellblade, new SpellbladeSpellbookConversion()
        },
        {
            Constants.FeatMachinist, new MachinistSpellbookConversion()
        }
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

    public virtual bool CanConvert(int ele)
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

    public override bool CanConvert(int ele)
    {
        // 508XX is intonation
        return ele / 100 == 508;
    }

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        spellbook.owner.refVal = 50809; // weapon_Holy
        spellbook.owner.elements.SetBase(759, 10);
    }
}

public class BattlemageSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatBattlemage;

    public override bool CanConvert(int ele)
    {
        // Can convert any non-void elemental attack except flare spells since we are converting into flares. 
        return ele > 50100 && ele < 51215 && ele / 100 != 512;
    }
    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int elementId = spellbook.owner.refVal % 100; // Extracts Element.
        spellbook.owner.refVal = 51200 + elementId; // Construct Flare element
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

    public override bool CanConvert(int ele)
    {
        // Has to be within the elemental attack spells and can't be one of the four natural elements already.
        return ele > 50100 && ele < 51215 && !JeneiElements.Contains(ele % 100);
    }

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int prefix = spellbook.owner.refVal / 100;
        spellbook.owner.refVal = prefix * 100 + JeneiElements.RandomItem();
    }
}

public class SaintSpellbookConversion : SpellbookConversion
{
    public override int PromotionFeatId => Constants.FeatSaint;

    public override bool CanConvert(int ele)
    {
        // Has to be within the elemental attack spells and can't be holy.
        return ele > 50100 && ele < 51215 && ele % 100 != 9;
    }

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

    public override bool CanConvert(int ele)
    {
        // Has to be within the elemental attack spells and can't be sword or intonation
        int spellType = ele / 100;
        return ele > 50100 && ele < 51215 && spellType != 510 && spellType != 508;
    }

    public override void ConvertSpellbook(ref TraitSpellbook spellbook)
    {
        int elementId = spellbook.owner.refVal % 100; // Extracts Element.
        if (spellbook.owner.refVal is < 50100 or > 50999) return; // Sanity Check
        // 50:50 for Intonation or Swords
        int newSpell = 50800 + elementId; // Construct Intonation Element.
        if (EClass.rnd(2) == 0)
        {
            newSpell = 51000 + elementId; // Construct Sword Element.
        }
        spellbook.owner.refVal = newSpell;
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