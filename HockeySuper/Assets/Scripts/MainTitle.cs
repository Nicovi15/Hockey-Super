using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainTitle : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Hockey;

    [SerializeField]
    TextMeshProUGUI Super;

    [SerializeField]
    float duree;

    [SerializeField]
    float normalSize;

    [SerializeField]
    float extraSize;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(textEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator textEffect()
    {
        Hockey.text = "<size=" + extraSize.ToString() + "%>HOCKEY";
        Super.text = "<size=" + normalSize.ToString() + "%>SUPER";

        yield return new WaitForSeconds(duree);

        Hockey.text = "<size=" + normalSize.ToString() + "%>HOCKEY";
        Super.text = "<size=" + extraSize.ToString() + "%>SUPER";

        yield return new WaitForSeconds(duree);
        StartCoroutine(textEffect());
    }
}
