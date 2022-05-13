using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //projectile direction
    private Vector3 direction;

    //projectile bounces
    [SerializeField] private int bounces = 3;

    //particle fx and sounds
    [SerializeField] private GameObject sparks;
    [SerializeField] private GameObject blocks;

    void Start()
    {
        direction = transform.forward;
    }

    void Update()
    {
        //projectile only moves when in play mode
        if (GameController.state == GameController.PlayingState.Playing)
        {
            transform.position += direction * Time.deltaTime * 9f;
        }
        if (GameController.state == GameController.PlayingState.Menu)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(sparks, transform.position, transform.rotation);

        //if projectile hits the room, it bounces until it's destroyed
        if (collision.gameObject.CompareTag("Playground"))
        {
            Vector3 inDirection = transform.forward;
            Vector3 inNormal = collision.contacts[0].normal;

            direction = Vector3.Reflect(inDirection, inNormal).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 1000f);

            bounces -= 1;
            if (bounces <= 0)
            {
                Destroy(gameObject);
            }
        }

        //if projectile hits a target, both the target and the projectile are destroyed
        if (collision.gameObject.CompareTag("Cube"))
        {
            Destroy(collision.gameObject);

            Instantiate(blocks, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            Destroy(gameObject);
            FindObjectOfType<GameController>().TargetDestroyed(collision.gameObject.transform);
        }

    }
}
