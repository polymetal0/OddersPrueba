using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cannon;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("shoot");
        //Rigidbody rb = 
        Instantiate(bullet, cannon.position, cannon.rotation);//.GetComponent<Rigidbody>();
        //rb.gameObject.transform.ro
        //rb.AddRelativeForce(Vector3.right, ForceMode.Impulse);
    }

}
