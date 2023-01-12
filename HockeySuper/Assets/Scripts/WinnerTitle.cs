using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerTitle : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Tm1;

    [SerializeField]
    TextMeshProUGUI Tm2;

    [SerializeField]
    float duree;

    [SerializeField]
    float normalSize;

    [SerializeField]
    float extraSize;

    string text1;
    string text2;

    Coroutine co;

    private void Awake()
    {
        text1 = Tm1.text;
        text2 = Tm2.text;
    }

    private void OnEnable()
    {
        co = StartCoroutine(textEffect());
    }

    private void OnDisable()
    {
        StopCoroutine(co);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator textEffect()
    {
        Tm1.text = "<size=" + extraSize.ToString() + "%>" + text1;
        Tm2.text = "<size=" + normalSize.ToString() + "%>" + text2;

        yield return new WaitForSeconds(duree);

        Tm1.text = "<size=" + normalSize.ToString() + "%>" + text1;
        Tm2.text = "<size=" + extraSize.ToString() + "%>" + text2;

        yield return new WaitForSeconds(duree);
        StartCoroutine(textEffect());
    }
}
