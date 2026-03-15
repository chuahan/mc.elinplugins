using Newtonsoft.Json;
namespace PromotionMod.Source.QuestEventSystem.QuestZones;

public class ZoneEventDefendPosition : ZoneEventQuest
{
    [JsonProperty] public int Kills;

    // 5 Turns before Enemies arrive.
    // Difficulty Scale: 80 to 200 turns
    [JsonProperty] public int Turns;

    public ZoneInstanceRandomQuest instance => _zone.instance as ZoneInstanceRandomQuest;

    public override void OnVisit()
    {
        if (game.isLoading) return;


    }

    public override void _OnTickRound()
    {
        // base.quest.difficulty
        Turns++;

        // Spawn first wave at wave 5, allowing 5 turns to get set up.
        if (Turns == 5)
        {
            SpawnAttackers();
        }
        else if (Turns > 5)
        {
            if (Turns % 10 == 5 || Turns % 10 == 0)
            {
                // Every 5 waves spawn more enemies.
                SpawnAttackers();
            }

            if (Turns % 30 == 0)
            {
                // Every 30 Turns spawn a boss.
                Rand.SetSeed(quest.uid);
                SpawnBoss(Turns > EClass.rnd(100));
                Rand.SetSeed();
            }
        }

        AggroEnemy();
    }

    // Spawn an allied hero unit when a specific number of kills has been met.
    public void SpawnHero()
    {
        // Picks a hero.
        // Default choice is a Watcher Knight
        // Depending on your rank with the Information Guild: Sena and Ruras can show up.
        // Depending on your rank with the Adventurers guild: Louise and Mitsune can show up.
        // Depending on your friendship level with Lailah: Lailah Golem can show up.
        // Depending on your friendship level with Doren: A squad of Aluena Watch can show up to help.
    }

    // Spawn a batch of defenders
    public void SpawnDefenders()
    {
        // Allied forces include: 2 Knights, 2 Archers, 2 Healers, and 1 Mage.
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

    //
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
        if (!c.IsPCParty && !c.IsPCPartyMinion)
        {
            Kills++;
            if (Kills == 50)
            {
                SpawnHero();
            }
        }
    }
}