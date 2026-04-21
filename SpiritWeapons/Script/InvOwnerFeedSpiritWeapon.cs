using Cwl.Helper.Extensions;
namespace SpiritWeapons;

public class InvOwnerFeedSpiritWeapon : InvOwnerDraglet
{

    public InvOwnerFeedSpiritWeapon(Card owner = null, Card container = null, CurrencyType _currency = CurrencyType.None)
            : base(owner, container, _currency)
    {
    }
    public override bool CanTargetAlly => false;

    public override string langTransfer => "invFeedSpiritWeapon".lang(owner.GetStr(Common.SpiritWeaponName));

    public override bool ShouldShowGuide(Thing t)
    {
        if (t != owner)
        {
            return (t.IsMeleeWeapon || t.IsRangedWeapon) && !t.HasElement(Common.SpiritWeaponEnc) && t.rarity != Rarity.Artifact && !t.IsImportant;
        }
        return false;
    }

    public override void _OnProcess(Thing t)
    {
        Dialog.YesNo("awakenWeapon_FeedConfirm".lang(t.Name, owner.c_altName), delegate
        {
            float multiplier = owner.GainSpiritWeaponExperience(t);
            int enjoymentIndex = 0;
            switch (multiplier)
            {
                case >= 2.5F:
                    enjoymentIndex = 2;
                    owner.BondSpiritWeapon(5);
                    break;
                case >= 1.5F:
                    enjoymentIndex = 1;
                    owner.BondSpiritWeapon(2);
                    break;
                default:
                    owner.BondSpiritWeapon();
                    break;
            }

            Msg.Nerun($"spiritweapon_feed_{owner.GetFlagValue(Common.SpiritWeaponPersonality)}_{enjoymentIndex}".langList().RandomItem(),
                owner.GetStr(Common.SpiritWeaponPortrait));
            t.ModNum(-1);
        });
    }
}