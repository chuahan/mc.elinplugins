namespace PromotionMod.Trait.ArtificerTools;

/// <summary>
///     Artificer Tools are basically chargeable canes that unleash large scale firepower.
///     They are explicitly friendly fire incapable, making them quite good.
///     Fire - Pyroclasm Sword - Fire Sword, inflicts Fire Res Down, inflicts burn. Does increased damage against burning
///     targets, does increased damage against targets who have over 80% hp.
///     Ice - Glacial Axe - Ice Ball, inflicts Ice Res Down, guaranteed crit damage against chilled targets, inflicts
///     chill.
///     Thunder - Plasma Spear - Thunder Bolt, inflicts Lightning Res Down. does bonus % HP damage against stunned enemies,
///     chance to inflict paralysis.
///     Impact - Gaia's Gauntlet - Earthquake, inflicts Impact Res Down. inflicts gravity and speed down.
///     Rainbow - Trinity  - Tri-Element Arrow, 25% chance to not consume charges.
///     Time - Chronomancer's Hourglass - MagDev Scaling, Hastens allies, Reduces ally cooldowns, extends buff durations.
///     Healing - Bouquet of Life - Heals % HP, Increases Maximum HP, Applies Regeneration
///     Light - Heavenly Pearl - Heals % HP, applies instances of damage reduction OR damage increase to the ally.
///     Death - Curseladen Cube - Can inflict up to 3 random debuffs from a list, 30% chance. Reduces magic resistance.
///     Reduces WILL.
/// </summary>
public class TraitArtificerTool : TraitTool
{

    public int ToolPower;
    public virtual string ArtificerToolId => "";
    public int MaxCharges => 12;

    public virtual int ChargesConsumed => 1;

    public override void OnCreate(int lv)
    {
        owner.c_ammo = MaxCharges;
        ToolPower = lv;
    }

    public void Recharge(int chargeAmount)
    {
        owner.c_ammo += chargeAmount;
        if (owner.c_ammo >= MaxCharges) owner.c_ammo = MaxCharges;
    }

    public override void OnSimulateHour(VirtualDate date)
    {
        if (date.IsRealTime && owner.c_ammo < MaxCharges)
        {
            owner.c_ammo++;
        }
    }

    public override void SetMainText(UIText t, bool hotitem)
    {
        string text = owner.c_ammo + "/" + MaxCharges;
        t.SetText(text ?? "", FontColor.Charge);
        t.SetActive(enable: true);
    }

    public override void TrySetHeldAct(ActPlan p)
    {
        if (p.cc.CanSeeLos(p.pos) == false && owner.c_ammo > 0) return;
        p.TrySetAct(ArtificerToolId.lang(), delegate
        {
            ArtificerToolEffect(p.pos);
            return false;
        }, owner, CursorSystem.IconRange);
    }

    public virtual bool ArtificerToolEffect(Point pos)
    {
        return false;
    }
}