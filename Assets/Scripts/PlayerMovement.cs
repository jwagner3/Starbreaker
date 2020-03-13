using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpSpeed = 30.0f;
    public float gravity = 55.0f;
    public float runSpeed = 70.0f;
    public float runSpeed1 = 70.0f;
    public float runSpeed2 = 140.0f;
    private float walkSpeed = 90.0f;
    private float rotateSpeed = 150.0f;

    public bool grounded;
    private Vector3 moveDirection = Vector3.zero;
    private bool isWalking;
    private string moveStatus = "idle";

    public GameObject camera1;
    public CharacterController controller;
    public bool isJumping;
    private float myAng = 0.0f;
    public bool canJump = true;

    public bool isFuture = true;
    public bool isPast;

    public GameObject[] fObjects;
    public GameObject[] pObjects;

    public Renderer[] fRenderer;
    public Renderer[] pRenderer;


    public GameObject[] audioRecordings;
    void Start()
    {

        controller = GetComponent<CharacterController>();
           isPast = false;
}

    void Update()
    {
      
        //force controller down slope. Disable jumping
        if (myAng > 50)
        {

            canJump = false;
        }
        else
        {

            canJump = true;
        }

        if (grounded)
        {

            isJumping = false;

            if (camera1.transform.gameObject.transform.GetComponent<UserCamera>().inFirstPerson == true)
            {
                moveDirection = new Vector3((Input.GetMouseButton(0) ? Input.GetAxis("Horizontal") : 0), 0, Input.GetAxis("Vertical"));
            }
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= isWalking ? walkSpeed : runSpeed;

            moveStatus = "idle";



            if (moveDirection != Vector3.zero)
                moveStatus = isWalking ? "walking" : "running";

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                moveDirection.y = jumpSpeed;
                isJumping = true;
            }

        }


        // Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down.


        if (camera1.transform.gameObject.transform.GetComponent<UserCamera>().inFirstPerson == false)
        {


            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            //else
            //{
            //    transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);

            //}
        }
        else
        {

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                camera1.transform.rotation = Quaternion.Euler(Input.mousePosition.x, Input.mousePosition.y, 0);
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            }

        }

        //Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;


        //Move controller
        CollisionFlags flags;
        if (isJumping)
        {
            flags = controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            flags = controller.Move((moveDirection + new Vector3(0, -100, 0)) * Time.deltaTime);
        }

        grounded = (flags & CollisionFlags.Below) != 0;
        audioRecordings = GameObject.FindGameObjectsWithTag("Audio Recording");
        if ( Input.GetButtonDown("Fire1"))
        {
            audioRecordings[0].GetComponent<AudioSource>().Play();
        }
        //Future Machine Controller
        fObjects = GameObject.FindGameObjectsWithTag("Future Object");
        pObjects = GameObject.FindGameObjectsWithTag("Past Object");
       
        if (Input.GetKeyDown(KeyCode.Q) && isFuture)
        {
            isFuture = false;
            isPast = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isPast)
        {
            isFuture = true;
            isPast = false;
        }
        if (isPast)
        {

            for (int i = 0; i < fObjects.Length; i++)
            {
                fObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                fObjects[i].gameObject.GetComponent<Collider>().enabled = false;

                
            }

            for (int i = 0; i < pObjects.Length; i++)
            {
                pObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                pObjects[i].gameObject.GetComponent<Collider>().enabled = true;
            }
        }
        else if (isFuture)
        {

            for (int i = 0; i < fObjects.Length; i++)
            {
                fObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                fObjects[i].gameObject.GetComponent<Collider>().enabled = true;
            }

            for (int i = 0; i < pObjects.Length; i++)
            {
                pObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                pObjects[i].gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        myAng = Vector3.Angle(Vector3.up, hit.normal);
    }
}
