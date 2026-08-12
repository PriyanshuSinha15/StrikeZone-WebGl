using UnityEngine;

[CreateAssetMenu(fileName = "Level_SO", menuName = "Scriptable Objects/Level_SO")]
public class Level_SO : ScriptableObject
{
    public int level;
    public int enemyCount;
    public float enemyDamage;
    public float enemySpeed;
}
