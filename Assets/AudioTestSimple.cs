using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioTestSimple : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        var source = GetComponent<AudioSource>();

        if (clip == null)
        {
            Debug.LogWarning("No hay clip asignado.");
            return;
        }

        Debug.Log("Reproduciendo audio...");
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D
        source.volume = 1f;

        source.PlayOneShot(clip);
    }
}
