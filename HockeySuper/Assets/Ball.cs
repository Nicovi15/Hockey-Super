using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Physics settings")]
    [SerializeField]
    float speed;
    [SerializeField]
    Vector3 dir;
    [SerializeField]
    const float cdColPlayerMax = 0.1f;
    [SerializeField]
    bool velocityImpactCalculation;
    [SerializeField]
    float minVel;

    float cdColPlayer = 0.0f;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (cdColPlayer > 0.0f)
            cdColPlayer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.velocity = dir * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && cdColPlayer <= 0.0f)
        {
            cdColPlayer = cdColPlayerMax;
            Rigidbody rbPlayer = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 playerCenter = new Vector3(collision.transform.position.x, 0.0f, collision.transform.position.z);

            if(velocityImpactCalculation && rbPlayer.velocity.magnitude > minVel)
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
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 n = collision.transform.GetComponent<Wall>().normal;
            dir = (-2.0f * Vector3.Dot(dir, n) * n + dir);
            dir.y = 0;
            dir.Normalize();
        }
    }
}
