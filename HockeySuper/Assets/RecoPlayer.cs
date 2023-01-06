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

    static Emgu.CV.Image<Bgr, byte> copyToImage(Emgu.CV.Image<Bgr, byte> original, Emgu.CV.Image<Bgr, byte> copy, int xOffset = 0, int yOffset = 0)
    {
        int dimX = Mathf.Max(original.Width, xOffset + copy.Width);
        int dimY = Mathf.Max(original.Height, yOffset + copy.Height);
        Emgu.CV.Image<Bgr, byte> res = new Image<Bgr, byte>(dimX, dimY);

        for (int i = 0; i < original.Height; i++)
            for (int j = 0; j < original.Width; j++)
            {
                res.Data[i, j, 0] = original.Data[i, j, 0];
                res.Data[i, j, 1] = original.Data[i, j, 1];
                res.Data[i, j, 2] = original.Data[i, j, 2];
            }

        for (int i = 0; i < copy.Height; i++)
            for (int j = 0; j < copy.Width; j++)
            {
                res.Data[i + yOffset, j + xOffset, 0] = copy.Data[i, j, 0];
                res.Data[i + yOffset, j + xOffset, 1] = copy.Data[i, j, 1];
                res.Data[i + yOffset, j + xOffset, 2] = copy.Data[i, j, 2];
            }

        return res;
    }

    static Emgu.CV.Image<Hsv, byte> copyToImage(Emgu.CV.Image<Hsv, byte> original, Emgu.CV.Image<Hsv, byte> copy, int xOffset = 0, int yOffset = 0)
    {
        int dimX = Mathf.Max(original.Width, xOffset + copy.Width);
        int dimY = Mathf.Max(original.Height, yOffset + copy.Height);
        Emgu.CV.Image<Hsv, byte> res = new Image<Hsv, byte>(dimX, dimY);

        for (int i = 0; i < original.Height; i++)
            for (int j = 0; j < original.Width; j++)
            {
                res.Data[i, j, 0] = original.Data[i, j, 0];
                res.Data[i, j, 1] = original.Data[i, j, 1];
                res.Data[i, j, 2] = original.Data[i, j, 2];
            }

        for (int i = 0; i < copy.Height; i++)
            for (int j = 0; j < copy.Width; j++)
            {
                res.Data[i + yOffset, j + xOffset, 0] = copy.Data[i, j, 0];
                res.Data[i + yOffset, j + xOffset, 1] = copy.Data[i, j, 1];
                res.Data[i + yOffset, j + xOffset, 2] = copy.Data[i, j, 2];
            }

        return res;
    }

    static Emgu.CV.Image<Bgr, byte> copyToImage(Emgu.CV.Image<Bgr, byte> original, Emgu.CV.Image<Gray, byte> copy, int xOffset = 0, int yOffset = 0)
    {
        int dimX = Mathf.Max(original.Width, xOffset + copy.Width);
        int dimY = Mathf.Max(original.Height, yOffset + copy.Height);
        Emgu.CV.Image<Bgr, byte> res = new Image<Bgr, byte>(dimX, dimY);

        for (int i = 0; i < original.Height; i++)
            for (int j = 0; j < original.Width; j++)
            {
                res.Data[i, j, 0] = original.Data[i, j, 0];
                res.Data[i, j, 1] = original.Data[i, j, 1];
                res.Data[i, j, 2] = original.Data[i, j, 2];
            }

        for (int i = 0; i < copy.Height; i++)
            for (int j = 0; j < copy.Width; j++)
            {
                res.Data[i + yOffset, j + xOffset, 0] = copy.Data[i, j, 0];
                res.Data[i + yOffset, j + xOffset, 1] = copy.Data[i, j, 0];
                res.Data[i + yOffset, j + xOffset, 2] = copy.Data[i, j, 0];
            }

        return res;
    }

    static Emgu.CV.Image<Bgr, byte> GrayCopyChannelToImage(Emgu.CV.Image<Bgr, byte> original, Emgu.CV.Image<Bgr, byte> copy, int canal, int xOffset = 0, int yOffset = 0)
    {
        int dimX = Mathf.Max(original.Width, xOffset + copy.Width);
        int dimY = Mathf.Max(original.Height, yOffset + copy.Height);
        Emgu.CV.Image<Bgr, byte> res = new Image<Bgr, byte>(dimX, dimY);

        for (int i = 0; i < original.Height; i++)
            for (int j = 0; j < original.Width; j++)
            {
                res.Data[i, j, 0] = original.Data[i, j, 0];
                res.Data[i, j, 1] = original.Data[i, j, 1];
                res.Data[i, j, 2] = original.Data[i, j, 2];
            }

        for (int i = 0; i < copy.Height; i++)
            for (int j = 0; j < copy.Width; j++)
            {
                res.Data[i + xOffset, j + yOffset, 0] = copy.Data[i, j, canal];
                res.Data[i + xOffset, j + yOffset, 1] = copy.Data[i, j, canal];
                res.Data[i + xOffset, j + yOffset, 2] = copy.Data[i, j, canal];
            }

        return res;
    }

    static Emgu.CV.Image<Hsv, byte> HsvTreshold(Emgu.CV.Image<Hsv, byte> original, Hsv borneInf, Hsv borneSupp, Hsv minValue, Hsv maxValue)
    {
        Emgu.CV.Image<Hsv, byte> res = new Image<Hsv, byte>(original.Width, original.Height);

        for (int i = 0; i < original.Height; i++)
            for (int j = 0; j < original.Width; j++)
            {
                res.Data[i, j, 0] = original.Data[i, j, 0] < borneInf.Hue ? (byte)minValue.Hue : original.Data[i, j, 0] > borneSupp.Hue ? (byte)maxValue.Hue : original.Data[i, j, 0];
                res.Data[i, j, 1] = original.Data[i, j, 1] < borneInf.Satuation ? (byte)minValue.Satuation : original.Data[i, j, 1] > borneSupp.Satuation ? (byte)maxValue.Satuation : original.Data[i, j, 1];
                res.Data[i, j, 2] = original.Data[i, j, 2] < borneInf.Value ? (byte)minValue.Value : original.Data[i, j, 2] > borneSupp.Value ? (byte)maxValue.Value : original.Data[i, j, 2];
            }

        return res;
    }

    static Emgu.CV.Image<Hsv, byte> HsvTreshold(Emgu.CV.Image<Hsv, byte> original, Hsv borneInf, Hsv borneSupp)
    {
        Emgu.CV.Image<Hsv, byte> res = new Image<Hsv, byte>(original.Width, original.Height);

        for (int i = 0; i < original.Height; i++)
            for (int j = 0; j < original.Width; j++)
            {
                res.Data[i, j, 0] = original.Data[i, j, 0] < borneInf.Hue ? (byte)borneInf.Hue :
                                    original.Data[i, j, 0] > borneSupp.Hue ? (byte)borneSupp.Hue :
                                    original.Data[i, j, 0];
                res.Data[i, j, 1] = original.Data[i, j, 1] < borneInf.Satuation ? (byte)borneInf.Satuation :
                                    original.Data[i, j, 1] > borneSupp.Satuation ? (byte)borneSupp.Satuation :
                                    original.Data[i, j, 1];
                res.Data[i, j, 2] = original.Data[i, j, 2] < borneInf.Value ? (byte)borneInf.Value :
                                    original.Data[i, j, 2] > borneSupp.Value ? (byte)borneSupp.Value :
                                    original.Data[i, j, 2];
            }

        return res;
    }


    [SerializeField] 
    Vector3 bornInfGreen;
    [SerializeField]
    Vector3 bornSuppGreen;

    [SerializeField] 
    Vector3 bornInfRed;
    [SerializeField] 
    Vector3 bornSuppRed;


    private Emgu.CV.VideoCapture webcam;
    private Mat webcamFrame;
    public UnityEngine.UI.RawImage rawImage;
    Texture2D tex;

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
            findNewFace(webcamFrame);
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

        // HSV
        Emgu.CV.Image<Hsv, byte> hsv = new Emgu.CV.Image<Hsv, byte>(webcamFrame.Width, webcamFrame.Height);
        CvInvoke.CvtColor(webcamFrame, hsv, ColorConversion.Bgr2Hsv);
        CvInvoke.Imshow("Hsv", hsv);

        // Seui hsv
        Hsv borneInfGreen = new Hsv(bornInfGreen.x, bornInfGreen.y, bornInfGreen.z);
        Hsv borneSuppGreen = new Hsv(bornSuppGreen.x, bornSuppGreen.y, bornSuppGreen.z);

        Hsv borneInfRed = new Hsv(bornInfRed.x, bornInfRed.y, bornInfRed.z);
        Hsv borneSuppRed = new Hsv(bornSuppRed.x, bornSuppRed.y, bornSuppRed.z);

        //hsv = hsv.ThresholdTrunc(borneInf);
        //hsv = hsv.ThresholdBinary(borneInf, borneSupp);

        //hsv = HsvTreshold(hsv, borneInf, borneSupp);
        //hsv = HsvTreshold(hsv, borneInf, borneSupp);

        //hsv = HsvTreshold(hsv, borneInf, borneSupp);

        //hsv = hsv.InRange(borneInf, borneSupp);

        Emgu.CV.Image<Gray, byte> treshHSVGreen = hsv.InRange(borneInfGreen, borneSuppGreen);
        treshHSVGreen = treshHSVGreen.Erode(3);
        treshHSVGreen = treshHSVGreen.Dilate(6);

        Emgu.CV.Image<Gray, byte> treshHSVRed = hsv.InRange(borneInfRed, borneSuppRed);
        treshHSVRed = treshHSVRed.Erode(3);
        treshHSVRed = treshHSVRed.Dilate(6);


        //CvInvoke.FindContours(treshHSV, )

        //hsv = HsvTreshold(hsv, borneInf, borneSupp, minValue, maxValue);
        //hsv = hsv.ThresholdTrunc(borneInf);


        Mat hier = new Mat();
        Emgu.CV.Util.VectorOfVectorOfPoint contoursGreen = new Emgu.CV.Util.VectorOfVectorOfPoint();
        Emgu.CV.Util.VectorOfVectorOfPoint contoursRed = new Emgu.CV.Util.VectorOfVectorOfPoint();
        
        CvInvoke.FindContours(treshHSVGreen, contoursGreen, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
        CvInvoke.FindContours(treshHSVRed, contoursRed, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);


        CvInvoke.DrawContours(webcamFrame, contoursGreen, 0, new MCvScalar(255, 0, 0), 2);
        CvInvoke.DrawContours(webcamFrame, contoursRed, 0, new MCvScalar(255, 255, 255), 2);
        //
        //CvInvoke.Imshow("Hsv3", original);

        Moments mGreen = CvInvoke.Moments(treshHSVGreen);
        Moments mRed = CvInvoke.Moments(treshHSVRed);
        
        int cGx = (int)(mGreen.M10 / mGreen.M00);
        int cGy = (int)(mGreen.M01 / mGreen.M00);
        System.Drawing.Point pGreen = new System.Drawing.Point(cGx, cGy);
        CvInvoke.Circle(webcamFrame, pGreen, 10, new MCvScalar(255, 0, 0), 2);

        int cRx = (int)(mRed.M10 / mRed.M00);
        int cRy = (int)(mRed.M01 / mRed.M00);
        System.Drawing.Point pRed = new System.Drawing.Point(cRx, cRy);
        CvInvoke.Circle(webcamFrame, pRed, 10, new MCvScalar(255, 255, 255), 2);


        //CvInvoke.MinAreaRect(contours);
        //System.Drawing.Rectangle rect = CvInvoke.BoundingRectangle(contours);
        //System.Drawing.Point p1 = new System.Drawing.Point(rect.Right, rect.Bottom);
        //System.Drawing.Point p2 = new System.Drawing.Point(rect.Right, rect.Top);
        //System.Drawing.Point p3 = new System.Drawing.Point(rect.Left, rect.Bottom);
        //System.Drawing.Point p4 = new System.Drawing.Point(rect.Left, rect.Top);
        //CvInvoke.Line(original, p1, p2, new MCvScalar(255, 0, 0), 2);
        //CvInvoke.Line(original, p3, p4, new MCvScalar(255, 0, 0), 2);
        //CvInvoke.Line(original, p1, p3, new MCvScalar(255, 0, 0), 2);
        //CvInvoke.Line(original, p2, p4, new MCvScalar(255, 0, 0), 2);

        //CvInvoke.Imshow("Hsv4", webcamFrame);
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
