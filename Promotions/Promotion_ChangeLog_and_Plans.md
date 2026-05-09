# Changelog and Roadmap

## Changelog

### April 26th, 2026, 1.101 - Mini patch.
- Fixed Alraune feat adding/removing abilties.
- Alraune now is "complete" and is entering feedback phase. Their 3rd Ability has been added: Consume Prey. Alraunes can now consume targets that fit the conditions and eat them, gaining attributes and skills based on the victim they consume. While they cannot gain abilities or spells like Slime can, they absorb far more attributes. If you are already an Alraune, don't worry, if you go in game and save, the new abilities will be added automatically.
- Fixed Artificer Golems not being able to talk (partially, still working on the specific golem type documentation as opposed to dumping all of it at once.) Thanks 오징어
- Fixed Necromancer's Corpse Explosion and Blessing of the Dead, they were using ints instead of floats which caused it to basically always do absolutely nothing. Thanks 오징어

### April 24th, 2026, 1.10 - Refactoring.
- Overhauled the Ability System codewise to reduce a lot of duplicate code.
- Lailah will now automatically progress deciphering on her own once you give her at least one book, gaining 1% progression a day. The amount of progression granted for giving her Ancient books has been doubled (1%->2%). This should reduce the grind on her deciphering.
- A lot of classes were missing some descriptions and whatnot, so I've been combing through adding more information where necessary.
- Effects that spawn multiple Alternative Funnels in one action will no longer spawn them past the minion cap of the caster.
- Fixed a bug where Rune Knight could attune their guard to Void Damage.

#### Promotion Ability Reclassification
Promotion Abilities have been divided into 3 Categories:
- Combat Abilities are now affected by Suppress/Disable. I am considering providing some scaling support from Combat Arts feat onto Combat Abilities, this will likely need to be done with a case by case scenario though cause combat abilities usually don't scale off power.
- Spell Abilities are now affected by Silence as well as Null Presence, will cost Mana. They also will scale with Spell Enhance and Antimagic.
- Generic Abilities are unaffected by either, usually reserved for things like Stances.
Abilities that are not really impacted by spellpower will not increase their resource costs.

