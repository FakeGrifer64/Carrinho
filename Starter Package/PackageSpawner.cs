using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PackageSpawner : MonoBehaviour
{
    public DrivingSurfaceManager DrivingSurfaceManager;
    public PackageBehaviour CurrentPackage;
    public ObstacleSpawner ObstacleSpawner; // Referência para o spawner de obstáculos

    [System.Serializable]
    public class PackageVariation
    {
        public GameObject Prefab;
        public int Points = 10;
    }

    public PackageVariation[] PackageVariations;

    public static PackageSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static Vector3 RandomInTriangle(Vector3 v1, Vector3 v2)
    {
        float u = Random.Range(0.0f, 1.0f);
        float v = Random.Range(0.0f, 1.0f);
        if (v + u > 1)
        {
            v = 1 - v;
            u = 1 - u;
        }
        return (v1 * u) + (v2 * v);
    }

    public static Vector3 FindRandomLocation(ARPlane plane)
    {
        var meshFilter = plane.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("No mesh found on ARPlane");
            return plane.center;
        }

        var mesh = meshFilter.mesh;
        var triangles = mesh.triangles;
        if (triangles.Length == 0)
        {
            return plane.center;
        }

        var triangleIndex = Random.Range(0, triangles.Length / 3);
        var firstVertexIndex = triangleIndex * 3;
        var vertices = mesh.vertices;

        var v1 = vertices[triangles[firstVertexIndex]];
        var v2 = vertices[triangles[firstVertexIndex + 1]];
        var v3 = vertices[triangles[firstVertexIndex + 2]];

        var randomInTriangle = RandomInTriangle(v1, v2);
        randomInTriangle = RandomInTriangle(randomInTriangle, v3);

        return plane.transform.TransformPoint(randomInTriangle);
    }

    public void SpawnPackage(ARPlane plane)
    {
        if (PackageVariations == null || PackageVariations.Length == 0)
        {
            Debug.LogError("No package variations assigned!");
            return;
        }

        var selectedVariation = PackageVariations[Random.Range(0, PackageVariations.Length)];
        var packageClone = Instantiate(selectedVariation.Prefab);
        packageClone.transform.position = FindRandomLocation(plane);

        CurrentPackage = packageClone.GetComponent<PackageBehaviour>();
        if (CurrentPackage != null)
        {
            CurrentPackage.Points = selectedVariation.Points;
        }
    }

    public void SpawnNewSet()
    {
        var lockedPlane = DrivingSurfaceManager.LockedPlane;
        if (lockedPlane != null)
        {
            // Destrói o pacote e obstáculo atuais se existirem
            if (CurrentPackage != null) Destroy(CurrentPackage.gameObject);
            if (ObstacleSpawner.CurrentObstacle != null)
                Destroy(ObstacleSpawner.CurrentObstacle.gameObject);

            // Spawna novo pacote e obstáculo
            SpawnPackage(lockedPlane);
            ObstacleSpawner.SpawnObstacle(lockedPlane);
        }
    }

    private void Update()
    {
        var lockedPlane = DrivingSurfaceManager.LockedPlane;
        if (lockedPlane != null)
        {
            if (CurrentPackage == null || !CurrentPackage.gameObject.activeInHierarchy)
            {
                SpawnNewSet(); // Spawna ambos quando não há pacote
            }
            else
            {
                // Mantém ambos alinhados com o plano
                var packagePosition = CurrentPackage.transform.position;
                packagePosition.y = lockedPlane.center.y;
                CurrentPackage.transform.position = packagePosition;

                if (ObstacleSpawner.CurrentObstacle != null)
                {
                    var obstaclePosition = ObstacleSpawner.CurrentObstacle.transform.position;
                    obstaclePosition.y = lockedPlane.center.y;
                    ObstacleSpawner.CurrentObstacle.transform.position = obstaclePosition;
                }
            }
        }
    }
}