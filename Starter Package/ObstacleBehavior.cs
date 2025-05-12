using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Nome da cena a ser carregada

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Só ativa se for o player
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Nome da cena não definido no obstáculo!");
            }
        }
    }
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Colisão com obstáculo! Carregando cena: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Nome da próxima cena não definido no ObstacleBehaviour");
        }

    }
}