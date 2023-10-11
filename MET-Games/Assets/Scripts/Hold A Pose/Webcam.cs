using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webcam : MonoBehaviour
{
    static WebCamTexture cameraTexture;
    [SerializeField] private RenderTexture renderTexture;

    private void Start()
    {
        if (cameraTexture == null)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            cameraTexture = new WebCamTexture();


            #if UNITY_EDITOR
                cameraTexture.deviceName = devices[0].name;
            #else
                cameraTexture.deviceName = devices[1].name;
            #endif

        }

        GetComponent<Renderer>().material.mainTexture = cameraTexture;

        if (!cameraTexture.isPlaying)
        {
            cameraTexture.Play();
        }

        this.transform.localScale = new Vector3(cameraTexture.width, cameraTexture.height, 1f) / 25f;
    }
}
