using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Physics settings")]
    [SerializeField]
    public Vector3 dir;
    [SerializeField]
    float cdColPlayerMax = 0.1f;
    [SerializeField]
    float cdColWallMax = 0.1f;
    [SerializeField]
    bool velocityImpactCalculation;
    [SerializeField]
    float minVel;

    [Header("Speed settings")]
    [SerializeField]
    float speedInit;
    [SerializeField]
    float speed;
    [SerializeField]
    public int hitsBeforeUp;
    [SerializeField]
    public int currentHits;
    [SerializeField]
    float speedMultiplier;
    [SerializeField]
    public int currentMultiplier;

    [Header("Score settings")]
    [SerializeField]
    public int score;

    [Header("Sounds settings")]
    [SerializeField]
    AudioClip playerSound;
    [SerializeField]
    AudioClip wallSound;
    AudioSource AS;
    [SerializeField]
    float pitchAdd = 0.1f;
    int pitchAddCount = 0;
    [SerializeField]
    float pitchComboTime = 0.5f;
    float pitchTime = 0.5f;

    [Header("Color settings")]
    [SerializeField]
    TrailRenderer TR;
    [SerializeField]
    List<Color> ballColors;
    [SerializeField]
    List<Gradient> trailGradients;

    GameManager GM;
    float cdColPlayer = 0.0f;
    float cdColWall = 0.0f;
    Rigidbody rb;
    public bool isDestroyed = false;
    Material mat;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        AS = GetComponent<AudioSource>();
        speed = speedInit;
        score = 1;
        GM.updateBallUI(this);
        mat = GetComponent<MeshRenderer>().material;
        mat.color = ballColors[currentMultiplier];
        TR.colorGradient = trailGradients[currentMultiplier];
    }

    void Update()
    {
        if (cdColPlayer > 0.0f)
            cdColPlayer -= Time.deltaTime;

        if (cdColWall > 0.0f)
            cdColWall -= Time.deltaTime;

        if (pitchTime > 0.0f)
            pitchTime -= Time.deltaTime;
        if (pitchTime < 0.0f)
            pitchAddCount = 0;
    }

    private void FixedUpdate()
    {
        rb.velocity = dir * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && cdColPlayer <= 0.0f)
        {
            addHit();
            AS.clip = playerSound;
            AS.Play();
            cdColPlayer = cdColPlayerMax;
            Rigidbody rbPlayer = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 playerCenter = new Vector3(collision.transform.position.x, 0.0f, collision.transform.position.z);
            //StartCoroutine(collision.gameObject.GetComponent<Player>().CP.shake(0.02f + 0.01f * currentMultiplier, 1.0f + 0.5f * currentMultiplier));
            collision.gameObject.GetComponent<Player>().CP.shake(0.02f + 0.01f * currentMultiplier, 1.0f + 0.5f * currentMultiplier);

            if (velocityImpactCalculation && rbPlayer.velocity.magnitude > minVel)
            {
                Vector3 n = transform.position - playerCenter;
                n.Normalize();

                float a1 = Vector3.Dot(rb.velocity, n);
                float a2 = Vector3.Dot(collision.gameObject.GetComponent<Rigidbody>().velocity, n);

                float optimizedP = Mathf.Min((2.0f * (a1 - a2)) / 2.0f, 0.0f);
                Vector3 v1 = rb.velocity - optimizedP * n;
                v1.y = 0.0f;
                dir = v1.normalized;
            }
            else
            {
                ContactPoint[] cp = new ContactPoint[collision.contactCount];
                collision.GetContacts(cp);
                Vector3 averageColPoint = Vector3.zero;
                foreach (var x in cp)
                    averageColPoint += x.point;
                averageColPoint /= collision.contactCount;
                //Vector3 averageColPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                Vector3 n = (transform.position - playerCenter).normalized;
                dir = (-2.0f * Vector3.Dot(dir, n) * n + dir);
                dir.y = 0;
                dir.Normalize();
            }
        }
        if (collision.gameObject.CompareTag("Wall") && cdColWall <= 0.0f)
        {
            addHit();
            AS.clip = wallSound;
            AS.Play();
            cdColWall = cdColWallMax;
            Vector3 n = collision.transform.GetComponent<Wall>().normal;
            dir = (-2.0f * Vector3.Dot(dir, n) * n + dir);
            dir.y = 0;
            dir.Normalize();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && cdColWall <= 0.0f)
        {
            //addHit();
            cdColWall = cdColWallMax;
            Vector3 n = collision.transform.GetComponent<Wall>().normal;
            dir = (-2.0f * Vector3.Dot(dir, n) * n + dir);
            dir.y = 0;
            dir.Normalize();
        }
    }

    void addHit()
    {
        pitchTime = pitchComboTime;
        pitchAddCount++;
        AS.pitch = Mathf.Min(1.0f + pitchAddCount * pitchAdd, 2.0f);
        StartCoroutine(GM.bumpBallUI());
        if (currentMultiplier < 3)
        {
            currentHits++;
            if (currentHits >= hitsBeforeUp)
            {
                currentHits = 0;
                currentMultiplier++;
                speed = speedInit + currentMultiplier * speedMultiplier;
                mat.color = ballColors[currentMultiplier];
                TR.colorGradient = trailGradients[currentMultiplier];
            }
            GM.updateBallUI(this);
        }
    }

}
