using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamaraManager : MonoBehaviour
{

   /* public static CamaraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallPanTime = 0.35f;
    public float fallSpeedYDampingChangeThershold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }
    private Coroutine lerpYPanCoroutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    private float normYPanAmount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i]; // set the current active camera
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>(); //set the framing transporter
            }
        }

        normYPanAmount = framingTransposer.m_YDamping;
    }

    // normYPanAmount = framingTransposer.m

    void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        //Toma la cantidad inicial de dumping
        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        // Elimina la cantidad de dumping tomada
        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }


        float elapsedTime = 0f;
        while (elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallPanAmount));
            framingTransposer.m_YDamping = lerpPanAmount;

            yield return null;
        }
        IsLerpingYDamping = false;
    }
   */
}
