using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpSpeed = 100.0f;
    public float gravity = 30f;
    public float runSpeed = 70.0f;
    public float runSpeed1 = 70.0f;
    public float runSpeed2 = 140.0f;
    private float walkSpeed = 90.0f;
  

    public bool grounded;
    private Vector3 moveDirection = Vector3.zero;
    private bool isWalking;
    private string moveStatus = "idle";

    public GameObject camera1;
    public CharacterController controller;
    public bool isJumping;
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
    public int jetFuel = 100;
    public Texture2D jetFuelBar;
    public GUIStyle jetBarStyle;

    public ParticleSystem jetParticles;

    //Text bools
    public GameObject canvas;
    public Scene clueFinder;
    public GUIStyle textStyle;
    public bool textOnScreen = false;
    public bool entryMessageB = true;
    public List<bool> messagesB = new List<bool>(10);
    public bool clue1;
    public bool clue2;
    public bool clue3;
    public bool clue4;
    public bool clue5;
    public List<string> messages = new List<string>(10);
    public string entryMessageS = "Welcome, this is the entry message";
    public string clue1S = "This seems to be the skeleton of the captain. Not event space barracudas could strip flesh so quickly";
    public string clue2S;
    public string clue3S;
    public string clue4S;
    public string clue5S;

    void Start()
    {
     
        messages.Add(entryMessageS);
        messages.Add(clue1S);
        messages.Add(clue2S);
        messages.Add(clue3S);
        messages.Add(clue4S);
        messages.Add(clue4S);

        messagesB.Add(entryMessageB);
        messagesB.Add(clue1);
        messagesB.Add(clue2);
        messagesB.Add(clue3);
        messagesB.Add(clue4);
        messagesB.Add(clue5);
        
      
        controller = GetComponent<CharacterController>();
        isPast = false;
    }


    private void OnGUI()
    {
        jetBarStyle.normal.background = jetFuelBar;

        GUI.Box(new Rect(260, 30, 30, jetFuel * 5), "Fuel: " + jetFuel, jetBarStyle);

        
            if (messagesB[0])
            {
                GUI.Box(new Rect(Screen.height/2, Screen.width/2, 50, 30), messages[0], textStyle);
            StartCoroutine("TextDisappear");               
            }
        if (messagesB[1])
        {
            GUI.Box(new Rect(Screen.height / 2, Screen.width / 2, 50, 30), messages[1], textStyle);
            StartCoroutine("TextDisappear");
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            { 

                for (int i = 0; i > messagesB.Count; i++)
                {
                    if (messagesB[i])
                    {
                        messagesB[i] = false;
                    }
                }
    }

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

        if (Input.GetKeyDown(KeyCode.LeftShift) && jetFuel > 0)
        {
            moveDirection.y = jetStrength;
            jetFuel -= 5;
            StartCoroutine("Jetpack");
        }
        if (!grounded)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveDirection.x -= jetStrength;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveDirection.x += jetStrength;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDirection.z += jetStrength;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveDirection.z -= jetStrength;
            }
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
            for (int i = 0; i < clues.Length + 1; i++)
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
            if (hit.rigidbody.name == "skeleton" && Input.GetMouseButtonDown(0))
            {
                Debug.Log(messages[1]);
                messagesB[1] = true;
                OnGUI();
            }


            //    bullet.GetComponent<Rigidbody>().velocity = (_hit.point - transform.position).normalized * speed;
        }
        {

        }
        //Future Machine Controller
        //If you want to make an object past or future, it must have a collider and a mesh renderer
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

    public IEnumerator Jetpack()
    {
        jetParticles.Play();
        yield return new WaitForSecondsRealtime(.5f);
        jetParticles.Stop();
    }

    public IEnumerator TextDisappear()
    {

        yield return new WaitForSecondsRealtime(.5f);

        for (int i = 0; i < messages.Count; i++)
        {
            if (messagesB[i]) { }
            messagesB[i] = false;

           
        }
        }


    }




//    void OnControllerColliderHit(ControllerColliderHit hit)
//    {

//        myAng = Vector3.Angle(Vector3.up, hit.normal);
//        Debug.Log(myAng);
//    }
//}
