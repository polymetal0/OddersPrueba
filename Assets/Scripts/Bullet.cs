using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private int bounces = 3;
    [SerializeField] private GameObject sparks;
    [SerializeField] private GameObject blocks;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.state == GameController.PlayingState.Playing)
        {
            transform.position += direction * Time.deltaTime * 9f;// * 1000f;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(sparks, transform.position, transform.rotation);

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

        if (collision.gameObject.CompareTag("Cube"))
        {
            Destroy(collision.gameObject);

            Instantiate(blocks, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            Destroy(gameObject);
            FindObjectOfType<GameController>().TargetDestroyed(collision.gameObject.transform.position);
        }

    }
}
