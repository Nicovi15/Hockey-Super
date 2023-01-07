using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    float timeDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball.isDestroyed)
                return;

            Debug.Log("Goal !");
            StartCoroutine(DestroyBall(timeDestroy, other.gameObject));
            ball.isDestroyed = true;
            player.Goal(ball.score);
        }
    }

    IEnumerator DestroyBall(float time, GameObject ball)
    {
        yield return new WaitForSeconds(time);
        Destroy(ball);
    }
}
