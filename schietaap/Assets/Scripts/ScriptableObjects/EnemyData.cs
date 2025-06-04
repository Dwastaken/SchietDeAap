using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("BasisInformatie")]
    public string enemyname = "new Enemy";
    public float maxHealth = 100f;
    public float moveSpeed = 5f;

    [Header("Gedrag")]
    public bool isAggressive = true;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float FOV = 85f;

    [Header("Specials")]
    public bool isBoss = false;
}
