namespace PromotionMod.Trait.Characters;

public class TraitGrandmaCat : TraitPromotionUniqueCharacter
{
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

        // Make Whole Roasted Meat out of Dragon Meat. 511
        // Make Soup. dish_soup
        for (int i = 0; i < 3; i++)
        {
            Thing wholeMeat = ThingGen.Create("511");
            wholeMeat.MakeRefFrom("dragon2");
            CraftUtil.WrapIngredient(wholeMeat, owner.Chara, CraftUtil.GetRandomLoveLunchIngredient(owner.Chara), CraftUtil.WrapType.Love);
            wholeMeat.elements.SetBase(701, 0);
            merchantInventory.AddCard(wholeMeat);

            Thing soup = ThingGen.Create("dish_soup");
            CraftUtil.WrapIngredient(soup, owner.Chara, CraftUtil.GetRandomLoveLunchIngredient(owner.Chara), CraftUtil.WrapType.Love);
            soup.elements.SetBase(701, 0);
            merchantInventory.AddCard(soup);
        }
    }

    public static Thing MakeGrandmaLunch(Chara c)
    {
        Thing lunchbox = ThingGen.Create("_dish_lunch");
        lunchbox.MakeRefFrom("dragon2");
        CraftUtil.WrapIngredient(lunchbox, c, CraftUtil.GetRandomLoveLunchIngredient(c), CraftUtil.WrapType.Love);
        lunchbox.elements.SetBase(701, 0);
        lunchbox.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        lunchbox.elements.SetBase(757, 1);
        return lunchbox;
    }
}