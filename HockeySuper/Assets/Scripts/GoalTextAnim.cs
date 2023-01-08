using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalTextAnim : MonoBehaviour
{
    [SerializeField]
    string titre;

    [SerializeField]
    float tailleNormale;

    [SerializeField]
    float tailleGrande;

    [SerializeField]
    float dureeOversize;

    TextMeshProUGUI tm;
    Coroutine co;

    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();

    }

    public void startEffect()
    {
        co = StartCoroutine(effetOversizeCharacter());
    }

    public void stopEffect()
    {
        StopCoroutine(co);
        tm.text = titre;
    }

    IEnumerator effetOversizeCharacter()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                tm.text = titre.Replace(titre[i].ToString(), "<size=" + tailleGrande.ToString() + "%>" + titre[i] + "<size=" + tailleNormale.ToString() + "%>");
                yield return new WaitForSeconds(dureeOversize);
            }
            tm.text = titre;
            yield return new WaitForSeconds(dureeOversize);
        }
        
    }
}
