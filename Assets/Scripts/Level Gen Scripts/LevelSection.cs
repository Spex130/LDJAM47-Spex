using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSection : MonoBehaviour
{

    public GameObject LeftSidePosition;

    public LevelSectionTransition LeftTransition;

    public GameObject RightSidePosition;

    public LevelSectionTransition RightTransition;

    public Transform[] ObjectSpawnPoints;

    [Tooltip("The objects that can possibly be spawned at the spawn points")]
    public GameObject[] ObjectPool;

    public Transform[] EnemySpawnPoints;
    
    [Tooltip("The objects that can possibly be spawned at the spawn points")]
    public  BasicEnemyScript[] EnemyPool;

    public BasicEnemyScript TestEnemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeSection()
    {
        for(int i = 0; i < ObjectSpawnPoints.Length; i++)
        {
            if(ObjectPool.Length >= 1)
            {
                GameObject spawnedObject = ObjectPool[Random.Range(0, ObjectPool.Length)];
                GameObject.Instantiate<GameObject>(spawnedObject, ObjectSpawnPoints[i]);
                print("Spawned!");
            }
            
        }
    }
}
