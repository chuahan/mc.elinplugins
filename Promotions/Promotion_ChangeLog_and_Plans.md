# Changelog and Roadmap

## Changelog

### April 19th, 2026, 1.10 - Refactoring.
- Overhauled the Ability System codewise to reduce a lot of duplicate code.
- Lailah will now automatically progress deciphering on her own, gaining 1% progression a day. The amount of progression granted for giving her Ancient books has been doubled (1%->2%). This should reduce the grind on her deciphering.

#### Promotion Ability Reclassification
Promotion Abilities have been divided into 3 Categories:
- Combat Abilities are now affected by Suppress/Disable. I am considering providing some scaling support from Combat Arts feat onto Combat Abilities, this will likely need to be done with a case by case scenario though cause combat abilities usually don't scale off power.
- Spell Abilities are now affected by Silence and usually cost Mana. They also will scale with Spell Enhance and Antimagic.
- Generic Abilities are unaffected by either, usually reserved for things like Stances.
Summarized changes are as follows:
- Artificer abilities are all Spell Abilities.
- Berserker's Sunder and Lifebreak have all been reclassified as Combat Abilities. Bloodlust has been classified as a Generic Abilitiy.
- Dancer's Offensive Steps (Illusion, Fouette, and Pirouette) are all considered Combat Abilities and all have now have a cooldown now of 5 seconds. Sword Fouette and Dagger Illusion now cost a flat 5 Stamina. Wild Pirouette will continue to use Mana.
- Dread Knight's Dark Burst and Barrier are classified as Spell Abilities. Mana Starter is a Combat Ability. Life Ignition Stance is Generic.
- Druid abilities are all now Spell Abilities.
- Elementalist abilities are all now Spell Abilities.
- Gambler's Card Throw and Dice Strike are now Combat Abilities. Lucky Cat and Spin Slots are both Spell Abilities. Feeling Lucky is Generic.
- Harbinger's Accursed Touch and Endleess Mists are Combat Abilities. Gloom remains Generic.
- Headhunter abilities are all Combat Abilities.
- Hermit abilites are all Combat Abilities.
- Hexer abilities are all Spell Abilities.
- Holy Knight abilities are all Combat Abilities.
- Jenei abilities are all Spell Abilities (except for Venus: Move, that one is Generic cause it's a non combat ability.)
- Justicar abilities are all Combat Abilities save for Flames of Judgement which is Generic.
- Knightcaller abilities are all Spell Abilities.
- Machinist abilities are all Combat Abilities except for Summon Turret. Loadup and Overclock recieved 10 turn cooldowns.
- Necromancer abilities are all Spell Abilities.
- Ranger abilities are Combat Abilities that cost Mana. Debating adding a Charge System to Throw Trap so that you can throw X without cooldown.
- Rune Knight abilities are now Spell Abilities.
- Saint abilities are Spell Abilities.
- Sentinel abilities are Combat Abilities while Rage/Restraint remain Generic. They will continue to use Mana instead of Stamina.
- Sharpshooter abilities are Combat Abilities.
- Sniper abilities are Combat Abilities.
- Sovereign abilities have been changed to Spell Abilities.
- Spellblade abilities are Combat Abilities that use Mana.
- Trickster abilities are Spell Abilities.
- War Cleric's Blessed Armament and Divine Sanctuary are Spell Abilities. Divine Fist and Descent are both considered Combat Abilities.
- Witch Hunter's abilities are Combat Abilities that use Mana. Null Zone can now be casted again to deactivate it.

### April 17th, 2026, 1.09 - Kolinca Patch
- A various bugfixes.
- Fixed Faction NPC summons now to have the same scaling as the player (Can use your maximum depth reached instead.
- Battlemage will now summon 1 normal funnel and 1 shield funnel in conjunction instead of 2 shield funnels.
- Hermit now loses Shadow Shroud when they are detected.
- Fixed Hidden Throwing weapons not tagged as throwing weapons.
- Fixed some grammatical errors in some of the Artificer Stuff. Shiba.
- Minor hiccup with Artificer requiring Mithril/Platinum typed ingots. Thanks Fresh for updating CWL to handle material name instead of just tags.
- Nerfed Saint. They were too charismatic and persuasive, basically converting any enemy they encountered under the same faith. Made it a lot harder for this to happen.
- Saint Blessing now properly updates the Player.
- Saint Blessing can no longer recursively power itself up by spamming Blessing to increase Faith to extraordinary high levels.

### April 13th, 2026, 1.08 - Changes
- HP Costing spells can no longer be redirected with Blood Bond or Wall of Flesh. These will modify your HP directly. This includes Hexer's Blood Curse, Berserker's Bloodlust, Dread Knight's Dark Burst and Mana Starter. Thanks 오징어.
- I've updated it so that Jenei and Elementalist conditions will now expire if they have 0 orbs. Thanks Kolinca.
- Fixed Faction Traps causing Proc errors cause cards can't apply debuffs. Thanks Kolinca.
- Evie's story is under development.
- I got slightly sidetracked by Spirit Weapons.

### April 6th, 2026, 1.07 - Bugfixes.
Sorry for the lack of updates, I've been working on the city update and the first class-less promotion. Pushing this patch out because 오징어 has informed me of a bug with CostType None preventing some abilities from leveling up. I've gone through and reworked the affected abilities, they should be able to level now!  
- Added sprites for the new Hermit and Sniper abilities, Preparation and Tactical Retreat, and the new Sniper Condition: Vigilance.  
- The next update will likely include the city of Aluèna and the NPCs involved. However, the coding for the Adventurers Guild and the Information Guild is not quite complete, so those buildings will remain vacant for now.  
- The Gambler Promotion will need a bit more cooking, but feel free to preview it's abilities below!

### March 26th, 2026 1.06 - Bugfixes
- Fixed a few issues with Elementalist. Thanks callomerlose.
- Fixed an issue with Sword Sages not being able to promote. Thanks Tesshu.

### March 22th, 2026 1.05 - Justicar, Hermit, and Sniper.
- Crit Boost now increases Critical Damage as well as Critical Chance.
- Fixed Headhunter condition accidentally reducing your damage dealt by a lot. Thanks Lickiwhirl.
- Fixed NRE issue with Undead characters (and presumably plant characters) in the character creation. Thanks Zeltarel.
- Some abilities (Hermit, Dancer) have been converted to use MP instead of SP.
- Fixed NRE issue with firing at empty tiles. Thanks Xaytan and Ruair.
- Fixed quite a few issues with Conditions not ticking.

#### Justicar
- Justicar Flames of Judgement now has special effects, complete with varying whether you are positive or negative karma.
- Justicar's Intimidate will apply Armor Boost on nearby allies if you are positive karma. If you are negative karma, it will do additional Sound damage.
- Justicar's Condemn will apply Crit Boost to nearby allies if you are postive karma. If you are negative karma, it can inflict burning.
- Justicar's Subdue will inflict the Mana Leak Debuff that will cause attacks to steal MP from the target if you are positive karma. IF you are negative karma, you will purge one active buff on the target.

#### Hermit
- New Active Ability: Preparation. This ability can only be used on targets that have been Marked for Death. Targetted debuff that also grants the Hermit Crit Boost, while inflicting one of: Sleep, Poison, Paralyze, Bleed, or Faint. The chance of inflicting the debuff increases depending on how high the Stalk value of Mark for Death is.
- Hermits Opportunist now also works with Poison and Bleed.

#### Sniper
- New Active Ability: Tactical Retreat - Can only be used in melee range. Move away from the target and make a ranged attack upon landing.
- New Passive: Reactive Shot - Snipers can make a reactive shot against any incoming ranged attacks, even if it misses. Upon making this reactive shot, they will gain one tick of Vigilance. While they have Vigilance they cannot make further reactive shots limiting you to one a tick.

### March 16th, 2026 1.04 - Minions Build
- Necromancers and Druids now give a passive buff to undead and plant allies respectively. 10% Boost to PV and DV and 20% Boost to Speed of their respective ally type. Note: This technically applies to themselves if you happen to be playing a Lich/Wraith/Alraune. Right now, this effect scales with how many of each class are on the field. E.G. if you have 3 Necromancers on the same team, all Undead Types will gain 30% boost to PV/DV and 60% Speed Boost. This also applies to the enemies (if there is an enemy necromancer on the field.) This effect caps at 10 stacks, so 100% increased PV and DV and 200% increased speed. This is subject to change, but you can very much do a mass bonelord party for funsies right now.
- Fixed the Knightcaller so the Knight Captains are generated with their promotion flag set.
- Buffed Shield Funnels to have 100 extra PV and 50% Physical Damage Reduction in an effort to offset the squishiness that is applied by Wall of Flesh. This does not impact their elemental defenses though, making them remain vulnerable to magic.
- Fixed Lifetaker and Berserker on-kill healing to only work on hostiles. No more using your allies as health potions, this ain't Mad Max.
- Fixed a lot of missing hints and hover text.

### March 14th, 2026 1.03 - Thanks Rombus Patch
- Refactored and fixed Dancer.
- Turns out I forgot to add ability costs for... everything. Added rudimentary costs for most abilities. Heavily subject to change.
- Tweaked the scaling of Protection across the board. Shouldn't be super inflated now. However, it's scaling like Light Healing, so will need to see if it remains relevant in the higher levels.
- Fixed a few bugs with War Cleric and Headhunter. Minimized cost to 1 SP for Headhunter abilities, even with Headhunter reduction.
- Added all the Condition Icons!
- Added missing Ability Icon for Headhunter: Reap
- Added missing Ability Icons for Dread Knight.

### March 12th, 2026 1.02 - Goddamnit Rotty and Ascalon Patch 2.
- Elementalist - Spell Tempo now also grants 2 Mana Consumption Reduction per stack, and the speed has been reduced down to 1% per stack.
- Added CN and JP Translations to Lailah's Letter, hopefully that fixes that bug.
- Fixes Jenei Feat Sheet crashing. Thanks Shu.
- Removed Invulnerability from some of the summons (Flowers and Holy Banner), ran into some issues with Blood Bond.
- Removed Vantage functionality while I figure out why it breaks things. Thanks 1z
- Added the missing throwing knife sprite. Thanks 1z

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

### March 11th, 2026 1.00 - Initial release
- Includes the base Promotions, Lailah, and the Quiet Cottage Map.

## To Do
- Need to add targetting highlights for... everything really.
- Need to add SFX and FX for a lot of stuff. I'll probably go through 1 by 1 and figure out what needs work.
- Balance things. Need user feedback.
- Move the Promotion Docs into the game as Hover. It's a bit much, but I will need to abbreviate it a bit.
- Add JP translations (Waiting till I got all the abilities tuned and ironed out.)

## Potential Balance Changes

### Adventurer
Probably needs work. Mostly passives, but I need to consider new abilities for them.  
Idea: Mealprep: To all food in the inventory, Adventurers can refresh the stale-ness and also add "Just Cooked" to it. Might be too powerful?  
Idea: Appraise: Basically a free Greater Identify with a cooldown.  
Idea: Treasure Finder Passive: When completing a Nefia (Boss dead), Adventurers have a chance of unlocking a "bonus floor" which is basically a Hoard of loot with enemy spawners and extra bosses. With great danger comes great reward?  

### Headhunter
Largely only passives, consider renaming them into Reaper and giving them the ability to equip and benefit from cursed equipment (2x stats from negatives? How much of a pain is this going to be to code...) Then I can work them around kills + curses.

### War Cleric
Seems a little lackluster compared to Saint.  
Sanctuary is in dire need of special effects. Consider changing to Cell effect.  
The other idea is to make them more of a War Monk, so make them a Martial based class. Maybe Shaolin style with crit strikes, knockbacks, and reinforcing their own body?

### Hermit
Seems too simple. Need to give them more abilities in their kit so they aren't a single target pony.
Consider giving them expertise in ranged or throwing weapons, or the ability to transfer Marked for Death on kill or something.
Give them some method of applying conditions to synergize with. Posioned weapons or something. Or throwing hidden weapon laced with venoms.

### Sniper
Passive Ability Idea: Does more damage with consecutive shots against the same target, stacking debuffs?

### Ranger
Consider also giving them Reactive Shots (Retaliate against ranged attacks with ranged attacks.)

### Jenei
Attempt to implement a Class-Install system for the Jenei that will use their current spirits to boost their normal combat capabilities. This will boost their attributes and provide temporary feats allow them to basically pseudo-class change into another archetype. E.G. a healer class that gains healing instinct and reduces mana cost of healing abilities. However, this comes with the cost that if you use Spirit Summon and use your current spirits, you will also lose your pseudo-class.

### Dancer
Flourishes are way too strong. Weaken them somehow, or give them cooldowns.
Dances themselves are too weak. Give them buffs. Give them more functionality or effects, and work on their scaling a bit more.

### Druid
Considering making Wrath of Nature a Berserker and Warmth of Nature a Saint.

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

### Gambler
Evie is a wandering bounty hunter who goes by the moniker Lucky Shot. She is known equally for her gunslinging skill as her penchant for gambling.  
If you run into this cheerful girl, she might be willing to teach you a couple tricks in the right circumstance. Evie wields the Fomalhaut hand cannon with deadly accuracy.

Gamblers are a luck based promotion class that lives by the dice. Their damage is highly variant and their abilities are whimsical, ranging from hitting extremely hard to potentially benefiting the target instead. It is important to note that all of their abilities are governed by Dice Rolls, and thus are impacted by the Gambler's Luck stat.

__I'm Feeling Lucky - Stance that modifies how much damage and healing you deal and how much damage / healing you recieve.__  
Every time you recieve or deal damage you make a Dice roll of 20 then subtract 10.
For damage dealt, your damage is multiplied by (10 + x) / 10, meaning a -10 would result in you dealing no damage, while a 20 will result in you dealing an additional 2x damage.
For damage taken, incoming damage is multiplied by (10 - x) / 10, meaning a -10 would result in 2x damage taken while a 20 will result in you taking no damage.

__Slots - Self Targeted Casting Ability - Fate is Whimsical.__  
The Gambler spins the slots, gaining the "Slots" condition. After one turn elapses, the slots will finalize and the effect will activate. This can be beneficial or harmful, for either the Gambler and allies or their targets.

__First Wheel - Gods - Yevan (Physical) / Itzpalt (Magical) / Jure (Buff/Debuff) / Kizuami (Random) - Ehekatl (Jackpot/Random)__
- Yevan enters the Attack Slots.
- Itzpalt enters to Magic Slots.
- Jure enters the Support Slots.
- Kizuami Picks one of the first three at random.
- Ehekatl Picks one of the first three at random if not Jackpot.

__Second Wheel - Elements - Anima / Holy / Dark / Void / Jackpot__

Yevan
- Anima - Strikes with Magic Sword attacks.
- Light - Strikes with Holy Sword attacks.
- Dark - Strikes with Dark Sword attacks.
- Sword - Performs Bladestorm.
- Jackpot - Strikes with a Melee attack that will cut the target's HP in half.

Itzpalt
- Anima - Strikes with one of Fire/Cold/Lightning Arrow.
- Light - Strikes with Holy Ball.
- Dark - Strikes with Dark Bolt
- Sword - Strikes with random magical sword of Fire/Cold/Lightning
- Jackpot - Drops a Meteor of Fire/Cold/Lightning on the target.

Jure
- Anima - Restores HP to the target.
- Light - Grants Holy veil to the target.
- Dark - Inflicts Bane on the target.
- Sword - Inflicts Weakness on the target.
- Jackpot - Fully Heals the target.

Kizuami - Random from the other options.  
Ehekatl - Random from the other options if not Jackpot.

__Third Wheel - Targetting - Bar / Cherry / Bell / Skull / 7__
- Bar - Targets all nearby enemies within 5F.
- Cherry - Targets a random single enemy within 5F.
- Bell - Targets all allies with a positive effect and enemies with a negative effect.
- Skill - Targets all allies within 5F.
- 7 - If not Jackpot, picks any but Skull.

__Jackpot - If the Gambler rolls Ehekatl/Jackpot/7, a fourth wheel is spun to determine the effect.__

- Little Sister - Fully HP/MP/Stamina of the Gambler's Allies.
- Grim Reaper - All nearby enemies take a massive critical Melee damage strike, with a 2.5x multiplier.
- Zantetsuken - A random nearby enemy is struck with a grevious attack with a 1/7 chance to instant kill. If the instant kill does not trigger, a 12 sided dice is rolled up with the results multiplied by 7. That number is used as the percent life removed from the target, so rolling a 12 will result in the target losing 84% of their life.
- Full Throttle - Allies are buffed with Haste/Hero/Elemental Shield/Greater Regeneration and 1 turn of invulnerability. Enemies nearby are hit with Slow/Bane/Weakness/EleScar/Nightmare
- Jackpot - All enemies nearby will drop their entire inventories and disappear.

__Blackjack Throw - Single Target Ranged Combat Ability - Draws cards to perform a throwing attack. Creates a card and modifies it's weight based on the number.__  
Use the dealers rules, hit when below 16 and stand when 17 or better.  
17 to 21 Will result in an enhanced throwing attack being made with guaranteed accuracy.  
Blackjack - Results in two throwing attacks executed with full Vorpal.  
Bust - The attack fails.  
5 Card Charlie - Throws 5 basic throwing attacks in a single action.  

__Dice Roll - Rolls a pair of dice to perform a melee attack.__  
SnakeEyes - Heals the target for the damage you would have dealt.
Between 3 and 11 does a melee hit with a damage multiplier of 1 + ((x - 7) * .1). So a 3 would cause you to deal 60% damage while an 11 will perform a melee attack with 140% damage.
Twelve - Reaper's Scythe - Does Double Damage.

__Lucky Cat - Buff that increases luck. Can be Partycast.__