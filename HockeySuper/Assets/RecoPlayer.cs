using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

public class RecoPlayer : MonoBehaviour
{

    private Emgu.CV.VideoCapture webcam;
    private Mat webcamFrame;
    public UnityEngine.UI.RawImage rawImage;
    public Texture2D tex;

    // Start is called before the first frame update
    void Start()
    {
        webcam = new VideoCapture(0, VideoCapture.API.DShow);
        webcamFrame = new Mat();
        webcam.ImageGrabbed += HandleWebcamQueryFrame;
        webcam.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (webcam != null && webcam.IsOpened)
        {
            webcam.Grab();
            DisplayFrameOnPlan();
        }
        else
        {
            if (webcam != null)
            {
                Destroy();
            }
        }

    }

    private void HandleWebcamQueryFrame(object sender, System.EventArgs e)
    {
        webcam.Retrieve(webcamFrame, 0);

        if (!webcamFrame.IsEmpty)
        {
            Debug.Log(webcamFrame.Size);
        }
    }

    void DisplayFrameOnPlan()
    {
        if (webcamFrame.IsEmpty) return;

        rawImage.rectTransform.sizeDelta = new Vector2(webcamFrame.Width, webcamFrame.Height);

        int width = (int)rawImage.rectTransform.rect.width;
        int height = (int)rawImage.rectTransform.rect.height;

        // destroy existing texture
        if (tex != null)
        {
            Destroy(tex);
            tex = null;
        }
        // creating new texture to hold our frame
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Resize mat to the texture format
        CvInvoke.Resize(webcamFrame, webcamFrame, new System.Drawing.Size(width, height));
        // Convert to unity texture format ( RGBA )
        CvInvoke.CvtColor(webcamFrame, webcamFrame, ColorConversion.Bgr2Rgba);
        // Flipping because unity texture is inverted.
        CvInvoke.Flip(webcamFrame, webcamFrame, FlipType.Vertical);

        // loading texture in texture object
        tex.LoadRawTextureData(webcamFrame.ToImage<Rgba, byte>().Bytes);
        tex.Apply();

        // assigning texture to gameObject
        rawImage.texture = tex;
    }

    void findNewFace(Mat frame)
    {
        //System.Drawing.Rectangle[] faces = cascade.DetectMultiScale(frame, 1.1, 2, System.Drawing.Size.Empty);
        //
        //if (faces.Length >= 1)
        //{
        //    float minSize = 0;
        //    System.Drawing.Rectangle visage = new System.Drawing.Rectangle();
        //
        //    foreach (System.Drawing.Rectangle r in faces)
        //    {
        //        if (minSize < r.Width * r.Height)
        //        {
        //            minSize = r.Width * r.Height;
        //            visage = r;
        //        }
        //    }
        //
        //    if (!visage.IsEmpty)
        //    {
        //        CvInvoke.Rectangle(frame, visage, new MCvScalar(100, 100, 0), 2);
        //    }
        //
        //    Debug.Log(faces[0].Location);
        //}
    }


    private void Destroy()
    {
        if (webcam != null)
        {

            Debug.Log("sleeping");
            //waiting for thread to finish before disposing the camera...(took a while to figure out)
            System.Threading.Thread.Sleep(60);
            // close camera
            webcam.Stop();
            webcam.Dispose();
        }
    }
}
