namespace PromotionMod.Trait.Artificer;

/// <summary>
///     Artificer Tools are basically chargeable canes that unleash large scale firepower.
///     Balance: Do I want to scale Artificer tools with their respective skills? Or will this be too powerful?
///     Fire - Pyroclasm Sword - Fire Sword, inflicts Fire Res Down, inflicts burn. Does increased damage against burning
///     targets, does increased damage against targets who have over 80% hp.
///     Ice - Glacial Axe - Ice Ball, inflicts Ice Res Down, guaranteed crit damage against chilled targets, inflicts
///     chill.
///     Thunder - Plasma Spear - Thunder Bolt, inflicts Lightning Res Down. Does bonus % HP damage against stunned enemies,
///     chance to inflict paralysis.
///     Impact - Gaia's Gauntlet - Impact Ball, inflicts Impact Res Down. Inflicts gravity and speed down.
///     Rainbow - Trinity  - Tri-Element Arrow, 25% chance to not consume charges, extends buff durations.
///     Time - Chronomancer's Hourglass - Hastens allies, Reduces ally cooldowns, slows enemies.
///     Healing - Bouquet of Life - Heals % HP, Increases Maximum HP, Applies Regeneration
///     Light - Heavenly Pearl - Heals % HP, applies instances of damage reduction to the ally.
///     Death - Curseladen Cube - Can inflict up to 3 random debuffs from a list, 30% chance. Reduces magic resistance.
///     Sound - Sonic Bomb - Grenade type tool that inflicts Dim
///     Poison - Bio Bomb - Grenade type tool that inflicts poison.
///     Light - Flash Bomb - Grenade type tool that inflicts blind.
/// </summary>
public class TraitArtificerTool : TraitTool
{
    public virtual string ArtificerToolId => "";
    public virtual int MaxCharges => 12;

    public virtual int ChargesConsumed => 1;

    public override void OnCreate(int lv)
    {
        owner.c_ammo = MaxCharges;
        owner.SetEncLv(100 * (100 + pc.Evalue(305) * 10 + pc.MAG / 2 + pc.PER / 2) / 100);
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
            // The enchant level of the artificer tool is the power factor.
            ArtificerToolEffect(p.cc, p.pos, owner.encLV);
            return false;
        }, owner, CursorSystem.IconRange);
    }

    public virtual bool ArtificerToolEffect(Chara cc, Point pos, int power)
    {
        return false;
    }
}