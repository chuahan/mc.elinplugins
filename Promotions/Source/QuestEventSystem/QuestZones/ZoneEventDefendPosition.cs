using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PromotionMod.Common;
using PromotionMod.Source.QuestEventSystem.SpawnerConfigurations;
namespace PromotionMod.Source.QuestEventSystem.QuestZones;

public class ZoneEventDefendPosition : ZoneEventQuest
{

    [JsonProperty] public int EnemyPresence;
    [JsonProperty] public int Kills;

    // 5 Turns before Enemies arrive.
    [JsonProperty] public int Turns;

    public override int TimeLimit => 200;

    public SpawnSetup EnemyTypes => new FantasyBaddiesSetup();
    public ZoneInstanceRandomQuest instance => _zone.instance as ZoneInstanceRandomQuest;

    public override void OnVisit()
    {
        int difficulty = quest.FameContent ? pc.FameLv / 100 * 50 : 0;
        _zone._dangerLv = 5 + difficulty;

        if (game.isLoading) return;
        // Initiate the Defense Zone
        // Spawn the Defending Standard on the Spot.
        Chara banner = CharaGen.Create(Constants.DefenseBannerCharaId, _zone._dangerLv);
        banner.homeZone = _zone;
        banner.SetFaction(game.factions.Find(Constants.AdvGuildFactionId));
        Room targetRoom = _zone.map.rooms.listRoom.Single(r => r.Name.Equals("defenseArea", StringComparison.InvariantCultureIgnoreCase));
        Point defenseAreaCenter = targetRoom.GetRoomCenter();
        _zone.AddCard(banner, defenseAreaCenter);

        // Spawn Defenders around and leash them to the Defending Standard.
        List<string> defenders = new List<string>
        {
            Constants.WatcherFighterCharaId,
            Constants.WatcherArcherCharaId,
            Constants.WatcherHealerCharaId
        };

        // Create two of each in the Defending Area.
        foreach (string defenderId in defenders)
        {
            for (int i = 0; i < 2; i++)
            {
                Chara defender = CharaGen.Create(defenderId, _zone._dangerLv);
                banner.homeZone = _zone;
                banner.SetFaction(game.factions.Find(Constants.AdvGuildFactionId));
                _zone.AddCard(banner, defenseAreaCenter);
            }
        }
    }

    public override void _OnTickRound()
    {
        // base.quest.difficulty
        Turns++;

        // Check the Status of the Defense Area:
        Room targetRoom = _zone.map.rooms.listRoom.Single(r => r.Name.Equals("defenseArea", StringComparison.InvariantCultureIgnoreCase));
        EnemyPresence = 0;
        foreach (Point pos in targetRoom.points)
        {
            foreach (Chara presence in pos.Charas)
            {
                if (presence.IsHostile(pc) && presence.faction != game.factions.Find(Constants.AdvGuildFactionId))
                {
                    EnemyPresence++;
                }
            }
        }

        if (EnemyPresence > 5)
        {
            // TODO: Fail the quest.
            Kill();
        }

        // Spawn first wave at wave 5, allowing 5 turns to get set up.
        if (Turns == 5)
        {
            SpawnAttackers();
        }
        else if (Turns > 5)
        {
            if (Turns % 10 == 5 || Turns % 10 == 0)
            {
                // Every 5 turns spawn more enemies.
                SpawnAttackers();
            }

            if (Turns % 30 == 0)
            {
                // Every 30 turns spawn a boss.
                Rand.SetSeed(quest.uid);
                SpawnBoss(Turns > EClass.rnd(100));
                Rand.SetSeed();
            }
        }

        MoveNPCs();
    }

    public override ZoneInstance.Status OnReachTimeLimit()
    {
        Msg.Say("position_defended".lang());
        return ZoneInstance.Status.Success;
    }

    // Spawn the enemy at the far edge if possible.
    public void SpawnAttackers(int num = 1)
    {
        for (int index = 0; index < num; ++index)
        {
            Chara chara = _zone.SpawnMob(_map.bounds.GetRandomEdge().GetNearestPoint(allowChara: false), SpawnSetting.DefenseEnemy(_zone.DangerLv));
            chara.hostility = chara.c_originalHostility = Hostility.Enemy;
            if (CountEnemy)
                enemies.Add(chara.uid);
        }
    }

    public void SpawnAttackingCommander(bool evolve = false)
    {
        Point nearestPoint = _map.bounds.GetRandomEdge().GetNearestPoint(allowChara: false);
        Chara chara = evolve ? _zone.TryGenerateEvolved(true, nearestPoint) : _zone.SpawnMob(nearestPoint, SpawnSetting.Boss(_zone.DangerLv));
        chara.hostility = chara.c_originalHostility = Hostility.Enemy;
        if (CountEnemy)
            enemies.Add(chara.uid);
        if (!WarnBoss)
            return;
        Msg.Say("defense_boss", chara.Name);
        game.Pause();
    }

    public override void OnCharaDie(Chara c)
    {
        if (c.id == Constants.DefenseBannerCharaId) Kill();
    }

    public void MoveNPCs()
    {
        Chara bannerTarget = _zone.FindChara(Constants.DefenseBannerCharaId);
        // 50% chance to target PC faction, otherwise target the banner.
        foreach (Chara chara in _map.charas)
        {
            if (!chara.IsPCFactionOrMinion && !chara.IsInCombat)
            {
                // If it's an ally character, they should move towards the banner.
                if (chara.faction != game.factions.Find(Constants.AdvGuildFactionId))
                {
                    if (bannerTarget != null) chara.TryMoveTowards(bannerTarget.pos);
                }
                else
                {
                    // Move the enemies towards the Defense Banner or the PC party.
                    if (EClass.rnd(3) == 0 && bannerTarget != null)
                    {
                        chara.SetEnemy(bannerTarget);
                    }
                    else
                    {
                        chara.SetEnemy(pc.party.members.RandomItem());
                    }

                    chara.SetAIAggro();
                }

            }
        }
    }
}