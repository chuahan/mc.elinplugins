using Newtonsoft.Json;
namespace PromotionMod.Source.QuestEventSystem.QuestZones;

public class ZoneEventDefendPosition : ZoneEventQuest
{
    // 5 Turns before Enemies arrive.
    // Difficulty Scale: 80 to 200 turns
    [JsonProperty] public int Turns;
    [JsonProperty] public int Kills;
    
    public ZoneInstanceRandomQuest instance => EClass._zone.instance as ZoneInstanceRandomQuest;

    public override void OnVisit()
    {
        if (EClass.game.isLoading) return;
        
        
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
                Rand.SetSeed(base.quest.uid);
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
            Chara chara = EClass._zone.SpawnMob(EClass._map.bounds.GetRandomEdge().GetNearestPoint(allowChara: false), SpawnSetting.DefenseEnemy(EClass._zone.DangerLv));
            chara.hostility = chara.c_originalHostility = Hostility.Enemy;
            if (this.CountEnemy)
                this.enemies.Add(chara.uid);
        }
    }

    //
    public void SpawnAttackingCommander(bool evolve = false)
    {
        Point nearestPoint = EClass._map.bounds.GetRandomEdge().GetNearestPoint(allowChara: false);
        Chara chara = evolve ? EClass._zone.TryGenerateEvolved(true, nearestPoint) : EClass._zone.SpawnMob(nearestPoint, SpawnSetting.Boss(EClass._zone.DangerLv));
        chara.hostility = chara.c_originalHostility = Hostility.Enemy;
        if (this.CountEnemy)
            this.enemies.Add(chara.uid);
        if (!this.WarnBoss)
            return;
        Msg.Say("defense_boss", chara.Name);
        EClass.game.Pause();
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