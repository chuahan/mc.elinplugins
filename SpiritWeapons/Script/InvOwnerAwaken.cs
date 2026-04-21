using System;
namespace SpiritWeapons;

public class InvOwnerAwaken : InvOwnerDraglet
{

    private readonly Func<Thing, bool> _verificationFunction;

    public InvOwnerAwaken(Func<Thing, bool> verificationFunction, Card owner = null, Card container = null, CurrencyType _currency = CurrencyType.None)
            : base(owner, container, _currency)
    {
        _verificationFunction = verificationFunction;
    }
    public override bool CanTargetAlly => false;

    public override string langTransfer => "invAwaken";

    public override bool ShouldShowGuide(Thing t)
    {
        if (t != owner)
        {
            return t.CanAwakenSpiritWeapon() && _verificationFunction(t);
        }
        return false;
    }

    public override void _OnProcess(Thing t)
    {
        // Create a dummy character.
        Chara spiritWeaponPersonified = CharaGen.Create(Common.SpiritWeaponManifestCharaId, t.LV);

        // TODO: Do I want to be able to read the data on the existing Core Crystal in case it is already a defined Spirit Weapon?

        Dialog.YesNo("awakenWeapon_Confirm".lang(), delegate
        {
            Dialog.InputName("awakenWeapon_Configure", t.GetStr(Common.SpiritWeaponName, NameGen.getRandomName()), delegate(bool cancel, string text)
            {
                if (!cancel)
                {
                    // Name the Weapon.
                    spiritWeaponPersonified.c_altName = text;
                    spiritWeaponPersonified.c_idPortrait = Portrait.GetRandomPortrait(EClass.rnd(1) + 1, spiritWeaponPersonified.GetIdPortraitCat());
                    LayerEditPortrait portraitSelect = ui.AddLayer<LayerEditPortrait>();

                    // Select the Portrait
                    portraitSelect.Activate(spiritWeaponPersonified);
                    portraitSelect.SetOnKill(delegate
                    {
                        // Select the Skin.
                        LayerEditSkin skinSelect = ui.AddLayer<LayerEditSkin>();
                        skinSelect.Activate(spiritWeaponPersonified);
                        skinSelect.SetOnKill(delegate
                        {
                            // Initialize the Spirit Weapon.
                            t.InitializeSpiritWeapon(
                                spiritWeaponPersonified.c_altName,
                                spiritWeaponPersonified.c_idPortrait,
                                spiritWeaponPersonified.c_idSpriteReplacer ?? "spiritweapon_manifested",
                                destInvOwner.Chara);
                            t.rarity = Rarity.Artifact;
                            t.elements.ModBase(Common.SpiritWeaponEnc, owner.Evalue(Common.SpiritWeaponEnc));
                            t.c_altName = t.GetStr(Common.SpiritWeaponName); // Update the name of the weapon.
                            owner.ModNum(-1);
                        });
                    });
                }
            });
        });
    }
}