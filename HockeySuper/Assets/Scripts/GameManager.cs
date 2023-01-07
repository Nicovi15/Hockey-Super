using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField]
    TextMeshProUGUI scoreBall;
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

    [Header("Game Settings")]
    [SerializeField]
    int scoreMax;
    [SerializeField]
    GameObject ballPrefab;
    [SerializeField]
    Player player1;
    [SerializeField]
    Player player2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateBallUI(Ball b)
    {
        scoreBall.text = b.score.ToString();
        fondBallJauge.color = ballColors[b.currentMultiplier];
        ball.color = ballColors[b.currentMultiplier];
        if(b.currentMultiplier < 3)
            ballJauge.color = ballColors[b.currentMultiplier + 1];
        ballJauge.fillAmount = (((float)b.currentHits) / ((float)b.hitsBeforeUp)) * 0.8f;
    }


    IEnumerator startGame()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(spawnNewBall());
    }

    public IEnumerator startNewRound(Vector3 dir)
    {
        if (player1.score >= scoreMax)
            Debug.Log("P1 win");
        else if (player2.score >= scoreMax)
            Debug.Log("P2 win");

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(spawnNewBall(false, dir));
    }

    public IEnumerator spawnNewBall(bool random = true, Vector3 dir = new Vector3())
    {
        Ball ball = Instantiate(ballPrefab).GetComponent<Ball>();
        ball.dir = Vector3.zero;
        yield return new WaitForSeconds(1.0f);
        if (random)
            ball.dir = Random.Range(0, 2) == 0 ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);
        else
            ball.dir = dir;
    }
}
