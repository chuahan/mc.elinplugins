namespace PromotionMod.Trait.Artificer;

/// <summary>
///     Golems are unique customizable allies usable by the Artificer.
///     Artificers can only ever have one golem deployed.
///     If they choose to do so, they can destroy the golem and recoup some resources.
///     To craft an "Inactive Golem", Artificers must create a Core, Frame, and Precept.
///     The Core is built by itself and is an expensive piece. (Do I want the hardness + weight to do anything?)
///     The Frame must be built before assembling. The Frame dictates the base "racial" traits and body parts of the golem.
///     All Frames Require a Golem Head Unit, then different pieces depending on the Frame Type.
///     Artificers can pick between the following Frames:
///     Mim Type Golems are simple humanoid Golems, simplicity in versatility.
///     1 Arm, 1 Leg.
///     Gains double the benefit from Expansion Chips.
///     Harpy Type Golems are flying golems, similar to the fairy race. Lower durability, but high speed and evasion,
///     innate perfect evasion, as well as innate flight.
///     1 Arm, 1 Leg. 1 Flight Module.
///     Have access to the "Eye in the Sky" Feat, which when in not inside, will increase the player's FOV by 2.
///     Have Crab Grip.
///     Siren Type Golems are aquatic golems inspired by the nereids. Lacking Legs, they are far slower normally, but will
///     show their potential when levitating or in water.
///     1 Arm. 1 Aquatic Module.
///     When levitating or in water, they gain "Adaptive Mobility" condition, which gives them a boost to their speed and
///     evasion.
///     When in water, they will gain the "Aquatic Cooling" condition, which gives them a boost to rapid magic and spell
///     enhance.
///     Titan Type Golems are massive humanoid golems. It differs from the other models as a model designed to protect the
///     rider. They are incredibly durable but slow, and are good to ride.
///     1 Arm, 1 Leg. 1 Pilot Module.
///     Has the "Core Protocols" Feat. When being ridden, the Titan golem gains increased stats, while redirecting 100% of
///     the damage done to the rider to itself.
///     When being ridden, Pilot gets access to the "Trample" ability, which is a bodyslam/rush attack that scales
///     based on the speed and END of the titan.
///     "Welcome back, pilot."
///     "Control transferring to pilot."
///     "Transferring systems to manual operations."
///     "Pilot onboard, welcome back."
///     "My systems are yours, we are more powerful as a team."
///     "Glad to have you back, pilot, we are better together."
///     "Switching to manual, all systems nominal."
///     The Precept must be built before assembling. The Precept dictates the base "class" traits and abilities of the
///     golem.
///     Golems are always Tourists for the allowance of Promoting into any class.
///     Artificers can pick between the following precepts:
///     Vanguard Class - A melee focused class.
///     Tourist / Tank
///     Starts with Rush, Taunt. Tactics. Evasion. Strategy.
///     Tower Class - A magical focused class.
///     Tourist / Wizard
///     Starts with Magic Arrow, Bolt, and Flare, Casting, Mana Capacity, and Mana Control.
///     Siege Class - A ranged focused class.
///     Tourist / Archer
///     Starts with Missile Barrage, Marksmanship. Evasion. Eye of Mind.
///     After construction, the golem can also be upgraded with Gene Engineering like normal allies and Upgrade Chips.
///     Adaptation Expansion Chips can be used to add FeatGolemUpgrade to a golem, which will increase their Gene Slots by
///     1.
///     Power Expansion Chips can be used to add Feat Points to a golem.
///     All Artificer Golems are Machine Type, and hence can be upgraded via Mani Worship.
///     All Artificer Golem basic attributes are the same as the base golem.
/// </summary>
public class TraitArtificerGolem : TraitUniqueChara
{
    public override bool CanInvite => true;
    public override bool CanJoinParty => true;
    public override bool CanJoinPartyResident => true;
}