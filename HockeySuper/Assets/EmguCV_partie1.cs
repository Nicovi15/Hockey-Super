using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

public class EmguCV_partie1 : MonoBehaviour
{
    // Webcam
    private Emgu.CV.VideoCapture webcam;
    private Mat webcamFrame;

    public UnityEngine.UI.RawImage rawImage;
    public Texture2D tex;

    private         CascadeClassifier frontFaceCascadeClassifier;
    private string  AbsPathToFrontCascadeClassifier = "D:\\L2\\github\\opencv\\data\\haarcascades\\haarcascade_frontalface_default.xml";

    private         System.Drawing.Rectangle[] frontFaces;
    private int     MIN_FACE_SIZE = 50;
    private int     MAX_FACE_SIZE = 200;

    // Start is called before the first frame update
    void Start()
    {
     
        Debug.Log("starting webcam");
        // Launch webcam capture
        // Manière Sans webcam (video)
        // webcam = new Emgu.CV.VideoCapture("D:\\L2\\Lyon2\\Aulas\\2021-2022\\OpenCV\\2021\\S5-S6\\video4.mp4");
        
        webcam = new Emgu.CV.VideoCapture("D:\\L2\\TD\\TP2\\demo.mp4");

        // Manière Avec Webcam (flux de la webcam)
        //webcam = new Emgu.CV.VideoCapture(0, VideoCapture.API.DShow);
        webcamFrame = new Mat();

        // Add event handler to the webcam
        //        webcam.ImageGrabbed += new System.EventHandler(HandleWebcamQueryFrame);
        // Demarrage de la webcam
        webcam.ImageGrabbed += HandleWebcamQueryFrame;
        webcam.Start();

        //initilisation du classificateur
        frontFaceCascadeClassifier = new CascadeClassifier(AbsPathToFrontCascadeClassifier);


    }
    private void HandleWebcamQueryFrame(object sender, System.EventArgs e)
    {
        if (webcam.IsOpened)
        {
            webcam.Retrieve(webcamFrame);
            Debug.Log("Test"+ webcamFrame.Size);
        }

        lock (webcamFrame)
        {
            Mat matGrayscale = new Mat(webcamFrame.Width, webcamFrame.Height, DepthType.Cv8U, 1);
            CvInvoke.CvtColor(webcamFrame, matGrayscale, ColorConversion.Bgr2Gray);

            // Face detection
            frontFaces = frontFaceCascadeClassifier.DetectMultiScale(matGrayscale,
                                                1.1,
                                                3,
                                                new System.Drawing.Size(MIN_FACE_SIZE, MIN_FACE_SIZE),
                                                new System.Drawing.Size(MAX_FACE_SIZE, MAX_FACE_SIZE));
            Debug.Log("number of detected faces:" + frontFaces.Length);

            // drawing detection rectangles

            for (int i = 0; i < frontFaces.Length; i++)
            {
                CvInvoke.Rectangle(webcamFrame, frontFaces[i], new MCvScalar(255, 255, 0), 5);
            }
        }

        // making the thread sleep so that things are not happening too fast. might be optional.
        System.Threading.Thread.Sleep(200);
    }
        // Update is called once per frame
    void Update()
    {
        if (webcam.IsOpened) {
           //Debug.Log("on est pret");
            webcam.Grab();
        }
        DisplayFrameOnPlane();
    }
    void OnDestroy()
    {
        Debug.Log("entering destroy");

        if (webcam != null)
        {

            Debug.Log("sleeping");
            //waiting for thread to finish before disposing the camera...(took a while to figure out)
            //System.Threading.Thread.Sleep(60);
            // close camera
            webcam.Stop();
            webcam.Dispose();
        }

        Debug.Log("Destroying webcam");
    }

    private void DisplayFrameOnPlane()
    {
        if (webcamFrame.IsEmpty) return;

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
}
