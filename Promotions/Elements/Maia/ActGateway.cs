using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Maia;
namespace PromotionMod.Elements.Maia;

/// <summary>
/// PC Only
/// When Enlightened: Immediate Evacuation.
/// When Corrupted: Immediate Return to bound home Location, but leaves a portal in the departure location, allowing return.
///     This portal disappears if the zone despawns, or if the spell is cast again. 
/// </summary>
public class ActGateway : Ability
{
    public override bool CanPerform()
    {
        // Ability is only usable by ascended Maia.
        if (CC.race.id != "maia" || (CC.Evalue(Constants.FeatMaiaCorrupted) == 0 && CC.Evalue(Constants.FeatMaiaEnlightened) == 0)) return false;

        // Not usable by NPCs
        if (!CC.IsPC) return false;

        // Can't be used in world map.
        if (_zone.IsRegion) return false;
        
        // Can't be used in the home zone.
        if (_zone == EClass.pc.homeZone) return false;
        
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // We assume at this point since we got past CanPerform that the Maia is ascended.
        bool isCorrupted = CC.Evalue(Constants.FeatMaiaCorrupted) > 0;
        
        if (isCorrupted)
        {
            ConGateway gateway = CC.GetCondition<ConGateway>();
            if (gateway != null)
            {
                // Check if the zone is still valid.
                Zone checkZone = EClass.game.spatials.Find(gateway.DestinationUid);
                if (checkZone != null)
                {
                    EClass.player.returnInfo = new Player.ReturnInfo
                    {
                        turns = 1,
                        isEvac = true,
                        uidDest = gateway.DestinationUid,
                    };
                    Msg.Say("maia_gateway_returning".langGame());
                    return true;
                }
                else
                {
                    Msg.Say("maia_gateway_destination_null".langGame());
                    gateway.Kill();
                }
            }
            
            Msg.Say("maia_gateway_escaping".langGame());
            // Store the location of the current zone.
            gateway = CC.AddCondition<ConGateway>() as ConGateway;
            gateway.DestinationUid = _zone.uid;
            EClass.player.returnInfo = new Player.ReturnInfo
            {
                turns = 1,
                isEvac = true,
                uidDest = EClass.pc.homeZone.uid
            };
        }
        else
        {
            Msg.Say("returnBegin");
            EClass.player.returnInfo = new Player.ReturnInfo
            {
                turns = 1,
                isEvac = true
            };
        }

        return true;
    }
}