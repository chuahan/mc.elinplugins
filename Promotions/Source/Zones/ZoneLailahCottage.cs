using System;
using System.IO;
using System.Linq;
using System.Reflection;
using PromotionMod.Common;
namespace PromotionMod.Source.Zones;

public class ZoneLailahCottage : Zone_Civilized
{
    public override bool ShouldRegenerate => true;

    public override bool HasLaw => true;

    public override bool AllowCriminal => true;

    public override bool RestrictBuild => true;

    public override bool isClaimable => false;
    
    public override bool IsTown => false;

    public override string IDAmbience => "forest";
    
    public override ZoneTransition.EnterState RegionEnterState => ZoneTransition.EnterState.Bottom;

    public override bool IsReturnLocation => true;
    
    public override void OnBeforeSimulate()
    {
        base.OnBeforeSimulate();
        AddLailah();
    }

    // 334 for Lailah's Cottage
    // 340 for Aluena
    
    public override string pathExport => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Maps", "lailahCottage.z");
    
    public void AddLailah()
    {
        if (EClass._map.charas.Find(c => c.id == Constants.LailahCharaId) != null)
        {
            return;
        }

        // If Lailah has joined the PC Faction.
        Chara? lailah = EClass.game.cards.globalCharas.Values.FirstOrDefault(gc => gc.id == Constants.LailahCharaId);
        if (lailah is { IsPCFaction: true }) return;
        lailah?.MoveZone(this);
        
        // Create Lailah and add her to her room.
        // Rooms are: CottageBedroom, CottageMain, CottageBathroom
        lailah = CharaGen.Create(Constants.LailahCharaId);
        if (lailah != null)
        {
            Room? targetRoom = this.map.rooms.listRoom.FirstOrDefault(r => r.Name.Equals("CottageMain", StringComparison.InvariantCultureIgnoreCase));
            Point lailahSpot = EClass._map.GetCenterPos().GetNearestPoint(allowBlock: false, allowChara: false);
            if (targetRoom != null)
            {
                lailahSpot = targetRoom.GetRandomFreePos();
            }
            EClass._zone.AddCard(lailah, lailahSpot);
            lailah.homeZone = this;
        };
    }
}