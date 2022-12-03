using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class SimpleSpheres : MonoBehaviour {
    public ComputeShader Shader;
    public Transform MainLight;
    public Sphere[] Spheres;
    public RawImage OutputTex;
    public RawImage DepthTex;
    public bool RunContinuous;

    RenderTexture colorTex;

    private void Update() {
        if (RunContinuous) {
            RunShader();
        }
    }

    [Button]
    public void RunShader() {
        int kernelHandle = Shader.FindKernel("CSMain");

        //data buffer
        ComputeBuffer sphereBuffer = new ComputeBuffer(Spheres.Length, sizeof(float) * 8);
        sphereBuffer.SetData(Spheres);
        Shader.SetBuffer(kernelHandle, "sphereData", sphereBuffer);

        //output texture
        if (colorTex == null) {
            colorTex = new RenderTexture(1920, 1080, 24);
            colorTex.enableRandomWrite = true;
            colorTex.Create();
        }

        //comparison depth texture
        //RenderTexture depthTex = new RenderTexture(1920, 1080, 24);
        //depthTex.enableRandomWrite = true;
        //depthTex.Create();

        Shader.SetTexture(kernelHandle, "Result", colorTex);
        //Shader.SetTexture(kernelHandle, "Depth", depthTex);
        Shader.SetVector("mainLightDir", MainLight.eulerAngles);

        Shader.Dispatch(kernelHandle, 1920/8, 1080/8, 1);
        OutputTex.texture = colorTex;
        //DepthTex.texture = depthTex;
        sphereBuffer.Dispose();
    }

    [System.Serializable]
    public struct Sphere {
        public Vector3 position;
        public Color color;
        public float Radius;
    }
}