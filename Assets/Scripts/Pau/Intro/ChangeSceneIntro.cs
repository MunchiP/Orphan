using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneIntro : MonoBehaviour
{
    // [SerializeField] private int indexScene;

    public void Cambiar()
    {
        SceneManager.LoadScene(1);
    }
}
