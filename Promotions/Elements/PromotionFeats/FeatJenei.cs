using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// Shapers of the natural energies. Jenei are those who call upon the natural forces of Earth, Fire, Air, and Water.
/// Jenei focus on combining the four natural elements to summon great representatives of the natural forces to aid them in combat.
/// They specialize in invoking mighty spells with a variety of effects through building up amounts of each of the elements.
///
/// TODO
/// Upon promotion, you must choose an element to align to, which will grant you a field skill and a combat skill. (Mercury instead adopts two combat skills)
/// Venus - Move, Gaia (Mother Gaia, Grand Gaia)
/// Mars - Blaze, Volcano (Eruption, Pyroclasm)
/// Jupiter - Reveal, Plasma (Shine Plasma, Spark Plasma)
/// Mercury - Douse (Drench, Deluge), Ply (Ply Well, Pure Ply)
/// Class Adoption - Skill - Checks your Jenei condition and uses your charges to transform your class, adjusting your stats.
///     Depending on how many charges are consumed, your class combat skills will be upgraded to T2 or T3.
///     When using Spirit Summon, you will lose your class adoption.
/// 
/// Spirit Summon - Skill - Checks your Jenei Condition and uses your charges to summon an incredibly powerful spirit to aid you in battle temporarily.
/// Passive - Natural Balances - Can convert spellbooks into Impact/Fire/Lightning/Ice.
/// Passive - 
/// </summary>
public class FeatJenei : PromotionFeat
{
    public override string PromotionClassId => Constants.JeneiId;
    public override int PromotionClassFeatId => Constants.FeatJenei;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSpiritSummonId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "farmer";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
    
    public class JeneiSummonCost
    {
        public JeneiSummonCost(Dictionary<int, int> cost, string summonId)
        {
            ElementalCost = cost;
            SummonId = summonId;
        }
        private Dictionary<int, int> ElementalCost { get; }
        public string SummonId { get; set; }

        public int Cost => ElementalCost.Values.Sum();
        public bool CanAfford(Dictionary<int, int> stockpile)
        {
            return ElementalCost.All(kvp => stockpile.TryGetValue(kvp.Key, out int count) && count >= kvp.Value);
        }
    }

    public static class JeneiSummons
    {
        public static readonly JeneiSummonCost Cybele = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleImpact, 3
            }
        }, Constants.JeneiCybeleCharaId);

        public static readonly JeneiSummonCost Tiamat = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleFire, 3
            }
        }, Constants.JeneiTiamatCharaId);

        public static readonly JeneiSummonCost Atlanta = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleLightning, 3
            }
        }, Constants.JeneiAtlantaCharaId);

        public static readonly JeneiSummonCost Boreas = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleCold, 3
            }
        }, Constants.JeneiBoreasCharaId);

        public static readonly JeneiSummonCost Zagan = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleFire, 1
            },
            {
                Constants.EleImpact, 1
            }
        }, Constants.JeneiZaganCharaId);

        public static readonly JeneiSummonCost Haures = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleImpact, 3
            },
            {
                Constants.EleFire, 2
            }
        }, Constants.JeneiHauresCharaId);

        public static readonly JeneiSummonCost Charon = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleImpact, 8
            },
            {
                Constants.EleLightning, 2
            }
        }, Constants.JeneiCharonCharaId);

        public static readonly JeneiSummonCost Megaera = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleFire, 1
            },
            {
                Constants.EleLightning, 1
            }
        }, Constants.JeneiMegaeraCharaId);

        public static readonly JeneiSummonCost Ulysses = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleFire, 2
            },
            {
                Constants.EleCold, 2
            }
        }, Constants.JeneiUlyssesCharaId);

        public static readonly JeneiSummonCost Daedalus = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleImpact, 3
            },
            {
                Constants.EleFire, 4
            }
        }, Constants.JeneiDaedalusCharaId);

        public static readonly JeneiSummonCost Iris = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleFire, 9
            },
            {
                Constants.EleCold, 4
            }
        }, Constants.JeneiIrisCharaId);

        public static readonly JeneiSummonCost Flora = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleImpact, 1
            },
            {
                Constants.EleLightning, 2
            }
        }, Constants.JeneiFloraCharaId);

        public static readonly JeneiSummonCost Eclipse = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleLightning, 3
            },
            {
                Constants.EleCold, 2
            }
        }, Constants.JeneiEclipseCharaId);

        public static readonly JeneiSummonCost Catastrophe = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleFire, 3
            },
            {
                Constants.EleLightning, 5
            }
        }, Constants.JeneiCatastropheCharaId);

        public static readonly JeneiSummonCost Moloch = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleLightning, 1
            },
            {
                Constants.EleCold, 2
            }
        }, Constants.JeneiMolochCharaId);

        public static readonly JeneiSummonCost Coatlicue = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleLightning, 3
            },
            {
                Constants.EleCold, 3
            }
        }, Constants.JeneiCoatlicueCharaId);

        public static readonly JeneiSummonCost Azul = new JeneiSummonCost(new Dictionary<int, int>
        {
            {
                Constants.EleImpact, 3
            },
            {
                Constants.EleCold, 4
            }
        }, Constants.JeneiAzulCharaId);

        public static readonly List<JeneiSummonCost> AllSummons = new List<JeneiSummonCost>
        {
            Cybele,
            Tiamat,
            Atlanta,
            Boreas,
            Zagan,
            Haures,
            Charon,
            Megaera,
            Ulysses,
            Daedalus,
            Iris,
            Flora,
            Eclipse,
            Catastrophe,
            Moloch,
            Coatlicue,
            Azul
        };

        public static string? GetSummon(Dictionary<int, int> stockpile)
        {
            return AllSummons
                    .Where(r => r.CanAfford(stockpile))
                    .OrderByDescending(r => r.Cost)
                    .FirstOrDefault()?.SummonId;
        }
    }
}