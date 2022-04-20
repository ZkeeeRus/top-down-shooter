using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    public bool isCleared;
    public bool isBoss;

    private bool isBattle = false;

    private int enemiesCount;

    
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] enemiesPrefab;
    [SerializeField] private GameObject[] bossesPrefab;

    private GameObject enemy;
    public List<GameObject> enemies = new List<GameObject>();

    private PlayerHandler player;

    private GameObject[] lockingBlocks = new GameObject[4];

    private AmbientSound audioPlayer;

    private void Start()
    {
        lockingBlocks[0] = GetComponentInParent<Room>().DoorU;
        lockingBlocks[1] = GetComponentInParent<Room>().DoorD;
        lockingBlocks[2] = GetComponentInParent<Room>().DoorR;
        lockingBlocks[3] = GetComponentInParent<Room>().DoorL;

        isBoss = GetComponentInParent<Room>().isBoss;

        player = FindObjectOfType<PlayerHandler>();

        audioPlayer = FindObjectOfType<AmbientSound>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerBody"))
        {
            if (!isCleared && !isBattle)
            {
                StartBattle();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (isBattle && !isCleared)
        {
            if (enemies.Count == 0)
            {
                EndBattle();
            }
        }
    }

    private void SetLockingBlocksActive(bool active)
    {
        foreach (GameObject block in lockingBlocks)
        {
            if (block.GetComponent<Locks>().isDoor)
            {
                block.SetActive(active);
            }
        }
    }

    private void StartBattle()
    {
        if (isBoss)
        {
            SpawnBoss();
        }
        else
        {
            int i = 0, count = Random.Range(1,3);
            while (i < count)
            {
                int randomSpawn = Random.Range(0, spawnPositions.Length);
                var point = spawnPositions[randomSpawn].GetComponent<SpawnPointHandler>();
                //Debug.Log(point);
                if (!point.isSpawned)
                {
                    SpawnEnemy(randomSpawn);
                    point.isSpawned = true;
                    i++;
                }

            }
        }

        enemiesCount = enemies.Count;


        isBattle = true;

        SetLockingBlocksActive(true);

        //Debug.Log("Enter");
    }
    private void SpawnEnemy(int spawnPoint)
    {
        
        enemy = Instantiate(enemiesPrefab[Random.Range(0, enemiesPrefab.Length)], spawnPositions[spawnPoint]);

        enemy.transform.SetParent(transform);
        enemies.Add(enemy);

        enemy.GetComponent<EnemyHandler>().OnEnemyDead += RoomHandler_OnEnemyDead;
    }
    private void SpawnBoss()
    {
        enemy = Instantiate(bossesPrefab[0], spawnPositions[0]);

        enemy.transform.SetParent(transform);
        enemies.Add(enemy);

        enemy.GetComponent<EnemyHandler>().OnEnemyDead += RoomHandler_OnEnemyDead;


        var shield = Instantiate(bossesPrefab[1], spawnPositions[0]);
        shield.transform.SetParent(transform);


        var shieldGenerator = Instantiate(bossesPrefab[2], spawnPositions[1]);
        shieldGenerator.transform.SetParent(transform);
        shieldGenerator.GetComponent<EnemyHandler>().OnEnemyDead += RoomHandler_OnEnemyDead;

        audioPlayer.ChooseBossSong();
    }

    private void RoomHandler_OnEnemyDead(object sender, System.EventArgs e)
    {
        if ((sender as EnemyHandler).enemyType == EnemyHandler.Enemy.Generator)
        {
            
            Destroy(transform.Find("Shield(Clone)").gameObject);
        }
        else
        {
            enemies.RemoveRange(0, 1);
        }
    }

    private void EndBattle()
    {
        //Debug.Log("Cleared");
        isBattle = false;
        isCleared = true;

        player.Exp += 5 * enemiesCount;
       

        if (isBoss)
        {
            transform.Find("Hole").GetComponent<Collider2D>().enabled = true;
            transform.Find("Hole").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Decals/Hole");
        }

        SetLockingBlocksActive(false);
        audioPlayer.ChooseSong();
    }
}