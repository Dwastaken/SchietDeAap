using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Transform hitTransform = other.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
        }
        Destroy(gameObject);
    }
}
