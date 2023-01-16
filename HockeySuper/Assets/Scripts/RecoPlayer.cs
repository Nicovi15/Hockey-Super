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
    [Header("Green Settings")]
    [SerializeField] 
    Vector3 bornInfGreen;
    [SerializeField]
    Vector3 bornSuppGreen;

    [Header("Blue Settings")]
    [SerializeField] 
    Vector3 bornInfBlue;
    [SerializeField] 
    Vector3 bornSuppBlue;


    private Emgu.CV.VideoCapture webcam;
    private Mat webcamFrame;
    [Header("Cam Settings")]
    public UnityEngine.UI.RawImage rawImage;
    Texture2D tex;

    public Vector2 greenPos;
    public Vector2 bluePos;

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
            findPlayers();
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
            //Debug.Log(webcamFrame.Size);
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

    void findPlayers()
    {
        // HSV
        Emgu.CV.Image<Hsv, byte> hsv = new Emgu.CV.Image<Hsv, byte>(webcamFrame.Width, webcamFrame.Height);
        CvInvoke.CvtColor(webcamFrame, hsv, ColorConversion.Bgr2Hsv);

        // Seui hsv
        Hsv borneInfGreen = new Hsv(bornInfGreen.x, bornInfGreen.y, bornInfGreen.z);
        Hsv borneSuppGreen = new Hsv(bornSuppGreen.x, bornSuppGreen.y, bornSuppGreen.z);

        Hsv borneInfBlue = new Hsv(bornInfBlue.x, bornInfBlue.y, bornInfBlue.z);
        Hsv borneSuppBlue = new Hsv(bornSuppBlue.x, bornSuppBlue.y, bornSuppBlue.z);

        Emgu.CV.Image<Gray, byte> treshHSVGreen = hsv.InRange(borneInfGreen, borneSuppGreen);
        treshHSVGreen = treshHSVGreen.Erode(3);
        treshHSVGreen = treshHSVGreen.Dilate(6);

        Emgu.CV.Image<Gray, byte> treshHSVBlue = hsv.InRange(borneInfBlue, borneSuppBlue);
        treshHSVBlue = treshHSVBlue.Erode(3);
        treshHSVBlue = treshHSVBlue.Dilate(6);

        Mat hier = new Mat();
        Emgu.CV.Util.VectorOfVectorOfPoint contoursGreen = new Emgu.CV.Util.VectorOfVectorOfPoint();
        Emgu.CV.Util.VectorOfVectorOfPoint contoursBlue = new Emgu.CV.Util.VectorOfVectorOfPoint();
        
        CvInvoke.FindContours(treshHSVGreen, contoursGreen, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
        CvInvoke.FindContours(treshHSVBlue, contoursBlue, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

        CvInvoke.DrawContours(webcamFrame, contoursGreen, 0, new MCvScalar(255, 0, 0), 2);
        CvInvoke.DrawContours(webcamFrame, contoursBlue, 0, new MCvScalar(255, 255, 255), 2);

        Moments mGreen = CvInvoke.Moments(treshHSVGreen);
        Moments mBlue = CvInvoke.Moments(treshHSVBlue);
        
        int cGx = (int)(mGreen.M10 / mGreen.M00);
        int cGy = (int)(mGreen.M01 / mGreen.M00);
        System.Drawing.Point pGreen = new System.Drawing.Point(cGx, cGy);
        CvInvoke.Circle(webcamFrame, pGreen, 10, new MCvScalar(255, 0, 0), 2);
        greenPos = new Vector2(cGx, cGy);

        int cRx = (int)(mBlue.M10 / mBlue.M00);
        int cRy = (int)(mBlue.M01 / mBlue.M00);
        System.Drawing.Point pBlue = new System.Drawing.Point(cRx, cRy);
        CvInvoke.Circle(webcamFrame, pBlue, 10, new MCvScalar(255, 255, 255), 2);
        bluePos = new Vector2(cRx, cRy);
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
