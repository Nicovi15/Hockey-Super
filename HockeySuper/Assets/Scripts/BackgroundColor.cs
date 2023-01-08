using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    [SerializeField]
    Camera cam1;
    [SerializeField]
    Camera cam2;
    [SerializeField]
    float h, s, v, vitesse;
    void Update()
    {
        h += vitesse * Time.deltaTime;
        if (h > 1)
            h = 0;
        cam1.backgroundColor = Color.HSVToRGB(h, s, v);
        cam2.backgroundColor = Color.HSVToRGB(h, s, v);
    }
}
