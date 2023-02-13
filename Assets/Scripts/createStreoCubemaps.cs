using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class createStreoCubemaps : MonoBehaviour
{
    public RenderTexture cubemapLeft;
    public RenderTexture cubemapRight;
    public RenderTexture equirect;
    public bool renderStereo = true;
    public float stereoSeparation = 0.064f;

    void LateUpdate(){
        Camera cam = GetComponent<Camera>();
        if(cam == null){
            cam = GetComponentInParent<Camera>();
        }
        if(cam == null){
            Debug.Log("stereo 360 capture node has no camera or parent camera");
        }
        if(renderStereo){
            cam.stereoSeparation = stereoSeparation;
            cam.RenderToCubemap(cubemapLeft, 63, Camera.MonoOrStereoscopicEye.Left);
            cam.RenderToCubemap(cubemapRight, 63, Camera.MonoOrStereoscopicEye.Right);
            
        }else{
            cam.RenderToCubemap(cubemapLeft, 63, Camera.MonoOrStereoscopicEye.Mono);
            
        }
        if(equirect == null){
            return;
        }
        if(renderStereo){
            cubemapLeft.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Left);
            cubemapRight.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Right);
            
        }
    }

}
