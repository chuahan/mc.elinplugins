using System.Collections.Generic;
using System.Linq;
using BardMod.Source;
using UnityEngine;

namespace NierMod.Traits;

internal class TraitNiyon : TraitUniqueChara
{
    public static bool IsBefriendedThroughDialog => EClass.player.dialogFlags.TryGetValue("niyonRecruited", 0) > 0;

    public override bool CanInvite => IsBefriendedThroughDialog;
    public override bool CanJoinParty => IsBefriendedThroughDialog;
    public override bool CanJoinPartyResident => IsBefriendedThroughDialog;
    public override bool CanBout => false;

    public void _OnBarter()
    {
        Thing merchantInventory = owner.things.Find("chest_merchant");
        if (merchantInventory == null)
        {
            merchantInventory = ThingGen.Create("chest_merchant");
            owner.AddThing(merchantInventory);
        }
        merchantInventory.c_lockLv = 0;

        merchantInventory.things.DestroyAll((Thing _t) => _t.GetInt(101) != 0);

        foreach (Thing oldStock in merchantInventory.things)
        {
            oldStock.invX = -1;
        }
        
        merchantInventory.Add("bard_windtouchedlyre", 1, 0);
        merchantInventory.Add("bard_seasitar", 1, 0);
        merchantInventory.Add("bard_palethundershamisen", 1, 0);
        merchantInventory.Add("bard_dragonbreathzither", 1, 0);

        // If you have done the repairs for the first batch.
        EClass.player.dialogFlags.TryGetValue("niyonRepairComplete", out int niyonRepairComplete);
        if (niyonRepairComplete > 0)
        {
            merchantInventory.Add("bard_yulanharp", 1, 0);
            merchantInventory.Add("bard_drumsofsleep", 1, 0);
            merchantInventory.Add("bard_radiantlyre", 1, 0);
            merchantInventory.Add("bard_abyssalmandolin", 1, 0);
        }

        // If you have completed Niyon/Selena Collab.
        EClass.player.dialogFlags.TryGetValue("niyonAwakened", out int niyonSelenaCollabComplete);
        if (niyonSelenaCollabComplete > 0)
        {
            merchantInventory.Add("bard_travelersbanjo", 1, 0);   
        }
        
        // If you have completed Niyon's Awakening (Repaired Nine-Realm Harp)
        EClass.player.dialogFlags.TryGetValue("niyonAwakened", out int niyonAwakened);
        if (niyonAwakened > 0)
        {
            merchantInventory.Add("bard_fantasiarealmzither", 1, 0);
            merchantInventory.Add("bard_ninerealmharp", 1, 0);   
        }
        
        // If you have completed Selena's Awakening.
        EClass.player.dialogFlags.TryGetValue("selenaAwakened", out int selenaAwakened);
        if (selenaAwakened > 0)
        {
            merchantInventory.Add("bard_waldmeister", 1, 0);
            merchantInventory.Add("bard_sarastro", 1, 0);
            merchantInventory.Add("bard_orpheuscradle", 1, 0);
            merchantInventory.Add("bard_dreamroamer", 1, 0);
        }
    }
}