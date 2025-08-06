using System;
using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Patches;
using UnityEngine;
using Random = System.Random;

namespace BardMod.Stats.BardSongConditions;

public class ConClearThunderSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    public override string TextDuration => "" + Stacks;

    public override void OnStart()
    {
        Stacks = 3;
        base.OnStart();
    }

    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }

        List<Chara> potentialTargets = HelperFunctions.GetCharasWithinRadius(owner.pos, 5f, owner, false, true);
        if (potentialTargets.Count != 0)
        {
            Random random = new Random();
            HashSet<Chara> selectedCharas = new HashSet<Chara>();
            while (selectedCharas.Count < Math.Min(3, potentialTargets.Count))
            {
                selectedCharas.Add(potentialTargets[random.Next(potentialTargets.Count)]);
            }

            int damage = HelperFunctions.SafeDice(Constants.BardFinaleClearThunderName, power);

            if (selectedCharas.Count > 0)
            {
                HashSet<Chara>.Enumerator targetEnumerator = selectedCharas.GetEnumerator();
                for (int i = 0; i < 3; i++)
                {
                    if (targetEnumerator.IsNull()) break;
                    if (!targetEnumerator.MoveNext())
                    {
                        // Godbless will allow you to strike the same target multiple times.
                        if (GodBlessed)
                        {
                            targetEnumerator = selectedCharas.GetEnumerator();
                            targetEnumerator.MoveNext();
                        }
                        else
                        {
                            break;
                        }
                    }

                    Chara? target = targetEnumerator.Current;
                    if (target is null) continue;

                    ElementRef elementRef = setting.elements["eleLightning"];
                    Effect lightningShot = Effect.Get("spell_arrow");
                    lightningShot.sr.color = elementRef.colorSprite;
                    TrailRenderer componentInChildren = lightningShot.GetComponentInChildren<TrailRenderer>();
                    Color startColor = componentInChildren.endColor = elementRef.colorSprite;
                    componentInChildren.startColor = startColor;
                    Point from = owner.pos;

                    lightningShot.Play(CC.pos, 0f, target.pos);
                    target.AddCondition<ConLightningSunder>(power);

                    if (Stacks == 1)
                    {
                        // The last strike will do additional Rhythm Stacks% hp damage.
                        int boostedDamage = HelperFunctions.SafeAdd(damage, target.hp * (RhythmStacks / 100));
                        // target.DamageHP(dmg: boostedDamage, ele: Constants.EleLightning, eleP: 100, attackSource: AttackSource.Condition, origin: owner);
                        BardCardPatches.CachedInvoker.Invoke(
                            target,
                            new object[] { boostedDamage, Constants.EleLightning, 100, AttackSource.Condition, owner }
                        );
                        /*
                         * TODO: Add Greater Lightning FX
                         */
                        owner.PlaySound("spell_bolt");
                    }
                    else
                    {
                        // target.DamageHP(dmg: damage, ele: Constants.EleLightning, eleP: 100, attackSource: AttackSource.Condition, origin: owner);
                        BardCardPatches.CachedInvoker.Invoke(
                            target,
                            new object[] { damage, Constants.EleLightning, 100, AttackSource.Condition, owner }
                        );
                        /*
                         * TODO: Add Lightning FX
                         */
                        owner.PlaySound("spell_bolt");
                    }
                }

                Stacks--;

                if (Stacks == 0)
                {
                    Kill();
                }
            }
        }
    }

    public override void OnWriteNote(List<string> list)
    {
        if (Stacks == 1)
        {
            list.Add("hintClearThunder2".lang(RhythmStacks.ToString(CultureInfo.InvariantCulture)));
        }
        else
        {
            list.Add("hintClearThunder1".lang(Stacks.ToString(CultureInfo.InvariantCulture)));
        }
    }
}