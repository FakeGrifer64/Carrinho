using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Nome da cena a ser carregada

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // S� ativa se for o player
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Nome da cena n�o definido no obst�culo!");
            }
        }
    }
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Colis�o com obst�culo! Carregando cena: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Nome da pr�xima cena n�o definido no ObstacleBehaviour");
        }

    }
}