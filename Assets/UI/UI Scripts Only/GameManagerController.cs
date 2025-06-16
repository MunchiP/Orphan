using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public static GameManagerController instance;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
