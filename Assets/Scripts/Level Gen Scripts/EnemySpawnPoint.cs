using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    public enum EnemySpawnDirection {Left, Right, Either}
    public enum EnemyGroundType {Grounded, Flying, Any}

    public EnemySpawnDirection SpawnDirection = EnemySpawnDirection.Either;
    public EnemyGroundType AerialType = EnemyGroundType.Grounded;

}
