namespace PromotionMod.Trait.Characters;

public class TraitUshrir : TraitPromotionUniqueCharacter
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

        // merchantInventory.AddCard(ThingGen.CreateScroll(8255).SetNum(2));
    }
}