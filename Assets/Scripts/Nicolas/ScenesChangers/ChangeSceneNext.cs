using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneNext : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FadeManager fadeManager = FindAnyObjectByType<FadeManager>();
            fadeManager.LoadNextScene();
        }        
    }
}
