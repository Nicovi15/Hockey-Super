using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using UnityEngine.UI;


public class test : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image im;
    VideoCapture videoCapture;
    Mat frame;

    // Start is called before the first frame update
    void Start()
    {
        //Emgu.CV.Mat img = new Mat("C:\\Users\\nvivier\\Downloads\\PNG_transparency_demonstration_1.png");
       
        videoCapture = new VideoCapture(0, VideoCapture.API.DShow);
        videoCapture.Start();
        frame = new Mat();
        videoCapture.ImageGrabbed += HandleWebcamQueryFrame;
    }

    // Update is called once per frame
    void Update()
    {
        if (videoCapture.IsOpened)
        {
            videoCapture.Grab();
        }

        if (frame.Width > 0 && frame.Height > 0)
            CvInvoke.Imshow("Exercice 1", frame);
    }

    void HandleWebcamQueryFrame(object sender, System.EventArgs e)
    {
        videoCapture.Retrieve(frame, 0);
        Debug.Log(frame.Size);
    }

    private void OnDestroy()
    {
        videoCapture.Stop();
        videoCapture.Dispose();
    }

    void DisplayFrameOnPlane()
    {
        Texture2D text = new Texture2D(frame.Width, frame.Height);
        text.LoadRawTextureData(frame.DataPointer, frame.GetData().Length);
        //text.app
        //im.se
    }
}
