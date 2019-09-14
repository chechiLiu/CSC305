using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Part2 : MonoBehaviour {
    Texture2D RayTracingResult;

    // Use this for initialization
    void Start() {
        Camera this_camera = gameObject.GetComponent<Camera>();
        Debug.Assert(this_camera);

        int pixel_width = this_camera.pixelWidth;
        int pixel_height = this_camera.pixelHeight;

        RayTracingResult = new Texture2D(pixel_width, pixel_height);

        Vector3 RayOrigin = Vector3.zero;
        Vector3 VPCenter = Vector3.forward;

        float ViewportWidth = 3;
        float ViewportHeight = ViewportWidth / pixel_width * pixel_height;

        float VPWidthHalf = ViewportWidth / 2;
        float VPHeightHalf = ViewportHeight / 2;

        float PixelWidthHalf = pixel_width / 2;
        float PixelHeightHalf = pixel_height / 2;

        Color BackgroundColor = Color.grey;

        List<Vector3> vertices = new List<Vector3>();
        //Vectors for the triangle
        vertices.Add(new Vector3(-4, -2.8f, 10)); //0
        vertices.Add(new Vector3(-4, 2.8f, 10));  //1
        vertices.Add(new Vector3(0, -2.8f, 9));   //2
        vertices.Add(new Vector3(0, 2.8f, 9));    //3
        vertices.Add(new Vector3(4, -2.8f, 10));  //4
        vertices.Add(new Vector3(4, 2.8f, 10));   //5

        List<int> indices = new List<int>();

        //triangle 1
        indices.Add(0);
        indices.Add(1);
        indices.Add(2);
        //triangle 2
        indices.Add(2);
        indices.Add(1);
        indices.Add(3);
        //triangle 3
        indices.Add(2);
        indices.Add(3);
        indices.Add(5);
        //triangle 4
        indices.Add(2);
        indices.Add(5);
        indices.Add(4);

        Vector3 RayDirection = VPCenter;

        for (int i = 0; i < pixel_width; ++i) {
            for (int j = 0; j < pixel_height; ++j) {
                RayDirection.x = (i - PixelWidthHalf) / PixelWidthHalf * VPWidthHalf;
                RayDirection.y = (j - PixelHeightHalf) / PixelHeightHalf * VPHeightHalf;

                //set background to grey first
                RayTracingResult.SetPixel(i, j, Color.grey);

                RayDirection.Normalize();

                Color pixelColor = new Color();

                //vA - vB
                float a = vertices[0].x - vertices[1].x;
                float b = vertices[0].y - vertices[1].y;
                float c = vertices[0].z - vertices[1].z;

                //vA - vC
                float d = vertices[0].x - vertices[2].x;
                float e = vertices[0].y - vertices[2].y;
                float f = vertices[0].z - vertices[2].z;

                float g = RayDirection.x;
                float h = RayDirection.y;
                float ii = RayDirection.z;

                float jj = vertices[0].x - RayOrigin.x;
                float k = vertices[0].y - RayOrigin.y;
                float l = vertices[0].z - RayOrigin.z;

                //Normal POR:
                Vector3 v1 = new Vector3(a, b, c);
                Vector3 v2 = new Vector3(d, e, f);

                //N = cross((vA - vB), (vA - vC));
                Vector3 Normal = Vector3.Cross(v1, v2);

                //dot(N, d)
                float NDD = Vector3.Dot(Normal, RayDirection);

                //vA-O
                float q = vertices[0].x - RayOrigin.x;
                float w = vertices[0].y - RayOrigin.y;
                float ee = vertices[0].z - RayOrigin.z;

                Vector3 x = new Vector3(q, w, ee);

                //dot(N, (vA-O))
                float NvA = Vector3.Dot(Normal, x);

                //t = dot(N, (vA-o))/dot(N, d)
                float t = NvA / NDD;

                //barycentric coordinate 
                //Cramer rule
                float ei_hf = e * ii - h * f;

                float gf_di = g * f - d * ii;

                float dh_eg = d * h - e * g;

                float ak_jb = a * k - jj * b;

                float jc_al = jj * c - a * l;

                float bl_kc = b * l - k * c;

                float M = (a * ei_hf) + (b * gf_di) + (c * dh_eg);

                float beta = (jj * ei_hf + k * gf_di + l * dh_eg) / M;

                float gamma = (ii * ak_jb + h * jc_al + g * bl_kc) / M;

                float alpha = 1 - beta - gamma;
                
                Vector3 barycentric_coordinate = new Vector3(alpha, beta, gamma);

                if (t > 0) {
                    if (beta > 0 && gamma > 0 && (beta + gamma) < 1) {
                        pixelColor.r = barycentric_coordinate.x;
                        pixelColor.g = barycentric_coordinate.y;
                        pixelColor.b = barycentric_coordinate.z;

                        RayTracingResult.SetPixel(i, j, pixelColor);
                    }
                }
            }
        }

        for (int i = 0; i < pixel_width; ++i) {
            for (int j = 0; j < pixel_height; ++j) {
                RayDirection.x = (i - PixelWidthHalf) / PixelWidthHalf * VPWidthHalf;
                RayDirection.y = (j - PixelHeightHalf) / PixelHeightHalf * VPHeightHalf;

                //RayTracingResult.SetPixel(i, j, Color.grey);

                RayDirection.Normalize();

                Color pixelColor = new Color();

                float a = vertices[2].x - vertices[1].x;
                float b = vertices[2].y - vertices[1].y;
                float c = vertices[2].z - vertices[1].z;

                float d = vertices[2].x - vertices[3].x;
                float e = vertices[2].y - vertices[3].y;
                float f = vertices[2].z - vertices[3].z;

                float g = RayDirection.x;
                float h = RayDirection.y;
                float ii = RayDirection.z;

                float jj = vertices[2].x - RayOrigin.x;
                float k = vertices[2].y - RayOrigin.y;
                float l = vertices[2].z - RayOrigin.z;

                //Normal POR:
                Vector3 v1 = new Vector3(a, b, c);
                Vector3 v2 = new Vector3(d, e, f);

                Vector3 Normal = Vector3.Cross(v1, v2);

                float NDD = Vector3.Dot(Normal, RayDirection);

                float q = vertices[2].x - RayOrigin.x;
                float w = vertices[2].y - RayOrigin.y;
                float ee = vertices[2].z - RayOrigin.z;

                Vector3 x = new Vector3(q, w, ee);

                float NvA = Vector3.Dot(Normal, x);

                float t = NvA / NDD;

                //barycentric coordinate 
                //Cramer rule
                float ei_hf = e * ii - h * f;

                float gf_di = g * f - d * ii;

                float dh_eg = d * h - e * g;

                float ak_jb = a * k - jj * b;

                float jc_al = jj * c - a * l;

                float bl_kc = b * l - k * c;

                float M = (a * ei_hf) + (b * gf_di) + (c * dh_eg);

                float beta = (jj * ei_hf + k * gf_di + l * dh_eg) / M;

                float gamma = (ii * ak_jb + h * jc_al + g * bl_kc) / M;

                float alpha = 1 - beta - gamma;

                Vector3 barycentric_coordinate = new Vector3(alpha, beta, gamma);

                if (t > 0)
                {
                    if (beta > 0 && gamma > 0 && (beta + gamma) < 1)
                    {
                        pixelColor.r = barycentric_coordinate.x;
                        pixelColor.g = barycentric_coordinate.y;
                        pixelColor.b = barycentric_coordinate.z;

                        RayTracingResult.SetPixel(i, j, pixelColor);
                    }
                }
            }
        }

        for (int i = 0; i < pixel_width; ++i) {
            for (int j = 0; j < pixel_height; ++j) {
                RayDirection.x = (i - PixelWidthHalf) / PixelWidthHalf * VPWidthHalf;
                RayDirection.y = (j - PixelHeightHalf) / PixelHeightHalf * VPHeightHalf;

                //RayTracingResult.SetPixel(i, j, Color.grey);

                RayDirection.Normalize();

                Color pixelColor = new Color();

                float a = vertices[2].x - vertices[3].x;
                float b = vertices[2].y - vertices[3].y;
                float c = vertices[2].z - vertices[3].z;

                float d = vertices[2].x - vertices[5].x;
                float e = vertices[2].y - vertices[5].y;
                float f = vertices[2].z - vertices[5].z;

                float g = RayDirection.x;
                float h = RayDirection.y;
                float ii = RayDirection.z;

                float jj = vertices[2].x - RayOrigin.x;
                float k = vertices[2].y - RayOrigin.y;
                float l = vertices[2].z - RayOrigin.z;

                //Normal POR:
                Vector3 v1 = new Vector3(a, b, c);
                Vector3 v2 = new Vector3(d, e, f);

                Vector3 Normal = Vector3.Cross(v1, v2);

                float NDD = Vector3.Dot(Normal, RayDirection);

                float q = vertices[2].x - RayOrigin.x;
                float w = vertices[2].y - RayOrigin.y;
                float ee = vertices[2].z - RayOrigin.z;

                Vector3 x = new Vector3(q, w, ee);

                float NvA = Vector3.Dot(Normal, x);

                float t = NvA / NDD;

                //barycentric coordinate 
                //Cramer rule
                float ei_hf = e * ii - h * f;

                float gf_di = g * f - d * ii;

                float dh_eg = d * h - e * g;

                float ak_jb = a * k - jj * b;

                float jc_al = jj * c - a * l;

                float bl_kc = b * l - k * c;

                float M = (a * ei_hf) + (b * gf_di) + (c * dh_eg);

                float beta = (jj * ei_hf + k * gf_di + l * dh_eg) / M;

                float gamma = (ii * ak_jb + h * jc_al + g * bl_kc) / M;

                float alpha = 1 - beta - gamma;

                Vector3 barycentric_coordinate = new Vector3(alpha, beta, gamma);

                if (t > 0)
                {
                    if (beta > 0 && gamma > 0 && (beta + gamma) < 1)
                    {
                        pixelColor.r = barycentric_coordinate.x;
                        pixelColor.g = barycentric_coordinate.y;
                        pixelColor.b = barycentric_coordinate.z;

                        RayTracingResult.SetPixel(i, j, pixelColor);
                    }
                }
            }
        }

        for (int i = 0; i < pixel_width; ++i) {
            for (int j = 0; j < pixel_height; ++j) {
                RayDirection.x = (i - PixelWidthHalf) / PixelWidthHalf * VPWidthHalf;
                RayDirection.y = (j - PixelHeightHalf) / PixelHeightHalf * VPHeightHalf;

                //RayTracingResult.SetPixel(i, j, Color.grey);

                RayDirection.Normalize();

                Color pixelColor = new Color();

                float a = vertices[2].x - vertices[5].x;
                float b = vertices[2].y - vertices[5].y;
                float c = vertices[2].z - vertices[5].z;

                float d = vertices[2].x - vertices[4].x;
                float e = vertices[2].y - vertices[4].y;
                float f = vertices[2].z - vertices[4].z;

                float g = RayDirection.x;
                float h = RayDirection.y;
                float ii = RayDirection.z;

                float jj = vertices[2].x - RayOrigin.x;
                float k = vertices[2].y - RayOrigin.y;
                float l = vertices[2].z - RayOrigin.z;

                //Normal POR:
                Vector3 v1 = new Vector3(a, b, c);
                Vector3 v2 = new Vector3(d, e, f);

                Vector3 Normal = Vector3.Cross(v1, v2);

                float NDD = Vector3.Dot(Normal, RayDirection);

                float q = vertices[2].x - RayOrigin.x;
                float w = vertices[2].y - RayOrigin.y;
                float ee = vertices[2].z - RayOrigin.z;

                Vector3 x = new Vector3(q, w, ee);

                float NvA = Vector3.Dot(Normal, x);

                float t = NvA / NDD;

                //barycentric coordinate 
                //Cramer rule
                float ei_hf = e * ii - h * f;

                float gf_di = g * f - d * ii;

                float dh_eg = d * h - e * g;

                float ak_jb = a * k - jj * b;

                float jc_al = jj * c - a * l;

                float bl_kc = b * l - k * c;

                float M = (a * ei_hf) + (b * gf_di) + (c * dh_eg);

                float beta = (jj * ei_hf + k * gf_di + l * dh_eg) / M;

                float gamma = (ii * ak_jb + h * jc_al + g * bl_kc) / M;

                float alpha = 1 - beta - gamma;

                Vector3 barycentric_coordinate = new Vector3(alpha, beta, gamma);

                if (t > 0)
                {
                    if (beta > 0 && gamma > 0 && (beta + gamma) < 1)
                    {
                        pixelColor.r = barycentric_coordinate.x;
                        pixelColor.g = barycentric_coordinate.y;
                        pixelColor.b = barycentric_coordinate.z;

                        RayTracingResult.SetPixel(i, j, pixelColor);
                    }
                }
            }
        }

        RayTracingResult.Apply();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Show the generated ray tracing image on screen
        Graphics.Blit(RayTracingResult, destination);
    }

}
