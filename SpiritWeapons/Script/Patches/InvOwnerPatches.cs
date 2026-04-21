using Cwl.Helper.Extensions;
using HarmonyLib;
namespace SpiritWeapons.Patches;

[HarmonyPatch(typeof(InvOwner))]
internal class InvOwnerPatches : EClass
{
    // Patch to add the option to talk to Spirit Weapons
    [HarmonyPatch(typeof(InvOwner), nameof(InvOwner.ListInteractions), typeof(ButtonGrid), typeof(bool))]
    [HarmonyPostfix]
    internal static void SpiritWeapons_OnListInteractionsPatch(InvOwner __instance, ref InvOwner.ListInteraction __result, ButtonGrid b, bool context)
    {
        Thing t = b.card.Thing;
        Trait trait = t.trait;
        if (context && __instance.owner.IsPC)
        {
            if (trait is TraitCoreCrystal)
            {
                __result.Add("invAwaken".lang(), 150, delegate
                {
                    LayerDragGrid.Create(new InvOwnerAwaken(((TraitCoreCrystal)trait).VerificationFunction, t));
                }, "invAwaken".lang());
            }
            else
            {
                if (t.IsSpiritWeapon())
                {
                    __result.Add("invSpiritWeaponFeed".lang(), 150, delegate
                    {
                        LayerDragGrid.Create(new InvOwnerFeedSpiritWeapon(t));
                    }, "invSpiritWeaponFeed".lang());

                    __result.Add("tTalk".lang(), 299, delegate
                    {
                        // Get the Spirit Weapon Personality
                        int personality = t.GetFlagValue(Common.SpiritWeaponPersonality);

                        // Create a dummmy character to talk to with the assigned portrait of the spirit weapon.
                        Chara spiritWeaponPersonified = CharaGen.Create(Common.SpiritWeaponManifestCharaId, t.LV);
                        spiritWeaponPersonified.c_altName = t.GetStr(Common.SpiritWeaponName);
                        spiritWeaponPersonified.c_idPortrait = t.GetStr(Common.SpiritWeaponPortrait);

                        // Transfer the UID over so we can access the Spirit Weapon Stats.
                        spiritWeaponPersonified.SetFlagValue(Common.SpiritWeaponUid, t.uid);

                        spiritWeaponPersonified.ShowDialog($"spiritWeapon_{personality}");
                    });

                    if (__instance.owner.uid == t.GetFlagValue(Common.SpiritWeaponBondTargetFlag))
                    {
                        // Check the Spirit Weapon's Hunger - If it's too hungry it won't trigger.
                        if (t.GetFlagValue(Common.SpiritWeaponHunger) < 80)
                        {
                            // Check the Spirit Weapon's Gauge, if it's at 100 we can manifest it.
                            if (t.GetFlagValue(Common.SpiritWeaponGauge) == 100)
                            {
                                __result.Add("tManifest".lang(), 299, delegate
                                {
                                    t.ConsumeSpiritWeaponGauge();
                                    Msg.Nerun($"spiritweapon_manifest_{t.GetFlagValue(Common.SpiritWeaponPersonality)}".langList().RandomItem(),
                                        t.GetStr(Common.SpiritWeaponPortrait));
                                    // Create the character.
                                    Chara spiritWeaponPersonified = CharaGen.Create(Common.SpiritWeaponManifestCharaId, t.LV + t.Evalue(Common.SpiritWeaponEnc));
                                    spiritWeaponPersonified.SetSpriteOverride(t.GetStr(Common.SpiritWeaponSprite));
                                    // Create a copy of the Spirit Weapon and give it to them.
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}