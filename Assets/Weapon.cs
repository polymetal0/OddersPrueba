using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float recoil;

    private Rigidbody rb;

    public Transform cannon;
    public GameObject bullet;
    public GameObject shotParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("shoot");
        Instantiate(shotParticles, cannon.position, cannon.rotation);

        Instantiate(bullet, cannon.position, cannon.rotation);//.GetComponent<Rigidbody>();
        GetComponent<AudioSource>().Play();
        //rb.gameObject.transform.ro
        //rb.AddRelativeForce(Vector3.right, ForceMode.Impulse);


        //ApplyRecoil();
    }

    public void ApplyRecoil()
    {
        rb.AddRelativeForce(Vector3.left * recoil, ForceMode.Impulse);
    }
}
