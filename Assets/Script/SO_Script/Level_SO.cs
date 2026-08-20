using UnityEngine;

[CreateAssetMenu(fileName = "Level_SO", menuName = "Scriptable Objects/Level_SO")]
public class Level_SO : ScriptableObject
{
    public GameObject enemyPrefab;
    public int level;
    public int enemyCount;
    public float spawnTime;

    public int playerBoostScore;
}
