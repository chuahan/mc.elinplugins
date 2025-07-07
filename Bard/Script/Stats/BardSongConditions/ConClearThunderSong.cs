using System;
using System.Collections.Generic;
using System.Globalization;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using UnityEngine;
using Random = System.Random;

namespace BardMod.Stats.BardSongConditions;

public class ConClearThunderSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public override string TextDuration => "" + _stacks;

    private int _stacks = 0;

    public override void OnStart()
    {
        _stacks = 3;
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
            int targetCount = this.GodBlessed ? 3 : 1;
            Random random = new Random();
            HashSet<Chara> selectedCharas = new HashSet<Chara>();
            while (selectedCharas.Count < Math.Min(targetCount, potentialTargets.Count))
            {
                selectedCharas.Add(potentialTargets[random.Next(potentialTargets.Count)]);
            }
            foreach (Chara target in selectedCharas)
            {
                int damage = HelperFunctions.SafeDice(Constants.BardFinaleClearThunderName, this.power);
                ElementRef elementRef = EClass.setting.elements["eleLightning"];
                Effect lightningShot = Effect.Get("spell_arrow");
                lightningShot.sr.color = elementRef.colorSprite;
                TrailRenderer componentInChildren = lightningShot.GetComponentInChildren<TrailRenderer>();
                Color startColor = (componentInChildren.endColor = elementRef.colorSprite);
                componentInChildren.startColor = startColor;
                Point from = owner.pos;
                lightningShot.Play(CC.pos, 0f, target.pos);
                target.AddCondition<ConLightningSunder>(power);
                
                if (_stacks == 1)
                {
                    // The last strike will do additional Rhythm Stacks% hp damage.
                    int boostedDamage = HelperFunctions.SafeAdd(damage, (int)(target.hp * (RhythmStacks / 100)));
                    target.DamageHP(boostedDamage, Constants.EleLightning, 100, AttackSource.Condition, owner);
                    /*
                     * TODO: Add Greater Lightning FX
                     */
                    owner.PlaySound("spell_bolt");
                }
                else
                {
                    target.DamageHP(damage, 912, 100, AttackSource.Condition, owner);
                    /*
                     * TODO: Add Lightning FX
                     */
                    owner.PlaySound("spell_bolt");
                }
            }

            _stacks--;
            
            if (_stacks == 0)
            {
                Kill();
            }
        }
    }
    
    public override void OnWriteNote(List<string> list)
    {
        if (_stacks == 1)
        {
            list.Add("hintClearThunder2".lang(RhythmStacks.ToString(CultureInfo.InvariantCulture)));
        }
        else
        {
            list.Add("hintClearThunder1".lang(_stacks.ToString(CultureInfo.InvariantCulture)));
        }
    }
}