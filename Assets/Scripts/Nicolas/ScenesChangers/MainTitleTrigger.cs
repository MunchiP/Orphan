using UnityEngine;
using UnityEngine.SceneManagement;
public class MainTitleTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