Reclassification changes are as follows:
- Artificer abilities are all Spell Abilities.
- Berserker's Sunder and Lifebreak have all been reclassified as Combat Abilities. Bloodlust has been classified as a Generic Abilitiy.
- Dancer's Offensive Steps (Illusion, Fouette, and Pirouette) are all considered Combat Abilities, but Wild Pirouette will continue to use Mana.
- Dread Knight's Dark Burst and Barrier are classified as Spell Abilities. Mana Starter is a Combat Ability. Life Ignition Stance is Generic.
- Druid abilities are all now Spell Abilities.
- Elementalist abilities are all now Spell Abilities.
- Gambler's Card Throw and Dice Strike are now Combat Abilities. Lucky Cat and Spin Slots are both Spell Abilities. Feeling Lucky is Generic.
- Harbinger's Accursed Touch and Endleess Mists are Combat Abilities. Gloom remains Generic.
- Headhunter abilities are all Combat Abilities.
- Hermit abilites are all Combat Abilities that use Mana.
- Hexer abilities are all Spell Abilities.
- Holy Knight abilities are all Combat Abilities, with summon Banner being Mana consuming.
- Jenei abilities are all Spell Abilities (except for Venus: Move, that one is Generic cause it's a non combat ability.)
- Justicar abilities are all Combat Abilities save for Flames of Judgement which is Generic.
- Knightcaller abilities are all Spell Abilities.
- Machinist abilities are all Combat Abilities except for Summon Turret. Loadup and Overclock recieved 10 turn cooldowns.
- Necromancer abilities are all Spell Abilities.
- Ranger abilities are Combat Abilities that cost Mana. Debating adding a Charge System to Throw Trap so that you can throw X without cooldown.
- Rune Knight abilities are now Spell Abilities.
- Saint abilities are now Spell Abilities. However, Blessing and Invigorate are unique cases that scale off Faith and do not benefit from Spell Enhance.
- Sentinel abilities are Combat Abilities while Rage/Restraint remain Generic. They will continue to use Mana instead of Stamina.
- Sharpshooter abilities are Combat Abilities that use Mana.
- Sniper abilities are Combat Abilities that use Mana.
- Sovereign abilities have been changed to Spell Abilities.
- Spellblade abilities are Combat Abilities that use Mana.
- Trickster abilities are Spell Abilities.
- War Cleric's Blessed Armament and Divine Sanctuary are Spell Abilities. Divine Fist and Descent are both considered Combat Abilities.
- Witch Hunter's abilities are Combat Abilities that use Mana. Null Zone can now be casted again to deactivate it.

Additional Changes
- Dancer's Offensive Steps (Illusion, Fouette, and Pirouette) all have now have a cooldown now of 5 seconds. Sword Fouette and Dagger Illusion now cost a flat 5 Stamina. Dancer's dances will be disabled if the dancer is restrained or disabled.
- Elementalist will now gain scaling exp based on # of orbs consumed for Elemental Fury and Extinction.
- Fixed Saint's Hand of God not... actually healing the correct targets.
- Sharpshooter's Overwatch "Under Fire" application range has been increased from 6 to 11, allowing it to mostly encompass their increased sight range.
- Sharpshooter's MarkHostiles now has SFX.
- War Cleric Divine Descent cooldown reduced from 1 Day to 12 Hours (720), this was largely because Elin doesn't like it when you go over 1000 in cooldown. It ends up adding the cooldown to the NEXT ability Id, which was why Divine Descent was adding cooldown for Divine Fist.
- Witch Hunter's Null Zone now activates it's cooldown on cancellation.

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

### Sniper
Passive Ability Idea: Does more damage with consecutive shots against the same target, stacking debuffs?

### Ranger
Consider also giving them Reactive Shots (Retaliate against ranged attacks with ranged attacks, currently held by Sniper.)

### Jenei
Attempt to implement a Class-Install system for the Jenei that will use their current spirits to boost their normal combat capabilities. This will boost their attributes and provide temporary feats allow them to basically pseudo-class change into another archetype. E.G. a healer class that gains healing instinct and reduces mana cost of healing abilities. However, this comes with the cost that if you use Spirit Summon and use your current spirits, you will also lose your pseudo-class.

### Dancer
Dances themselves are too weak. Give them buffs. Give them more functionality or effects, and work on their scaling a bit more.

## Future Content
- City of Aluèna
- Information guild and Covert Ops Quests
- Adventurer's guild and Combat Ops Quests
- Advanced Combat Skills (Finish Implementing Galeforce)
- Add Promotion Mod enemies
- Minari storyline quests. (Sena and Ruras Recruit)
- Aluèna Councilmember Saro
- Aluèna Councilmember Cadem
- Vyers storyline quests. (Louise and Mitsune Recruit)
- Lailah storyline quests. (Lailah and AGS-L41 Recruit)
- City of Aluèna: Government District
- Azalea's Teahouse
- Aluèna Councilmember Wyndin
- Aluèna Councilmember Arthur
- Aluèna Councilmember Doren
- Aluèna Councilmember Larissa
- Aluèna Councilmember Sarvesh

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

### Alraune
A forest-dwelling plant monster with the upper body of a beautiful human, appearing to rise from an enormous, bulbous flower. It releases a sweet fragrance to entice living creatures, drawing them close before ensnaring and consuming them.

Alraunes are slow but durable creatures. They have a low speed score, high Charisma, and are magic leaning. They are innately immune to Poison and Acid, but extremely weak to Fire and moderately weak to Cold. Using their vines they can mimic an additional pair of Hands, but do not have Waist, Leg, or Feet equipment spots. Their unique plant constitution prevents them from benefiting as much from normal foods like other races, instead depending on consuming other creatures to satisfy their nutritional needs.

Alraune Racial Trait: Floral Constitution.
When exposed to the sunless winter air, they will gain fatigue twice as fast as other races. This effect is avoided if in sunlight or if the Alraune possesses enough cold resistance.
When exposed to sunlight, they will automatically self-cast Nature's Embrace on themselves to boost their own regeneration.
When they are exposed to sunlight and are also wet, they will digest food faster, gaining hunger at 3 times the normal rate.
Alraunes will suffer a 70% penalty to nutrition, greatly decreasing the amount of gains they can recieve from normal food.

Racial Ability: Sweet Scent (Aura)
Reserve 10% of your mana to activate your Sweet Scent Aura, attempting to apply Infatuation and Drunk to nearby hostiles.

Racial Ability: Wild Growth
AOE Debuff Ability that grabs nearby enemies within 3 tiles with poisonous vines. While this ability does not do damaage, it can inflict Poison and Entangle.

Racial Ability: Consume Prey.
Usable on non boss, non unique enemies that are at or below 10% HP and afflicted with Infatuation. The Alraune must also be hungry.
If the conditions are met, the Alraune will grab the enemy and pull them into their flower, consuming them.
Any of the victim's possessions will be spat out with extra rust if not acid proof.
The Alraune will gain a large amount of attributes, skills, and Feat Points from this act (Similar calculations to feeding a Baby Character milk.)
The Alraune will also gain a unique Punishment Debuff called "Digesting Prey" which reduces their speed by 50% and can only wear off over time. The duration of this debuff is based off of the overlevel of the victim consumed in respect to the Alraune's own level. While under the affect of this Debuff, any hunger the Alraune would normally gain will be instead set to help reduce the duration of this debuff, and they cannot perform Consume Prey again.

### Gambler
Evie is an adventurer who goes by the moniker Lucky Shot. She is known equally for her gunslinging skill as her penchant for gambling.  
Evie wields the Fomalhaut hand cannon with deadly accuracy, specializing in hunting down bounty targets. If you run into her and complete her questline, she will unlock the Gambler Promotion for your run, allowing ANY class to advance into this lucky Promotion.  
  
The Gambler is a luck based offshoot Promotion. Their abilities possess extreme randomness, making them capable of changing the tides of battle in either direction in a blink of an eye. Gamblers heavily depend on their Luck attribute, using it to increase the odds of their abilities producing positive results.

### WIP, 1.2 - Aluèna
- Welcome to Aluèna adventurers!

#### Questlines
- Lailah: Betrayed Soldier
- Minari: Regicide à Deux
- Vyers: War against the Dark guild
- Evie: Gomidor's Bounty
- Larissa: Rivalry with Sarvesh

#### Non Recruitable NPCs.
- Grandmeow Kari is runs the Warm Hearth Cafeteria.
- Alder runs the Moonlit Boutique Tailor.
- Ùshrir runs the Summer Mountain Forge.
- Camellia runs the Viridian Grove Teahouse.
- Vyers is the leader of the Adventurer's Guild.
- Louise is the Secretary of the Adventurer's Guild.
- Minari is the leader of the Information Guild.

#### Recruitable NPCs
- Evie is a recruitable Catsister Gambler, location varies.
- Vessia is a recruitable Mifu Dancer that works at the Moonlit Boutique.
- Azalea is a recruitable Alraune Druid that works at the Viridian Grove Teahouse.
- Mitsune is a recruitable Nefu Dread Knight that works at the Adventurer's Guild.
- Sena and Ruras are recruitable Juere Hermits that works at the Information Guild.