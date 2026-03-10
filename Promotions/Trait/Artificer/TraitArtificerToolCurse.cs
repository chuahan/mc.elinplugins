using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolCurse : TraitArtificerTool
{

    internal static List<string> PossibleConditions = new List<string>
    {
        nameof(ConPoison),
        nameof(ConSleep),
        nameof(ConAttackBreak),
        nameof(ConArmorBreak),
        nameof(ConStatBreak),
        nameof(ConFaint)
    };

    public override string ArtificerToolId => "artificer_cursecube";

    public override float EffectRadius => 2;

    public virtual void MarkMapHighlights(bool shouldHighlight, Point target)
    {
        EClass._map.ForeachSphere(target.x, target.z, EffectRadius, delegate(Point p)
        {
            if (!p.HasBlock && shouldHighlight)
            {
                p.SetHighlight(8);
            }
        });
    }
    
    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(pos, EffectRadius, cc, false, false);
        pos.PlayEffect("curse");
        foreach (Chara target in targets)
        {
            if (target.IsHostile(cc))
            {
                for (int i = 0; i < 3; i++)
                {
                    string randomCondition = PossibleConditions.RandomItem();
                    if (randomCondition is nameof(ConAttackBreak) or nameof(ConArmorBreak) or nameof(ConStatBreak))
                    {
                        target.AddCondition(SubPoweredCondition.Create(randomCondition, power, 5));
                    }
                    else
                    {
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, Act.CC, target, target.pos, true, new ActRef
                        {
                            origin = Act.CC.Chara,
                            n1 = randomCondition
                        });
                    }

                    target.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), power, 5));
                }
            }
        }
        return true;
    }
}