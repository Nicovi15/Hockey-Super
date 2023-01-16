using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    TextMeshProUGUI p1Name;
    [SerializeField]
    TextMeshProUGUI p2Name;

    [Header("Music Settings")]
    [SerializeField]
    AudioSource backgroundMusic;
    [Header("Sounds Settings")]
    [SerializeField]
    AudioClip startSound;
    [SerializeField]
    AudioClip goalSound;
    [SerializeField]
    AudioSource mainTitleSound;

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
    [SerializeField]
    TMP_InputField scoreInput;
    [SerializeField]
    TMP_InputField pseudoP1;
    [SerializeField]
    TMP_InputField pseudoP2;
    [SerializeField]
    TMP_Dropdown inputP1;
    [SerializeField]
    TMP_Dropdown inputP2;
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject CameraMenu;

    [Header("Win Settings")]
    [SerializeField]
    GameObject WinObject;
    [SerializeField]
    TextMeshProUGUI wName;
    [SerializeField]
    GameObject RPgo;

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

    public void camCalibButtonClick()
    {
        MainMenu.SetActive(false);
        CameraMenu.SetActive(true);
    }

    public void backButtonClick()
    {
        MainMenu.SetActive(true);
        CameraMenu.SetActive(false);
    }

    public void startButtonClick()
    {
        if ((inputP1.value == 2 || inputP2.value == 2) && !RPgo.activeSelf)
            RPgo.SetActive(true);
        UImenu.SetActive(false);
        UIgame.SetActive(false);
        WinObject.SetActive(false);
        camAnim.SetTrigger("Start");
        player1.pseudo = pseudoP1.text;
        player2.pseudo = pseudoP2.text;
        player1.score = 0;
        player2.score = 0;
        player1.Reset();
        player2.Reset();
        p1Name.text = pseudoP1.text;
        p2Name.text = pseudoP2.text;
        player1.controller = inputP1.value == 0 ? Controller.Mouse : inputP1.value == 1 ? Controller.Keyboard : Controller.Webcam;
        player2.controller = inputP2.value == 0 ? Controller.Mouse : inputP2.value == 1 ? Controller.Keyboard : Controller.Webcam;
        scoreMax = int.Parse(scoreInput.text);
        mainTitleSound.Play();
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

    public IEnumerator startNewRound(Vector3 dir, Player p)
    {
        if (player1.score >= scoreMax || player2.score >= scoreMax)
        {
            showWinner(p);
            yield break;
        }

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
        StartCoroutine(startNewRound(dir, p));
    }

    public IEnumerator bumpBallUI()
    {
        fondBall.rectTransform.sizeDelta = initFondSize + tailleBump;
        ball.rectTransform.sizeDelta = initBallSize + tailleBump;
        yield return new WaitForSeconds(timeBump);
        fondBall.rectTransform.sizeDelta = initFondSize;
        ball.rectTransform.sizeDelta = initBallSize;
    }

    public void showWinner(Player p)
    {
        wName.text = p.pseudo;
        WinObject.SetActive(true);
    }

    public void menuClick()
    {
        SceneManager.LoadScene(0);
    }
}
