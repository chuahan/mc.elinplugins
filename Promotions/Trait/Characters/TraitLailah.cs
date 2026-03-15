using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitLailah : TraitPromotionUniqueCharacter
{

    private static readonly List<int> RareScrollStock = new List<int>
    {
        8280, // Flying
        8288, // Reconstruction
        8281 // Faith
    };

    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("lailahRecruited") > 0;

    public override int Prepromotion => Constants.FeatSharpshooter;
    public override int RestockDay => 7;

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

        player.dialogFlags.TryGetValue("lailahShopOpen", out int lailahShopOpen);
        player.dialogFlags.TryGetValue("lailahFriendship1FollowupComplete", out int lailahShopT2);
        player.dialogFlags.TryGetValue("lailahFriendship4Completed", out int lailahShopT3);
        if (lailahShopOpen > 0)
        {
            merchantInventory.Add("promotion_manual", 1, 0);
            merchantInventory.Add("demotion_manual", 5, 0);

            // Completing her first friendship dialog will add a two randomized specialized skill books into her inventory.
            if (lailahShopT2 > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    merchantInventory.AddCard(ThingGen.Create("book_skill", -1, owner.LV));
                }
            }

            // Completing her final dialog will add two more randomized specialized skill books.
            // A random scroll out of Flying, Reconstruction, and Faith.
            // One of each Greater Enchant Armor and Greater Enchant Weapon
            // Two of each Enchant Armor and Enchant Weapon
            if (lailahShopT3 > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    merchantInventory.AddCard(ThingGen.Create("book_skill", -1, owner.LV));
                }

                merchantInventory.AddCard(ThingGen.CreateScroll(RareScrollStock.RandomItem()));
                merchantInventory.AddCard(ThingGen.CreateScroll(8251)); // Greater Weapon
                merchantInventory.AddCard(ThingGen.CreateScroll(8256)); // Greater Armor
                merchantInventory.AddCard(ThingGen.CreateScroll(8250).SetNum(2)); // Weapon
                merchantInventory.AddCard(ThingGen.CreateScroll(8255).SetNum(2)); // Armor
            }
        }
    }
}