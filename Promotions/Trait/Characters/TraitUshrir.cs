using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitUshrir : TraitPromotionUniqueCharacter
{

    public static List<string> UshrirWeapons = new List<string>
    {
        "knife_throwing",
        "knife_valor",
        "knife_killer",
        "rapier",
        "rapier_valor",
        "rapier_killer",
        "lance",
        "lance_valor",
        "lance_killer",
        "axe_valor",
        "axe_killer",
        "sword_valor",
        "sword_armorslayer"
    };

    public override bool IsBefriendedThroughDialog => false;

    public override int RestockDay => 5;

    public void _OnBarter()
    {
        Thing merchantInventory = owner.things.Find("chest_merchant");
        if (merchantInventory == null)
        {
            merchantInventory = ThingGen.Create("chest_merchant");
            owner.AddThing(merchantInventory);
        }
        merchantInventory.c_lockLv = 0;

        merchantInventory.things.DestroyAll(_t => _t.GetInt(101) != 0);

        foreach (Thing oldStock in merchantInventory.things)
        {
            oldStock.invX = -1;
        }

        // All but 2 weapons will be made out of Steel (20), the remaining 2 will be made out of Adamantite (29).
        // All of the new Valor/Killer weapons will be included here except for Scythe and Bow.
        int inventoryStock = HelperFunctions.GetMaximumStock(ShopLv);
        int adamantiteWeapons = 2;
        for (int i = 0; i < inventoryStock; i++)
        {
            string weaponSelected = UshrirWeapons.RandomItem();
            if (adamantiteWeapons > 0 && EClass.rnd(5) == 0)
            {
                merchantInventory.AddCard(ThingGen.Create(weaponSelected, 29, ShopLv));
                adamantiteWeapons--;
            }
            else
            {
                merchantInventory.AddCard(ThingGen.Create(weaponSelected, 20, ShopLv));
            }
        }
    }
}