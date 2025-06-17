using UnityEngine;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject mainCanvas;
    public GameObject InGameCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InGameCanvas.SetActive(false);
        mainCanvas.SetActive(false);
        videoPlayer.loopPointReached += OnVideoEnd;

    }
   

    void OnVideoEnd(VideoPlayer vp)
    {
        mainCanvas.SetActive(true);
        InGameCanvas.SetActive(true );
    }
}
