using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObstacleSpawner : MonoBehaviour
{
    public DrivingSurfaceManager DrivingSurfaceManager;
    public GameObject ObstaclePrefab;
    [HideInInspector]public ObstacleBehaviour CurrentObstacle;

    public void SpawnObstacle(ARPlane plane)
    {
        if (ObstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab not assigned!");
            return;
        }

        var obstacleClone = Instantiate(ObstaclePrefab);
        obstacleClone.transform.position = PackageSpawner.FindRandomLocation(plane);
        CurrentObstacle = obstacleClone.GetComponent<ObstacleBehaviour>();
    }
}