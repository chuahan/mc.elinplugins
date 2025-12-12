using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     Shapers of the natural energies. Jenei are those who call upon the natural forces of Earth, Fire, Air, and Water.
///     Jenei focus on combining the four natural elements to summon great representatives of the natural forces to aid
///     them in combat.
///     They specialize in invoking mighty spells with a variety of effects through building up amounts of each of the
///     elements.
///     Upon promotion, you must choose an element to align to, which will grant you two abilities based on that element.
///     Venus: The Element of Substance, Venus Jenei manipulate earth.
///     Skill - Move - Moves the target card away from you. If PC uses it, it can be used on
///     Skill - Mother Gaia - Causes an Earth Spire to erupt out of the ground, inflicting Impact damage, does half damage
///     to neighboring tiles.
///     Basically small scale Earthquake with 2F radius instead.
///     Mars: The Element of Power, Mars Jenei manipulate fire.
///     Skill - Blaze - Does Fire Hand, but also sets a fire pillar on the target location.
///     Skill - Dragon Fume - Basically Fire Breath with guaranteed chance to inflict Burning.
///     Jupiter: The Element of Mind and Spirit, Jupiter Jenei manipulate wind and lightning
///     Skill - Reveal -
///     When used by the Player, it acts like a Stethoscope as a spell.
///     When used by NPCs, the NPC inflicts ConMagicBreak.
///     Skill - Shine Plasma - Strikes a single target with a large lightning bolt, dealing heavy lightning damage.
///     Mercury: The Element of Healing and Life, Mercury Jenei manipulate water and ice.
///     Skill - Deluge - Drops a deluge of water on the target. Guaranteed to inflict Wet and leave a puddle.
///     Skill - Ply - Heals a single ally. Power equivalent to Heal.
///     Spirit Summon - Skill - Checks your Jenei Condition and uses your charges to summon an incredibly powerful spirit
///     to aid you in battle temporarily.
///     Passive - Conspectus of the Adept - Can convert spellbooks into Impact/Fire/Lightning/Ice.
/// </summary>
public class FeatJenei : PromotionFeat
{
    public override string PromotionClassId => Constants.JeneiId;
    public override int PromotionClassFeatId => Constants.FeatJenei;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSpiritSummonId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActSpiritSummonId, 30, false);
        switch (c.GetFlagValue(Constants.JeneiAttunementFlag))
        {
            // All elements get two spells of that element.
            case 0: // Venus
                c.ability.Add(Constants.ActJeneiMoveId, 50, false);
                c.ability.Add(Constants.ActJeneiMotherGaiaId, 75, false);
                c.ability.Add(50115, 75, false); // Impact Ball
                c.ability.Add(50415, 75, false); // Impact Hand
                break;
            case 1: // Mars
                c.ability.Add(Constants.ActJeneiBlazeId, 50, false);
                c.ability.Add(Constants.ActJeneiDragonPlumeId, 75, false);
                c.ability.Add(50215, 75, false); // Fire Breathe
                c.ability.Add(51000, 75, false); // Fire Sword
                break;
            case 2: // Jupiter
                c.ability.Add(Constants.ActJeneiRevealId, 50, false);
                c.ability.Add(Constants.ActJeneiShinePlasmaId, 75, false);
                c.ability.Add(50302, 75, false); // Lightning Bolt
                c.ability.Add(50502, 75, false); // Lightning Arrow
                break;
            case 3: // Mercury
                c.ability.Add(Constants.ActJeneiDelugeId, 50, false);
                c.ability.Add(Constants.ActJeneiPlyId, 75, true);
                c.ability.Add(50201, 75, false); // Cold Breathe
                c.ability.Add(51001, 75, false); // Cold Sword
                break;
        }
    }


    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "farmer";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        // For PCs, depending on which element they attuned to, add their two abilities.
        if (owner.Chara.IsPC)
        {
            switch (owner.Chara.GetFlagValue(Constants.JeneiAttunementFlag))
            {
                // All elements get two spells of that element.
                case 0: // Venus
                    owner.Chara.AddElement(Constants.ActJeneiMoveId, 0);
                    owner.Chara.AddElement(Constants.ActJeneiMotherGaiaId, 0);
                    break;
                case 1: // Mars
                    owner.Chara.AddElement(Constants.ActJeneiBlazeId, 0);
                    owner.Chara.AddElement(Constants.ActJeneiDragonPlumeId, 0);
                    break;
                case 2: // Jupiter
                    owner.Chara.AddElement(Constants.ActJeneiRevealId, 0);
                    owner.Chara.AddElement(Constants.ActJeneiShinePlasmaId, 0);
                    break;
                case 3: // Mercury
                    owner.Chara.AddElement(Constants.ActJeneiDelugeId, 0);
                    owner.Chara.AddElement(Constants.ActJeneiPlyId, 0);
                    break;
            }
        }
        base._OnApply(add,eleOwner, hint);
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