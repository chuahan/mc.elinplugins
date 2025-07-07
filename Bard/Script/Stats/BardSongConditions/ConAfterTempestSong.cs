using System.Collections.Generic;
using BardMod.Common;
using BardMod.Common.HelperFunctions;

namespace BardMod.Stats.BardSongConditions;

/*
 * Beauty after the Storm
 * Lightning ball bursts out of the caster every few turns for x amount of turns that only hits enemies and stuns them.
 * Inflicts Lightning Sundered on hit.
 * Tracks how many enemies are hit with roaring thunder.
 * When Roaring Thunder expires. Overguard based on stacks is added to all allies.
 */
public class ConAfterTempestSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;

    private int _stacks = 0;

    public override void Tick()
    {
        // Will not persist in regions.
        if (_zone.IsRegion)
        {
            Kill();
        }
        
        // TODO: Play FX
        // TODO: Play SFX
        Effect lightningBallEffect = Effect.Get("Element/ball_Lightning");
        lightningBallEffect.Play(owner.pos).Flip(owner.pos.x > CC.pos.x);
        owner.PlaySound("spell_ball");
        
        int damage = HelperFunctions.SafeDice(Constants.BardFinaleAfterTempestName, power);
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos,4f, owner, false, true);
        foreach (Chara target in targets)
        {
            target.AddCondition<ConLightningSunder>(power);
            target.DamageHP(damage, Constants.EleLightning, eleP: 100, AttackSource.Condition, owner);
            _stacks++;
        }
        
        base.Tick();
    }

    public override void OnRemoved()
    {
        // TODO: Play FX
        // TODO: Play SFX
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(owner.pos,5f, this.Caster, true, false);
        int overguard = HelperFunctions.SafeMultiplier(_stacks, 1 + (power / 40.0F));
        foreach (Chara target in targets)
        {
            (target.AddCondition<ConOverguard>(1) as ConOverguard)?.AddOverGuard(overguard);   
        }
        base.OnRemoved();
    }
    
    public override void OnWriteNote(List<string> list)
    {
        string plural = this._stacks > 1 ? "s" : "";
        list.Add("hintAfterTempestSong".lang(this._stacks.ToString(), plural));
    }
}