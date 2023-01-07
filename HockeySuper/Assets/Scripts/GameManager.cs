using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField]
    Image fondBall;
    [SerializeField]
    Image ball;
    [SerializeField]
    Image fondBallJauge;
    [SerializeField]
    Image ballJauge;
    [SerializeField]
    public List<Color> ballColors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateBallUI(Ball b)
    {
        fondBallJauge.color = ballColors[b.currentMultiplier];
        ball.color = ballColors[b.currentMultiplier];
        if(b.currentMultiplier < 3)
            ballJauge.color = ballColors[b.currentMultiplier + 1];
        ballJauge.fillAmount = (((float)b.currentHits) / ((float)b.hitsBeforeUp)) * 0.8f;
    }
}
