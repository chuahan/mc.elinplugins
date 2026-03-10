using PromotionMod.Trait.Artificer;
namespace PromotionMod.Elements.PromotionAbilities.Artificer;

public class ActArtificerTool : Act
{
    public TraitArtificerTool trait;

    public override int MaxRadius => 5;

    public override int PerformDistance => 99;

    public override TargetType TargetType => this.trait.TargetType;

    public override string GetHintText(string str = "")
    {
        return GetText(str);
    }

    public override string GetText(string text = "")
    {
        return trait.ArtificerToolAction.langGame();
    }

    public override string GetTextSmall(Card c)
    {
        if (c == null)
        {
            return null;
        }
        return c.Name + c.GetExtraName();
    }
    
    public override bool CanPerform()
    {
        if (trait.IsTargetCast && !Act.CC.CanSeeLos(Act.TP)) return false;
        return base.CanPerform();
    }

    public override void OnMarkMapHighlights()
    {
        if (!EClass.scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        if (this.trait != null)
        {
            this.trait.MarkMapHighlights(EClass.scene.mouseTarget.pos);
        }
    }
    
    public override bool Perform()
    {
        if (this.trait.owner.c_ammo > 0)
        {
            string toolUseString = $"{this.trait.ArtificerToolId}_use";
            Act.CC.Say(toolUseString.langGame(CC.NameSimple, this.trait.ArtificerToolName));
            trait.owner.c_ammo--;
           
            Act.CC.PlayEffect("rod");
            Act.CC.PlaySound("rod");
            Act.CC.RemoveCondition<ConInvisibility>();

            // The enchant level of the artificer tool is the power factor.
            if (this.trait.IsTargetCast)
            {
                this.trait.ArtificerToolEffect(Act.CC, Act.TP, trait.owner.encLV);    
            }
            else
            {
                this.trait.ArtificerToolEffect(Act.CC, CC.pos, trait.owner.encLV);
            }
            
            Act.CC.ModExp(305, 50);
        }
        else
        {
            Act.CC.Say("artificertool_empty".langGame());
            Act.CC.PlaySound("rod_empty");   
        }
        return true;
    }
}