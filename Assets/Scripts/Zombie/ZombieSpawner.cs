using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    //============== Unity Inspector ===============
    [Header("Requirements")] 
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform player;

    [Header("Parameters")] 
    [Range(1, 20)]
    [SerializeField] private int quantity;
    [Range(1, 20)]
    [SerializeField] private float safeRage;
    //==============================================
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < quantity; i++)
        {
            spawnZombie(spawnPoints);
        }
    }

    private void spawnZombie(GameObject[] spawnPoints)
    {
        bool findingSpawnPoint = true;

        while (findingSpawnPoint)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            GameObject sp = spawnPoints[randomIndex];
            
            //Spawnpoint should be 10units away the player
            if (Vector3.Distance(player.transform.position, sp.transform.position) > safeRage)
            {
                findingSpawnPoint = false;
                GameObject zombie = Instantiate(zombiePrefab);
                zombie.transform.position = sp.transform.position;
            }
            
        }
    }
}
