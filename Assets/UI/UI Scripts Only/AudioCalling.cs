using UnityEngine;

public class AudioCalling : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic("MainTheme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
