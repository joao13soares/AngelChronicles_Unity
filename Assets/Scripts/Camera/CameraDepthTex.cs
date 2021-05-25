using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDepthTex : MonoBehaviour
{

    [SerializeField]
    DepthTextureMode depthTextureMode;


    private void SetCameraDepthTextureMode()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }
}
