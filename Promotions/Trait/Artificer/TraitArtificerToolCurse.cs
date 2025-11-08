using System.Collections.Generic;
using PromotionMod.Stats;
using PromotionMod.Stats.Spellblade;
using UnityEngine;
namespace PromotionMod.Trait.Artificer;

public class TraitArtificerToolCurse : TraitArtificerTool
{
    public override string ArtificerToolId => "artificer_cursecube";

    internal static List<string> PossibleConditions = new List<string>
    {
        nameof(ConPoison),
        nameof(ConSleep),
        nameof(ConAttackBreak),
        nameof(ConArmorBreak),
        nameof(ConStatBreak),
        nameof(ConFaint)
    };
    
    public override bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        List<Chara> targets = pos.Charas;
        pos.PlayEffect("curse");
        foreach(Chara target in targets)
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
                        ActEffect.ProcAt(EffectId.Debuff, power, BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
                        {
                            origin = Act.CC.Chara,
                            n1 = randomCondition,
                        });
                    }
                    
                    target.AddCondition(SubPoweredCondition.Create(nameof(ConMagicBreak), power, 5));
                }
            }
        }
        owner.c_ammo--;
        return true;
    }
}