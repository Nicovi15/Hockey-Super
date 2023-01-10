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
    [SerializeField]
    Animator ballAnim;
    [SerializeField]
    float timeBump;
    [SerializeField]
    Vector2 tailleBump;
    [SerializeField]
    Animator goalAnim;
    [SerializeField]
    TextMeshProUGUI goalText;
    [SerializeField]
    Color colorP1;
    [SerializeField]
    Color colorP2;

    [Header("Music Settings")]
    [SerializeField]
    AudioSource backgroundMusic;
    [Header("Sounds Settings")]
    [SerializeField]
    AudioClip startSound;
    [SerializeField]
    AudioClip goalSound;

    [Header("Game Settings")]
    [SerializeField]
    int scoreMax;
    [SerializeField]
    GameObject ballPrefab;
    [SerializeField]
    Player player1;
    [SerializeField]
    Player player2;

    Vector2 initFondSize;
    Vector2 initBallSize;
    AudioSource AS;

    [Header("Menu Settings")]
    [SerializeField]
    GameObject UImenu;
    [SerializeField]
    GameObject UIgame;
    [SerializeField]
    Animator camAnim;

    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        initFondSize = fondBall.rectTransform.sizeDelta;
        initBallSize = ball.rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startButtonClick()
    {
        UImenu.SetActive(false);
        camAnim.SetTrigger("Start");
        StartCoroutine(startCoroutine());
    }

    IEnumerator startCoroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(camAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length * 3.0f + 0.3f);
        StartCoroutine(startGame());
        UIgame.SetActive(true);
    }

    public void updateBallUI(Ball b)
    {
        backgroundMusic.pitch = 1.0f + 0.025f * b.currentMultiplier;
        scoreBall.text = b.score.ToString();
        fondBallJauge.color = ballColors[b.currentMultiplier];
        ball.color = ballColors[b.currentMultiplier];
        if(b.currentMultiplier < 3)
            ballJauge.color = ballColors[b.currentMultiplier + 1];
        ballJauge.fillAmount = (((float)b.currentHits) / ((float)b.hitsBeforeUp)) * 0.8f;
    }


    IEnumerator startGame()
    {
        yield return new WaitForSeconds(1.0f);
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
        ballAnim.SetTrigger("Next");
        Ball ballO = Instantiate(ballPrefab).GetComponent<Ball>();
        ballO.dir = Vector3.zero;
        AS.clip = startSound;
        AS.Play();
        yield return new WaitForSeconds(1.0f);
        if (random)
            ballO.dir = Random.Range(0, 2) == 0 ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);
        else
            ballO.dir = dir;
    }

    public IEnumerator Goal(Player p, Vector3 dir)
    {
        if (p == player1)
            goalText.color = colorP1;
        else
            goalText.color = colorP2;
        AS.clip = goalSound;
        AS.Play();
        player1.CP.shake(0.1f, 6.0f);
        player2.CP.shake(0.1f, 6.0f);
        ballAnim.SetTrigger("Next");
        yield return new WaitForSeconds(0.2f);
        goalAnim.SetTrigger("Play");
        yield return new WaitForSeconds(goalAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        yield return new WaitForSeconds(0.5f);
        fondBall.rectTransform.sizeDelta = initFondSize;
        ball.rectTransform.sizeDelta = initBallSize;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(startNewRound(dir));
    }

    public IEnumerator bumpBallUI()
    {
        fondBall.rectTransform.sizeDelta = initFondSize + tailleBump;
        ball.rectTransform.sizeDelta = initBallSize + tailleBump;
        yield return new WaitForSeconds(timeBump);
        fondBall.rectTransform.sizeDelta = initFondSize;
        ball.rectTransform.sizeDelta = initBallSize;
    }
}
