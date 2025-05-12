
using UnityEngine;

public class PackageBehaviour : MonoBehaviour
{
    public int Points = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(Points);
            PackageSpawner.Instance.SpawnNewSet(); // Isso vai destruir e recriar ambos
        }
    }

}