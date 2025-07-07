using System.Collections.Generic;
using BardMod.Common;

namespace BardMod.Stats.BardSongConditions;

/*
 * Allies gain damage, spell enhance, accuracy, PDR, EDR.
 * If allies take lethal damage, activates Undying.
 * For the next 5 turns, allies will instead heal from damage instead.
 * What do I want to do with Yevan?
 */
public class ConHeavensFallSong : ConBardSong
{
    public override Constants.BardSongType SongType => Constants.BardSongType.Finale;
    public override ConditionType Type => ConditionType.Buff;
    
    public bool ActivatedUndying;
    public int UndyingTurns;

    public void ActivateUndying()
    {
        // TODO: Add SFX
        // TODO: Add FX
        ActivatedUndying = true;
        UndyingTurns = 3 + P2;
        
        // Purge all debuffs.
        foreach (Condition item5 in owner.conditions.Copy())
        {
            if (item5.Type == ConditionType.Debuff &&
                !item5.IsKilled &&
                item5 is not ConWrath && // Don't purge Wrath of God.
                item5 is not ConDeathSentense) // Don't purge Death Sentence.
            {
                owner.Say("removeHex", owner, item5.Name.ToLower());
                item5.Kill();
            }
        }
    }
    
    public override void OnStartOrStack()
    {
        ActivatedUndying = false;
        base.OnStartOrStack();
    }

    public override void Tick()
    {
        if (ActivatedUndying)
        {
            if (UndyingTurns > 0)
            {
                UndyingTurns--;
            }
        }
        base.Tick();
    }
    
    public override void OnWriteNote(List<string> list)
    {
        if (!ActivatedUndying) list.Add("hintHeavensFall1".lang());
        if (UndyingTurns != 0) list.Add("hintHeavensFall2".lang(UndyingTurns.ToString()));
    }
}