using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
//using OpenCVForUnity.Utils;

public class Webcam : MonoBehaviour
{
    // Déclarez une variable pour stocker l'image de la webcam
    private Mat webcamImage;

    // Déclarez une variable pour stocker la texture qui sera affichée dans Unity
    private Texture2D texture;

    // Déclarez une variable pour stocker le flux vidéo de la webcam
    private VideoCapture webcam;

    void Start()
    {
        // Initialisez le flux vidéo de la webcam
        webcam = new VideoCapture(0);

        // Initialisez la variable qui stockera l'image de la webcam
        webcamImage = new Mat();
    }

    void Update()
    {
        // Récupérez l'image de la webcam dans la variable webcamImage
        webcam.Retrieve(webcamImage);

        // Convertissez l'image de la webcam en texture
        texture = ConvertMatToTexture(webcamImage);

        // Appliquer la texture à l'objet Unity
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
    }

    // Fonction pour convertir une image Emgu CV en texture Unity
    private Texture2D ConvertMatToTexture(Mat mat)
    {
        // Créez une nouvelle texture Unity
        Texture2D texture = new Texture2D(mat.Width, mat.Height, TextureFormat.RGBA32, false);

        // Copiez les données de l'image Emgu CV dans la texture Unity
        //Utils.MatrixToTexture2D(mat, texture);

        // Retournez la texture
        return texture;
    }
}
