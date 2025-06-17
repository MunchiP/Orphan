using System;
using UnityEngine;

public class HabilitiesActivatorSwitch : MonoBehaviour
{
    private WallJumpController wallJumpController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wallJumpController =GetComponent<WallJumpController>();
        if (PlayerPrefs.GetInt("hability1", 0) == 1)
        {
            wallJumpController.enabled = true;
        }
        else
        {
            wallJumpController.enabled = false;
        }

    }

    void OnEnable()
    {
        wallJumpController.GetComponent<WallJumpController>();
        if (PlayerPrefs.GetInt("hability1", 0) == 1)
        {
            wallJumpController.enabled = true;
        }
        else
        {
            wallJumpController.enabled = false;
        }

    }
    public void ActivarHabilidad1()
    {
        wallJumpController = GetComponent<WallJumpController>();
        wallJumpController.enabled = true;
    }
}
