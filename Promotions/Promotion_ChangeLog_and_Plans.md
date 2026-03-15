# Changelog and Roadmap

## Changelog
### March 11th, 2026 1.00 - Initial release
- Includes the base Promotions, Lailah, and the Quiet Cottage Map.

### March 11th, 2026 1.01 - Goddamnit Ascalon Patch
- Updated documentation to fix typoes and include more numbers for the number inclined.
- Reworked the Evaluation system for Promos to an extension method. Technically this means that Sub-classing shouldn't be that hard to implement, but not what I want to deal with.  
- Hermit: Opportunist and Shadow Shroud now apply the increased damage to spells as well. Spells unfortunately can't crit though.  
- Sentinel: Adds a cap to how much damage Rage can do cause Holy Shield Stacking Shenanigans. For now it has been capped at 200% damage and 1000 PV lost. Thanks Ascalon  
- Necromancer: Lord of the Dead Passive: Provides passive boosts to all allied undead (minions or charas.) Basically Hardware Upgrade for Undead instead of Machines.  
- Druid: Plant: Speaker for Nature Passive: Provides passive boosts to all allied undead (minions or charas.) Basically Hardware Upgrade for Plants instead of Machines.  
- Dread Knight - Healing recieved reduced by 75% with Life Ignition. Lifetaker is now true healing (does not get reduced.)  
- Berserker - Healing recieved from revel now is true healing (does not get reduced.)  
- Hexer - Fixed Paranoia.  
- Artificer - Fixed null ref exception on Quickcraft. Thanks Ruair.  

### March 12th, 2026 1.02 - Goddamnit Rotty and Ascalon Patch 2.
- Elementalist - Spell Tempo now also grants 2 Mana Consumption Reduction per stack, and the speed has been reduced down to 1% per stack.
- Added CN and JP Translations to Lailah's Letter, hopefully that fixes that bug.
- Fixes Jenei Feat Sheet crashing. Thanks Shu.
- Removed Invulnerability from some of the summons (Flowers and Holy Banner), ran into some issues with Blood Bond.
- Removed Vantage functionality while I figure out why it breaks things. Thanks 1z
- Added the missing throwing knife sprite. Thanks 1z

### March 14th, 2026 1.03 - Thanks Rombus Patch
- Refactored and fixed Dancer.
- Turns out I forgot to add ability costs for... everything. Added rudimentary costs for most abilities. Heavily subject to change.
- Tweaked the scaling of Protection across the board. Shouldn't be super inflated now. However, it's scaling like Light Healing, so will need to see if it remains relevant in the higher levels.
- Fixed a few bugs with War Cleric and Headhunter. Minimized cost to 1 SP for Headhunter abilities, even with Headhunter reduction.
- Added all the Condition Icons!
- Added missing Ability Icon for Headhunter: Reap

## To Do
- Need to add targetting highlights for... everything really.
- Need to add SFX and FX for a lot of stuff. I'll probably go through 1 by 1 and figure out what needs work.
- Balance things. Need user feedback.
- Move the Promotion Docs into the game as Hover. It's a bit much, but I will need to abbreviate it a bit.

## Potential Balance Changes
- Adventurer probably needs work.
- Headhunter needs work. Largely only passives, consider renaming them into Reaper and giving them the ability to equip and benefit from cursed equipment (2x stats from negatives? How much of a pain is this going to be to code...)
- War Cleric. Seems a little lackluster compared to Saint. Sanctuary is in dire need of special effects. Consider changing to Cell effect.
- Hermit. Seems too simple. Need to give them more abilities in their kit so they aren't a single target pony. Consider giving them expertise in ranged or throwing weapons, or the ability to transfer Marked for Death on kill or something.
- Sniper. Absolutely needs more work compared to Ranger. It's a passive class with... very little passive abilities, and Target X is pretty lackluster.
- Jenei. Attempt to implement a Class-Install system for the Jenei that will use their current spirits to boost their normal combat capabilities. This will boost their attributes and provide temporary feats allow them to basically pseudo-class change into another archetype. E.G. a healer class that gains healing instinct and reduces mana cost of healing abilities. However, this comes with the cost that if you use Spirit Summon and use your current spirits, you will also lose your pseudo-class.
- Dancer. Flourishes are way too strong. Weaken them somehow, or give them cooldowns.

