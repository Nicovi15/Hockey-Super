using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

public class RecoVert : MonoBehaviour
{
    // Webcam
    private Emgu.CV.VideoCapture webcam;
    private Mat webcamFrame;

    public UnityEngine.UI.RawImage rawImage;
    public Texture2D tex;

    // Cascade classifier
    private CascadeClassifier frontFaceCascadeClassifier;

    // data needed for classifiers
    private System.Drawing.Rectangle[] frontFaces;
    private int MIN_FACE_SIZE = 50;
    private int MAX_FACE_SIZE = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
