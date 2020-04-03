using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpSpeed = 100.0f;
    public float gravity = 30f;
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
    private float myAng = 51.0f;
    public bool canJump = true;

    public bool isFuture = true;
    public bool isPast;

    public GameObject[] fObjects;
    public GameObject[] pObjects;

    public Renderer[] fRenderer;
    public Renderer[] pRenderer;

    public GameObject[] clues;
    public GameObject[] audioLogs;
    public bool audioLogPlaying = false;
    public bool allowAudioLog = true;
    public string readout;

    public int jetStrength = 30;

    void Start()
    {

        controller = GetComponent<CharacterController>();
        isPast = false;
    }


    private void OnGUI()
    {

    }

    void Update()
    {
        //if (GameObject.FindGameObjectWithTag("Audio Recording").gameObject.GetComponent<AudioSource>().isPlaying == true)
        //{
        //    audioLogPlaying = false;
        //    //allowAudioLog = false;
        //}
        //else
        //{
        //    allowAudioLog = true;
        //}
        //force controller down slope. Disable jumping

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveDirection.y = jetStrength;
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

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            for (int i = 0; i < clues.Length+1; i++)
            {
                if (hit.rigidbody == audioLogs[i].GetComponent<Rigidbody>() && Input.GetMouseButtonDown(0))
                {
                    audioLogs[i].gameObject.GetComponent<AudioSource>().Play();
                    break;
                }
            }
            for (int i = 0; i < clues.Length; i++)
                if (hit.rigidbody == clues[i])
                {
                    readout = clues[i].gameObject.GetComponent<string>();
                }
                else if (hit.rigidbody == audioLogs[i])
                {
                    Debug.Log("Hit");

                    break;
                }



            //    bullet.GetComponent<Rigidbody>().velocity = (_hit.point - transform.position).normalized * speed;
        }
        {

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
}

//    void OnControllerColliderHit(ControllerColliderHit hit)
//    {

//        myAng = Vector3.Angle(Vector3.up, hit.normal);
//        Debug.Log(myAng);
//    }
//}
