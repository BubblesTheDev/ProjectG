using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class roomEnemySpawner : MonoBehaviour
{
    public bool doorClosed = false;
    [SerializeField] private bool spawnerActive = false; 
    [SerializeField] private bool playerInRoom = false;
    [SerializeField] private int currentWaveIndex;
    [SerializeField] private float timeBetweenEnemySpawns;
    [SerializeField] private List<wave> waves = new List<wave>();
    [HideInInspector] public List<GameObject> enemiesRemaining = new List<GameObject>();

    [SerializeField] private Animator doorController;
    [SerializeField] private int hpToHeal = 2;

    private playerHealth playerStats;

    private void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<playerHealth>();
    }

    private void Update()
    {
        if (playerInRoom && !spawnerActive && currentWaveIndex < waves.Count)
        {
            if (waves[currentWaveIndex].noEnemiesRemaining && enemiesRemaining.Count <= 0 || !waves[currentWaveIndex].noEnemiesRemaining)
            {
                StartCoroutine(spawnWave());
            }
        }
        else if (currentWaveIndex == waves.Count && enemiesRemaining.Count <= 0 && doorClosed)
        {
            openDoor();
            playerStats.currentHP += hpToHeal;
            playerStats.healedDamage.Invoke();
        }
    }

    private IEnumerator spawnWave()
    {
        spawnerActive = true;
        yield return new WaitForSeconds(waves[currentWaveIndex].delayBeforeWave);
        for (int i = 0; i < waves[currentWaveIndex].enemies.Count; i++)
        {
            AudioManager.instance.PlaySFX(FMODEvents.instance.enemySpawn, this.transform.position);
            GameObject temp = Instantiate(waves[currentWaveIndex].enemies[i].enemyToSpawn,
                waves[currentWaveIndex].enemies[i].spawnPoint.transform.position,
                waves[currentWaveIndex].enemies[i].spawnPoint.transform.rotation,
                GameObject.Find("EnemyHolder").transform);

            enemiesRemaining.Add(temp);
            temp.GetComponent<enemyStats>().spawner = GetComponent<roomEnemySpawner>();
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
        }
        spawnerActive = false;
        currentWaveIndex++;
    }

    private void closeDoor()
    {
        //Play door close animation here
        doorClosed = true;
        if (doorController != null) doorController.Play("CloseDoor");
    }

    private void openDoor()
    {
        //play open door animation here
        doorClosed = false;
        if (doorController != null) doorController.Play("OpenDoor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) if (!playerInRoom)
            {
                playerInRoom = true;
                closeDoor();
            }

    }
}
[System.Serializable]
public struct wave
{
    public bool noEnemiesRemaining;
    public float delayBeforeWave;
    public List<enemyAndPos> enemies;
}

[System.Serializable]
public struct enemyAndPos
{
    public GameObject enemyToSpawn;
    public GameObject spawnPoint;
}
