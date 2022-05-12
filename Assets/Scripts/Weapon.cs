using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float recoil;

    //private Rigidbody rb;

    [SerializeField] private Transform cannon;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shotParticles;


    private XRInteractorLineVisual pointer;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponentInChildren<Rigidbody>();
        pointer = GetComponentInParent<XRInteractorLineVisual>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.state == GameController.PlayingState.Playing && pointer.enabled)
        {
            pointer.enabled = false;
        }
        if (GameController.state != GameController.PlayingState.Playing && !pointer.enabled)
        {
            pointer.enabled = true;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (GameController.state == GameController.PlayingState.Playing && context.performed)
        {
            Debug.Log("shoot");
            Instantiate(shotParticles, cannon.position, cannon.rotation);

            Instantiate(bullet, cannon.position, cannon.rotation);
            GetComponent<AudioSource>().Play();

            //Recoil();

        }

    }

    /*public void Recoil()
    {
        rb.AddRelativeForce(Vector3.back * recoil, ForceMode.Impulse);
    }*/
}
