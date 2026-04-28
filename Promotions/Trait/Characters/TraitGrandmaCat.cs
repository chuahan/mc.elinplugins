using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitGrandmaCat : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => false;

    public override int RestockDay => 1;

    public override int Prepromotion => Constants.FeatSentinel;

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

        int dailySpecial = world.date.day % 7;
        switch (dailySpecial)
        {
            case 1: // Roast Dinner
                Thing wholeMeat = TraitGrandmaCat.MakeRoastDinner(owner.Chara);
                wholeMeat.ModNum(3);
                merchantInventory.AddCard(wholeMeat);
                break;
            case 2: // Steak Dinner
                Thing steakDinner = TraitGrandmaCat.MakeSteakDinner(owner.Chara);
                steakDinner.ModNum(3);
                merchantInventory.AddCard(steakDinner);
                break;
            case 3: // Karaage
                Thing chicken = TraitGrandmaCat.MakeFriedChicken(owner.Chara);
                chicken.ModNum(3);
                merchantInventory.AddCard(chicken);
                break;
            case 4: // Dragon Pot Pie
                Thing potPie = TraitGrandmaCat.MakePotPie(owner.Chara);
                potPie.ModNum(3);
                merchantInventory.AddCard(potPie);
                break;
            case 5: // Seafood Platter, Fish n Chips
                Thing fishFry = TraitGrandmaCat.MakeFishFry(owner.Chara);
                fishFry.ModNum(3);
                merchantInventory.AddCard(fishFry);
                break;
        }

        // Normal Stock: Soup and Bentos
        for (int i = 0; i < 3; i++)
        {
            Thing soup = TraitGrandmaCat.MakeSimpleSoup(owner.Chara);
            merchantInventory.AddCard(soup);

            Thing bento = TraitGrandmaCat.MakeDeluxeBento(owner.Chara);
            merchantInventory.AddCard(bento);
        }

        // Testing Purposes.
        Thing wholeMeat1 = TraitGrandmaCat.MakeRoastDinner(owner.Chara);
        merchantInventory.AddCard(wholeMeat1);
        Thing steakDinner1 = TraitGrandmaCat.MakeSteakDinner(owner.Chara);
        merchantInventory.AddCard(steakDinner1);
        Thing chicken1 = TraitGrandmaCat.MakeFriedChicken(owner.Chara);
        merchantInventory.AddCard(chicken1);
        Thing potPie1 = TraitGrandmaCat.MakePotPie(owner.Chara);
        merchantInventory.AddCard(potPie1);
        Thing fishFry1 = TraitGrandmaCat.MakeFishFry(owner.Chara);
        merchantInventory.AddCard(fishFry1);
    }

    public static Thing MakeGrandmaLunch(Chara c)
    {
        Thing lunchbox = ThingGen.Create("lunch_love");
        lunchbox.MakeRefFrom("dragon2");
        CraftUtil.WrapIngredient(lunchbox, c, ThingGen.Create("106"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(lunchbox, c, ThingGen.Create("101"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(lunchbox, c, ThingGen.Create("692"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(lunchbox, c, ThingGen.Create("695"), CraftUtil.WrapType.Love);
        lunchbox.elements.SetBase(701, 0);
        lunchbox.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        lunchbox.elements.SetBase(757, 1);
        return lunchbox;
    }

    public static Thing MakeDeluxeBento(Chara c)
    {
        Thing lunchbox = ThingGen.Create("511");
        Thing croquette = ThingGen.Create("106");
        croquette.MakeRefFrom("dragon");
        // Fishes: Salmon 87, Mackerel 78, Sea Bream 86, Sweetfish 75
        Thing grilledFish = ThingGen.Create("109");
        string[] randomFish =
        {
            "87",
            "78",
            "86",
            "75"
        };
        grilledFish.MakeRefFrom(randomFish.RandomItem());

        Thing rice = ThingGen.Create("692");
        Thing pickledVeggie = ThingGen.Create("101");
        pickledVeggie.MakeRefFrom("bamboo_shoot");
        CraftUtil.WrapIngredient(lunchbox, c, croquette, CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(lunchbox, c, grilledFish, CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(lunchbox, c, rice, CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(lunchbox, c, pickledVeggie, CraftUtil.WrapType.Love);
        lunchbox.elements.SetBase(701, 0);
        lunchbox.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        lunchbox.elements.SetBase(757, 1);
        lunchbox.c_priceFix = -50;
        return lunchbox;
    }

    public static Thing MakeSimpleSoup(Chara c)
    {
        Thing heartySoup = ThingGen.Create("dish_soup");
        CraftUtil.WrapIngredient(heartySoup, c, ThingGen.Create("miso"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(heartySoup, c, ThingGen.Create("783"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(heartySoup, c, ThingGen.Create("mushroom"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(heartySoup, c, ThingGen.Create("seaweed"), CraftUtil.WrapType.Love);
        heartySoup.elements.SetBase(701, 0);
        heartySoup.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        heartySoup.elements.SetBase(757, 1);
        heartySoup.c_priceFix = -50;
        return heartySoup;
    }

    public static Thing MakeRoastDinner(Chara c)
    {
        Thing wholeMeat = ThingGen.Create("511");
        wholeMeat.MakeRefFrom("dragon");
        CraftUtil.WrapIngredient(wholeMeat, c, ThingGen.Create("yakiimo"), CraftUtil.WrapType.Love); // Roasted Taters
        CraftUtil.WrapIngredient(wholeMeat, c, ThingGen.Create("108"), CraftUtil.WrapType.Love); // Stewed Veggies
        wholeMeat.elements.SetBase(701, 0);
        wholeMeat.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        wholeMeat.elements.SetBase(757, 1);
        wholeMeat.c_priceFix = -50;
        return wholeMeat;
    }

    public static Thing MakeSteakDinner(Chara c)
    {
        Thing meal = ThingGen.Create("511");
        Thing meat = ThingGen.Create("110");
        meat.MakeRefFrom("dragon");
        CraftUtil.WrapIngredient(meal, c, meat, CraftUtil.WrapType.Love); // Meat on Bone
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("107"), CraftUtil.WrapType.Love); // Salad
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("fried_potato"), CraftUtil.WrapType.Love); // Fries
        meal.elements.SetBase(701, 0);
        meal.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        meal.elements.SetBase(757, 1);
        meal.c_priceFix = -50;
        return meal;
    }

    public static Thing MakeFriedChicken(Chara c)
    {
        Thing meal = ThingGen.Create("fried_chicken");
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("fried_potato"), CraftUtil.WrapType.Love); // Whole Meat
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("785"), CraftUtil.WrapType.Love); // Lemon
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("sauce_soy"), CraftUtil.WrapType.Love); // Soy Sauce
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("692"), CraftUtil.WrapType.Love); // Cooked Rice
        meal.elements.SetBase(701, 0);
        meal.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        meal.elements.SetBase(757, 1);
        meal.c_priceFix = -50;
        return meal;
    }

    public static Thing MakePotPie(Chara c)
    {
        Thing meal = ThingGen.Create("pie_meat");
        Thing meat = ThingGen.Create("511");
        meat.MakeRefFrom("dragon");
        CraftUtil.WrapIngredient(meal, c, meat, CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("carrot"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("potato"), CraftUtil.WrapType.Love);
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("701"), CraftUtil.WrapType.Love); // Truffle
        meal.elements.SetBase(701, 0);
        meal.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        meal.elements.SetBase(757, 1);
        meal.c_priceFix = -50;
        return meal;
    }

    public static Thing MakeFishFry(Chara c)
    {
        Thing meal = ThingGen.Create("683");
        // Fishes: Salmon 87, Mackerel 78, Sea Bream 86, Sweetfish 75
        string[] randomFish =
        {
            "87",
            "78",
            "86",
            "75"
        };
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create(randomFish.RandomItem()), CraftUtil.WrapType.Love); // Whole Meat
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create(randomFish.RandomItem()), CraftUtil.WrapType.Love); // Whole Meat
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("785"), CraftUtil.WrapType.Love); // Lemon
        CraftUtil.WrapIngredient(meal, c, ThingGen.Create("fried_potato"), CraftUtil.WrapType.Love); // Fries
        meal.elements.SetBase(701, 0);
        meal.c_dateCooked = world.date.GetRaw() + 2 * 48 * 60;
        meal.elements.SetBase(757, 1);
        meal.c_priceFix = -50;
        return meal;
    }
}