using System;
using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats.Harbinger;

/// <summary>
/// Overloaded Miasma Clone so that I can have Harbingers apply multiple miasmas.
/// </summary>
public class ConHarbingerMiasma : TimeDebuff
{
    public virtual int Element { get; }
    public virtual void ApplyCondition(Chara c)
    {
    }

    public override void Tick()
    {
        Dice dice = Dice.Create("miasma_", power);
        try
        {
            owner.DamageHP(dice.Roll(), Element, EClass.rnd(power / 2) + power / 4, AttackSource.Condition);
            ApplyCondition(owner);
            if (owner.IsAliveInCurrentZone && value > 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    foreach (Chara chara in HelperFunctions.GetCharasWithinRadius(owner.pos, 2F, owner, true, false))
                    {
                        if (!chara.HasCondition<ConMiasma>())
                        {
                            if (chara.Evalue(Constants.FeatHarbinger) > 0)
                            {
                                // Harbinger allies will consume the miasma instead, regaining health.
                                int healAmount = (int)(chara.MaxHP * 0.1F);
                                chara.HealHP(healAmount);
                                Kill();
                            }
                            else
                            {
                                Condition condition = chara.AddCondition(Condition.Create(power / 2, delegate(ConMiasma con)
                                {
                                    con.givenByPcParty = givenByPcParty;
                                    con.SetElement(refVal);
                                }));
                                if (condition != null)
                                {
                                    condition.value = value - 1;
                                }
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log(owner);
            Debug.Log(refVal);
        }
        Mod(-1);
    }

    public override void OnWriteNote(List<string> list)
    {
        Dice dice = Dice.Create("miasma_", power);
        list.Add("hintDOT".lang(dice.ToString(), sourceElement.GetName().ToLower()));
    }
}