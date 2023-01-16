using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CamCalibration : MonoBehaviour
{
    [SerializeField]
    TMP_InputField hMinG;

    [SerializeField]
    TMP_InputField sMinG;

    [SerializeField]
    TMP_InputField vMinG;

    [SerializeField]
    TMP_InputField hMaxG;

    [SerializeField]
    TMP_InputField sMaxG;

    [SerializeField]
    TMP_InputField vMaxG;

    [SerializeField]
    TMP_InputField hMinB;

    [SerializeField]
    TMP_InputField sMinB;

    [SerializeField]
    TMP_InputField vMinB;

    [SerializeField]
    TMP_InputField hMaxB;

    [SerializeField]
    TMP_InputField sMaxB;

    [SerializeField]
    TMP_InputField vMaxB;

    [SerializeField]
    RecoPlayer RP;

    public void setFields()
    {
        hMinG.text = ((int)RP.bornInfGreen.x).ToString();
        sMinG.text = ((int)RP.bornInfGreen.y).ToString();
        vMinG.text = ((int)RP.bornInfGreen.z).ToString();
        hMaxG.text = ((int)RP.bornSuppGreen.x).ToString();
        sMaxG.text = ((int)RP.bornSuppGreen.y).ToString();
        vMaxG.text = ((int)RP.bornSuppGreen.z).ToString();
        hMinB.text = ((int)RP.bornInfBlue.x).ToString();
        sMinB.text = ((int)RP.bornInfBlue.y).ToString();
        vMinB.text = ((int)RP.bornInfBlue.z).ToString();
        hMaxB.text = ((int)RP.bornSuppBlue.x).ToString();
        sMaxB.text = ((int)RP.bornSuppBlue.y).ToString();
        vMaxB.text = ((int)RP.bornSuppBlue.z).ToString();
    }

    public void updateTresholds()
    {
        RP.bornInfGreen = new Vector3(int.Parse(hMinG.text), int.Parse(sMinG.text), int.Parse(vMinG.text));
        RP.bornSuppGreen = new Vector3(int.Parse(hMaxG.text), int.Parse(sMaxG.text), int.Parse(vMaxG.text));
        RP.bornInfBlue = new Vector3(int.Parse(hMinB.text), int.Parse(sMinB.text), int.Parse(vMinB.text));
        RP.bornSuppBlue = new Vector3(int.Parse(hMaxB.text), int.Parse(sMaxB.text), int.Parse(vMaxB.text));
    }

    public void enableCameraReco()
    {
        RP.gameObject.SetActive(true);
    }
}
