using System.Collections.Generic;
namespace PromotionMod.Trait.Characters;

public class TraitBoutiqueShopkeeper : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => false;

    public static List<int> PossibleMaterials = new List<int>
    {
        73, // cashmere
        74, // zylon
        75, // spirit
        76, // dusk
        77, // griffon scale
        80, // wool
        81, // spidersilk
    };
    
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

        // Create a stock of various clothing.
        merchantInventory.AddCard(ThingGen.Create("robe_pope", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("girdle_corset", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("dress_wedding", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("hat_wedding", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("tuxedo", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("cloak_light", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("cloak_light", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("cloak_light", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("shirt", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("shirt", PossibleMaterials.RandomItem(), this.ShopLv));
        merchantInventory.AddCard(ThingGen.Create("shirt", PossibleMaterials.RandomItem(), this.ShopLv));
    }
}