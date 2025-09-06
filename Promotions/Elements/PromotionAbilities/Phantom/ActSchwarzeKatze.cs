using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats.Phantom;
namespace PromotionMod.Elements.PromotionAbilities.Phantom;

/// <summary>
/// Strike an enemy twice, then retreat away from the enemy. 30 Stam
/// Finisher: Drops orbital lasers on all enemies within 3 radius.
/// </summary>
public class ActSchwarzeKatze : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatPhantom) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.PhantomId.lang()));
            return false;
        }
        if (Act.TC == null)
        {
            return false;
        }
        return ACT.Melee.CanPerform();
    }
    
    public override Cost GetCost(Chara c)
    {
        return new Cost()
        {
            cost = 30,
            type = CostType.SP,
        };
    }
    
    public override bool Perform()
    {
        ConPhantomMark phantomMark = TC.Chara.GetCondition<ConPhantomMark>();
        int currMarks = phantomMark?.Stacks ?? 0;
        TC.PlayEffect("ab_bladestorm");
        TC.PlaySound("ab_swarm");
        new ActSchwarzeKatzeMelee().Perform(Act.CC, TC);
        TweenUtil.Delay(0.7F, delegate
        {
            TC.PlayEffect("ab_bladestorm");
            TC.PlaySound("ab_swarm");
            new ActSchwarzeKatzeMelee().Perform(Act.CC, TC);
        });
        Act.CC.Chara.TryMoveFrom(Act.TC.pos);
        FeatPhantom.AddPhantomMarks(TC.Chara, 2);

        // Trigger Finisher if Target has 10 Phantom Stacks
        if (currMarks == 10)
        {
            // Remove Phantom Mark from the target.
            phantomMark.Kill();

            // Lock onto all foes within 3 radius and drop a laser.
            foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 3F, CC, false, true))
            {
                Effect laser = Effect.Get("aura_heaven");
                ElementRef colorRef = setting.elements["eleDarkness"];
                laser.SetParticleColor(colorRef.colorTrail, changeMaterial: true, "_TintColor");
                laser.sr.color = colorRef.colorSprite;
                laser.Play(CC.pos);
                int damage = HelperFunctions.SafeDice("phantom_schwarzekatze", this.GetPower(CC));
                HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleMagic);

                // Proc Stamina and Mana Recovery of Phantom
            }
        }
        return true;
    }
}