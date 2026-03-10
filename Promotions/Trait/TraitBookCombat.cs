using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Trait;

public class TraitBookCombat : TraitScroll
{
    public int idEle => owner.refVal;
    
    public override SourceElement.Row source => EClass.sources.elements.map[idEle];

    public static List<int> SpawnableCombatSkills = new List<int>
    {
	    Constants.FeatAegisId,
	    Constants.FeatPaviseId,
	    Constants.FeatAstraId,
	    Constants.FeatLunaId,
	    Constants.FeatSolId,
	    Constants.FeatGaleforceId,
	    Constants.FeatNihilId,
	    Constants.FeatLethalityId,
	    Constants.FeatRendHeavenId,
	    Constants.FeatVengeanceId,
	    Constants.FeatDeadeyeId,
	    Constants.FeatVantageId,
    };
    
    public virtual bool IsPlan => false;

    public override string IdNoRestock => owner.id + "_" + source?.id;

    public override bool IsOnlyUsableByPc => false;
    
    public override bool CanRead(Chara c)
    {
	    if (c.GetFlagValue(Constants.AdvancedCombatSkillFlag) > 0) return false;
        return !c.isBlind;
    }
    
    public override int GetActDuration(Chara c)
    {
        return 5;
    }
    
    public override void OnCreate(int lv)
	{
		owner.refVal = SpawnableCombatSkills.RandomItem();
	}

	public override void SetName(ref string s)
	{
		if (idEle != 0)
		{
			
			string text = "";
			s = "_of".lang((source.GetName() + text).Bracket(1), s);
		}
	}

	public override void OnRead(Chara c)
	{
		//owner.Say(IsPlan ? "skillbook_learnPlan" : "skillbook_learn", c, source.GetName());
		c.SetFeat(idEle);
		c.SetFlagValue(Constants.AdvancedCombatSkillFlag, idEle);
		Msg.Say("combatskilllearned".lang(c.NameSimple, EClass.sources.elements.map[idEle].GetName()));
		
		c.Say("spellbookCrumble", owner.Duplicate(1));
		owner.ModNum(-1);
	}

	public override int GetValue()
	{
		return owner.sourceCard.value;
	}

	public override void WriteNote(UINote n, bool identified)
	{
		base.WriteNote(n, identified);
		if (IsPlan)
		{
			return;
		}
		n.Space();
		foreach (Chara member in EClass.pc.party.members)
		{
			bool flag = member.elements.HasBase(idEle);
			n.AddText("_bullet".lang() + member.Name + " " + (flag ? "alreadyLearned" : "notLearned").lang(), flag ? FontColor.Good : FontColor.Warning);
		}
	}
}