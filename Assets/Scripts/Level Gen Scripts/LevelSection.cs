using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSection : MonoBehaviour
{

    public GameObject LeftSidePosition;

    public LevelSectionTransition LeftTransition;

    public GameObject RightSidePosition;

    public LevelSectionTransition RightTransition;

    public LevelBlock LeftLevelBlock;
    public LevelBlock RightLevelBlock;
    public bool completed;
    public LevelGenHandler LevelHandler;

    public Transform[] ObjectSpawnPoints;

    [Tooltip("The objects that can possibly be spawned at the spawn points")]
    public GameObject[] ObjectPool;

    public Transform[] EnemySpawnPoints;
    
    [Tooltip("The objects that can possibly be spawned at the spawn points")]
    public  BasicEnemyScript[] EnemyPool;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(RightLevelBlock != null && completed == false)
        {
            if(RightLevelBlock.Health.GetHealth() <= 1 && completed == false)
            {
                completed = true;
                LevelHandler.CurrentSection++;
            }
        }
        else if(RightLevelBlock == null && completed == false)
        {
            completed = true;
            LevelHandler.CurrentSection++;
        }
    }

    public void InitializeSection()
    {
        for(int i = 0; i < ObjectSpawnPoints.Length; i++)
        {
            if(ObjectPool.Length >= 1)
            {
                if(Random.Range(0, 2) > 0)
                {
                    GameObject spawnedObject = ObjectPool[Random.Range(0, ObjectPool.Length)];
                    spawnedObject = GameObject.Instantiate<GameObject>(spawnedObject, ObjectSpawnPoints[i]);
                    //spawnedObject.transform.parent = null;
                }
            }
            
        }
        for(int i = 0; i < EnemySpawnPoints.Length; i++)
        {
            if(EnemyPool.Length >= 1)
            {
                if(Random.Range(0, 2) > 0)
                {
                    BasicEnemyScript spawnedEnemy = EnemyPool[Random.Range(0, EnemyPool.Length)];
                    spawnedEnemy = GameObject.Instantiate<BasicEnemyScript>(spawnedEnemy, EnemySpawnPoints[i]);
                    //spawnedEnemy.EnemyAI=(BasicEnemyScript.EnemyAiType)Random.Range(0,4);
                    spawnedEnemy.transform.parent = this.transform;
                }
            }
        }
    }
}
