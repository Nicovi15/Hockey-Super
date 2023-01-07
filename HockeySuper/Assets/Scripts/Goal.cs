using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.isDestroyed)
                return;

            Debug.Log("Goal !");
            StartCoroutine(DestroyBall(0.3f, other.gameObject));
            ball.isDestroyed = true;
            player.Goal();
        }
    }

    IEnumerator DestroyBall(float time, GameObject ball)
    {
        yield return new WaitForSeconds(time);
        Destroy(ball);
    }
}