## Future Content
- City of Aluena
- Information guild and Covert Ops Quests
- Adventurer's guild and Combat Ops Quests
- Advanced Combat Skills (Finish Implementing Galeforce)
- Add Promotion Mod enemies
- Minari storyline quests. (Sena and Ruras Recruit)
- Aluena Councilmember Saro
- Aluena Councilmember Cadem
- Vyers storyline quests. (Louise and Mitsune Recruit)
- Lailah storyline quests. (Lailah and AGS-L41 Recruit)
- City of Aluena: Government District
- Azalea's Teahouse
- Aluena Councilmember Wyndin
- Aluena Councilmember Arthur
- Aluena Councilmember Doren
- Aluena Councilmember Larissa
- Aluena Councilmember Sarvesh

### A New Threat
With class promotions unlocking levels of power for the players, it is only natural that the enemies will also gain a power boost to provide adequate challenge.
This mod adds a variety of new enemy types for you to face. 
Upon reaching specific Void Depth, enemies can also potentially be a promoted class. They can also randomly possess Advanced Combat Skills, so be careful!
Monsters that are promoted or possess combat skills with have a visual indicator of their increased strength, so tread with caution!

### Aluèna City
Aluèna is a large bustling city located to the west of the Quiet Cottage, south of the Little Garden. A thriving commercial Hub, this city houses two new guilds that provide unique services to help the player.
An incredibly diverse city rich with opportunity, it draws people from all walks of life. Aluèna possesses a sizeable military force, even having them work as the City Watch, protecting both the walls and the streets.
However, behind it's great walls and affluent Commercial and Government Districts, this city hides many secrets. The Slum area is a bleak reminder that the road to riches is pocked with pitfalls of greed and villiany.
The city is ruled by a high council of multiple nobles, whom all have something to hide. Keep your wits around you while exploring this city.

### The Adventurer's Guild
Aluèna possesses a branch of the Adventurer's Guild run by a retired soldier, Vyers. The Adventurer's Guild  is an organization that handles the bureaucratic and public facing elements of adventurer work.
Commonly known for handling the request boards, vetting the requestees, and connecting them with the adventurers themselves. The Adventurer's Guild was established to help foster new Adventurers to be able to safely handle and complete requests from the public. Their distinctive blue cloaks serve as a sign to anyone in need of assistance, that there there are people ready to help.
The Adventurers Guild provides the request handling services in Aluèna. More experienced adventurers are able to enjoy advanced training and learn powerful combat abilities.

### The Information Guild
The Drunken Crow is a small speakeasy in the Commercial District of Aluèna run by the Proprietress, Minari. Behind the bar she operates as the leader of the Crows, an Information Guild that spans the entire continent.
A single snowflake can cause an avalanche; Minari's agents pick up on small rumors, or fabricate them in order to monitor and manipulate the economic and political stage.
Customers who know what to order can keep up to date with events happening across North Tyris, and even intervene in them for great rewards.
In addition to being excellent information gatherers, agents of the Drunken Crow are also quite resourceful, taking a few pages from the Thieves Guild. Valued members of the Information Guild can make requests to find and purchase incredibly rare goods, at a premium of course.

### New Request Types
This mod adds a variety of new quests for players to undertake.

Combat Operations from the Adventurer's Guild
    - Defend position.
    - Capture position.
    - Rout the Enemy
    - Defeat Enemy Leader
    - Rescue allied forces.

Covert Operations from the Information Guild.
    - Delivering Letters or Packages.
    - Gathering Reports from field agents.
    - Abduction Requests
    - Eliminate hidden agents.