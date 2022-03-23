using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FindPlayerAtStart: MonoBehaviour
{
    private static bool retainCamera;

    void Awake()
    {
        //vcam = GetComponent<CinemachineVirtualCamera>();
        DontDestroyOnLoad(GameObject.Find("CM vcam1"));
    }

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (!retainCamera)
        {
            retainCamera = true;
            DontDestroyOnLoad(transform.gameObject);
            Debug.Log("Camera Loaded");

        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Camera Destroyed!");
        }
    }
}