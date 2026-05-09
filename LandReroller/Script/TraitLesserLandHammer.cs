using System.Collections.Generic;
using System.Linq;

public class TraitLesserLandHammer : TraitItem
{
    public override string LangUse => "actLandSlam";

    public override bool OnUse(Chara c)
    {
        if (!EClass.pc.currentZone.isClaimable)
        {
            Msg.Say("landhammer_notclaimable".lang());
            return false;
        }

        if (EClass.pc.currentZone.landFeats.Count == 0)
        {
            Msg.SayNothingHappen();
            return false;
        }

        List<string> landFeats = EClass.pc.currentZone.landFeats.Select(x => EClass.sources.elements.GetRow(x.ToString()).GetName()).ToList();
        // Construct the string.
        string currentFeats = "";
        foreach (string featName in landFeats)
        {
            currentFeats += (featName) + "\n\r";
        }
        
        Dialog.YesNo(currentFeats + "rerollConfirmation".lang(), delegate
        {
            this.RerollLandfeats();
        });
        return false;
    }

    public void RerollLandfeats()
    {
        Zone editingZone = EClass.pc.currentZone;
        List<int> newLandFeats = new List<int>();
        string[] listBase = editingZone.IDBaseLandFeat.Split(',');
        string[] currentFeats = listBase;
        // Preserve the Biome.
        newLandFeats.Add(EClass.sources.elements.alias[currentFeats[0]].id);
        
        // Get a Mapping of Feat Id to Name
        Dictionary<int, string> landFeatIdNameMap = EClass.sources.elements.rows.Where(e => e.category == "landfeat").ToDictionary(row => row.id, row => row.GetName());
        
        // Determine the other feats. 
        List<SourceElement.Row> landFeatList = EClass.sources.elements.rows.Where(delegate(SourceElement.Row e)
        {
            if (e.category != "landfeat" || e.chance == 0)
            {
                return false;
            }
            bool notBFTag = true;
            foreach (string tag in e.tag)
            {
                if (tag.StartsWith("bf"))
                {
                    notBFTag = false;
                    if (listBase[0] == tag)
                    {
                        notBFTag = true;
                        break;
                    }
                }
            }
            return notBFTag ? true : false;
        }).ToList();
        
        SourceElement.Row row = landFeatList.RandomItemWeighted((SourceElement.Row e) => e.chance);
        newLandFeats.Add(row.id);
        landFeatList.Remove(row);
        row = landFeatList.RandomItemWeighted((SourceElement.Row e) => e.chance);
        newLandFeats.Add(row.id);
        
        editingZone.landFeats = newLandFeats;
        
        // Consume the Hammer and Report the changes.
        List<string> newLandFeatNames = new List<string>();
        foreach (int id in newLandFeats)
        {
            newLandFeatNames.Add(landFeatIdNameMap[id]);
        }
        Msg.Say("newLandFeats".lang(newLandFeatNames[0], newLandFeatNames[1], newLandFeatNames[2]));
        owner.ModNum(-1);
    }
}