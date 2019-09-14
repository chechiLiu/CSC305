using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Part1 : MonoBehaviour {
    Texture2D RayTracingResult;

    //Set Light source parameters
    Vector3 LightDirection = new Vector3(1, 1, -0.5f);
    Color LightColor = Color.yellow;

    //Define the sphere
    Vector3 SphereCenter = new Vector3(0, 0, 10);
    float SphereRadius = 3;

    // Use this for initialization
    void Start () {
        Camera this_camera = gameObject.GetComponent<Camera>();
        Debug.Assert(this_camera);

        int pixel_width = this_camera.pixelWidth;
        int pixel_height = this_camera.pixelHeight;

        RayTracingResult = new Texture2D(pixel_width, pixel_height);

        //Please put your own ray tracing code here
        Vector3 RayOrigin = Vector3.zero;
        Vector3 VPCenter = Vector3.forward;

        float ViewportWidth = 3;
        float ViewportHeight = ViewportWidth / pixel_width * pixel_height;

        float VPWidthHalf = ViewportWidth / 2;
        float VPHeightHalf = ViewportHeight / 2;
 
        float PixelWidthHalf = pixel_width / 2;
        float PixelHeightHalf = pixel_height / 2;

        Color BackgroundColor = Color.grey;
        Color AmbientColor = new Color(0.1f, 0.1f, 0);

        //Set a Background, Ambient Color, Diffuse Strength, Specular Strength and Power
        float diffuseStrength = 0.0008f;
        float specularStrength = 0.0007f;
        float specularPower = 4;

        
        //Calculate the current pixel position and ray direction
        Vector3 RayDirection = VPCenter;
        
        float discriminent;
        float t;
        Vector3 intersect_normal;

        for (int i = 0; i < pixel_width; ++i) {
            for (int j = 0; j < pixel_height; ++j) {
                RayDirection.x = (i - PixelWidthHalf) / PixelWidthHalf * VPWidthHalf;
                RayDirection.y = (j - PixelHeightHalf) / PixelHeightHalf * VPHeightHalf;

                //set background to grey first
                RayTracingResult.SetPixel(i, j, Color.grey);

                RayDirection.Normalize();

                Color PixelColor;

                //OC
                Vector3 EO = SphereCenter - RayOrigin;
                //OG
                float v = Vector3.Dot(EO, RayDirection);

                float RadiusSquared = SphereRadius * SphereRadius;
                float EOSquared = Vector3.Dot(EO, EO);

                discriminent = RadiusSquared - (EOSquared - v * v);

                if (discriminent > 0) {

                    //PG
                    float d = Mathf.Sqrt(discriminent);

                    //OP = OG - PG
                    t = v - d;

                    //position of p/intersection
                    Vector3 Intersection = RayOrigin + RayDirection * t;
                    intersect_normal = Intersection - SphereCenter;

                    PixelColor = AmbientColor;

                    //Diffuse
                    float diffuse = Vector3.Dot(intersect_normal, LightDirection) * diffuseStrength;
                    PixelColor += LightColor * diffuse;

                    //blinn parameters
                    Vector3 view = RayDirection * (-1);
                    Vector3 half = view + LightDirection;

                    //specular
                    float blinn = Vector3.Dot(half, intersect_normal);
                    float specular = Mathf.Pow(blinn, specularPower) * specularStrength;
                    PixelColor += LightColor * specular;

                    RayTracingResult.SetPixel(i, j, PixelColor);
                }
                else { //does not intersect, so just ignore.
                    intersect_normal = Vector3.zero;
                }
            }
        }
        RayTracingResult.Apply();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Show the generated ray tracing image on screen
        Graphics.Blit(RayTracingResult, destination);
    }
}
