using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

// DAGIER Mathieu

public class TP2 : MonoBehaviour
{
    VideoCapture webcam;
    Mat img = new Mat();
    public RawImage webcamScreen;

    // Start is called before the first frame update
    void Start()
    {
        webcam = new VideoCapture(0, VideoCapture.API.DShow);
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
        webcam.Retrieve(img, 0);

        if (!img.IsEmpty)
        {
            Debug.Log(img.Size);
        }
    }

    void DisplayFrameOnPlan()
    {
        //Création de la texture
        int width = webcam.Width;
        int height = webcam.Height;
        Texture2D textImage = new Texture2D(width, height, TextureFormat.RGBA32, false);

        //Conversion de l'image
        //CvInvoke.Resize(img, img, new System.Drawing.Size(width, height));
        CvInvoke.CvtColor(img, img, ColorConversion.Bgr2Rgba);
        CvInvoke.Flip(img, img, 0);

        findNewFace(img);
        //Copie de l'image sur la texture
        textImage.LoadRawTextureData(img.ToImage<Rgba, byte>().Bytes);

        //Ajout de la texture a webcamScreen
        webcamScreen.texture = textImage;
        textImage.Apply();
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
        webcam.Stop();
        webcam.Dispose();
        webcam = null;
    }
}