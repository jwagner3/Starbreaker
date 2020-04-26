using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float jumpSpeed = 100.0f;
    public float gravity = 30f;
    public float runSpeed = 70.0f;
    public float runSpeed1 = 70.0f;
    public float runSpeed2 = 140.0f;
    private float walkSpeed = 90.0f;
  

    public bool grounded;
    public Vector3 moveDirection = Vector3.zero;
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
    public Font mainText;
    public bool textOnScreen;
    public List<bool> messagesB = new List<bool>(10);
    public bool entryMessageB;
    public bool controlsMessageB;
    public bool clue1;
    public bool clue2;
    public bool warning1;
    public bool response1;
    public bool clue3;
    public bool clue4;
    public bool clue5;
    public List<string> messages = new List<string>(10);
    private string entryMessageS1 = "Press Q to go backwards and forwards in time. Left click to pick up objects and move the camera. Find the monster within the ship!";
    private string entryMessageS = "Spacetec Salutarian, you’ve served the Empire with grace and aplomb for many years, and we acknowledge your term of service is complete. We, however, require you for one final mission. Project Excelsior has proven to be a failure thus far, as all ships we’ve sent into the great beyond of the universe have failed to return. Until now. We received scattered distress beacons from the Roanoke a few cycles ago. By the time we’d found them, no life signs were left active on board.Please, discover what happened to the Roanoke’s crew and why they returned from what was supposed to be a one way voyage. The Empire of Life rests in your hands, Salutarian.Don’t fail us. -Project Excelsior Lead Astra Montreu";
    private string clue1S = "Hmm, it seems like this support beam collapsed on him. There are signs of a struggle within the room, indicating he was chased. He’s missing all of his flesh as well, which suggests perhaps a scavenger has made its way onto the ship. I’ll need to proceed with caution. I’ll need his key to investigate the reactor room, along with at least two other keys from various crew members.";
    private string clue2S = "It seems as if one of the pipes ruptured during a struggle. Chlorine seems to have killed this group of engineers, but they’re all missing their skin, which suggests the aggressor isn’t among their number. What sort of monster could do this?";
    private string warning1S = "Be advised, Salutarian, we're getting wildly varying readings from the reactor. If you stay on the ship much longer, we won't be able to guarentee your safety. -Project Excelsior Lead Astra Montreu";
    private string response1S = "There's only a few more rooms to search, and this might be our only chance to discover what really happened here.";
    public string clue3S;
    public string clue4S;
    public string clue5S;

    public Text readoutText;

    public Light flashLight;

    public float hp = 100;

    public GameObject entropy;

    public int keyCount = 0;

    void Start()
    {
        textStyle.wordWrap = true;
        textStyle.fontSize = 20;
        textStyle.font = mainText;
        messages.Add(entryMessageS);
        messages.Add(entryMessageS1);
        messages.Add(clue1S);
        messages.Add(clue2S);
        messages.Add(warning1S);
        messages.Add(response1S);
        messages.Add(clue3S);
        messages.Add(clue4S);
        messages.Add(clue4S);

        messagesB.Add(entryMessageB);
        messagesB.Add(controlsMessageB);
        messagesB.Add(clue1);
        messagesB.Add(clue2);
        messagesB.Add(warning1);
        messagesB.Add(response1);
        messagesB.Add(clue3);
        messagesB.Add(clue4);
        messagesB.Add(clue5);
        
      
        controller = GetComponent<CharacterController>();
        isPast = false;
        messagesB[0] = true;
    }


    private void OnGUI()
    {
        jetBarStyle.normal.background = jetFuelBar;
        jetBarStyle.normal.textColor = Color.green;
        GUI.Box(new Rect(260, 30, 30, jetFuel * 5), "Fuel: " + jetFuel, jetBarStyle);
        GUI.Box(new Rect(200, 30, 30, hp * 5), "Life: " + hp, jetBarStyle);

        
        //    if (messagesB[0])
        //    {
        //        GUI.Box(new Rect(700,300, 300, 150), messages[0], textStyle);
        //    StartCoroutine("TextDisappear");               
        //    }
        //if (messagesB[1])
        //{
        //    GUI.Box(new Rect(Screen.height / 2, Screen.width / 2, 50, 30), messages[1], textStyle);
        //    StartCoroutine("TextDisappear");
        //}
        
    }

    void Update()
    {
        if(Time.deltaTime > 20 && Time.deltaTime < 50)
        {
            messagesB[1] = true;
        }
        TimeMachine();
        Flashlight();
        //Check what messages are enabled and knows what to show the player
        for (int i = 0; i < messages.Count; i++)
        {
            if (messagesB[i])
            {
                Debug.Log(messagesB[i]);
                TextReadout(messages[i]);
                StartCoroutine("TextDisappear");
            }
        }
        if (hp <= 0)
        {
            SceneManager.LoadScene("Defeat");
        }
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
            moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        }

        //camera1.transform.gameObject.transform.GetComponent<UserCamera>().inFirstPerson = true;

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
            if (hit.rigidbody.name == "skeleton pillar" && Input.GetMouseButtonDown(0))
            {
                Debug.Log(messages[2]);
                messagesB[2] = true;

            }
            if(hit.rigidbody.name == "Crate" && Input.GetMouseButtonDown(0))
            {
                messagesB[1] = true;
            }
            if (hit.rigidbody.name == "Poison Skeleton" && Input.GetMouseButtonDown(0))
            {
                messagesB[3] = true;
            }
            if (hit.rigidbody.name == "Key 2" && Input.GetMouseButtonDown(0))
            {
                messagesB[4] = true;
            }
            if (hit.rigidbody.tag == "Key" && Input.GetMouseButtonDown(0))
            {
                Debug.Log("Hit key");
                Destroy(hit.rigidbody.gameObject);
                keyCount++;
            }
            if (messagesB[4])
            {
                StartCoroutine("ResponseOne");
                messagesB[5] = true;
            }

            //    bullet.GetComponent<Rigidbody>().velocity = (_hit.point - transform.position).normalized * speed;
        }
    }



    //Future Machine Controller
    //If you want to make an object past or future, it must have a collider and a mesh renderer

     public void Flashlight()
    {
        if (entropy.GetComponent<Entropy>().active != true)
        {
            if (Input.GetKeyDown(KeyCode.E) && flashLight.intensity > 0)
            {
                flashLight.intensity = 0;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                flashLight.intensity = 10;
            }
        }
        else
        {
            flashLight.intensity = 0;
        }
    }




    public void TimeMachine()
    {
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
                if (fObjects[i].gameObject.GetComponent<MeshRenderer>() != null)
                    fObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                if (fObjects[i].gameObject.GetComponent<Collider>() != null)
                    fObjects[i].gameObject.GetComponent<Collider>().enabled = false;
                if (fObjects[i].gameObject.GetComponent<ParticleSystem>() != null)
                {
                    fObjects[i].gameObject.GetComponent<ParticleSystem>().Stop();
                }


            }

            for (int i = 0; i < pObjects.Length; i++)
            {
                if (pObjects[i].gameObject.GetComponent<MeshRenderer>() != null)
                    pObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                if (pObjects[i].gameObject.GetComponent<Collider>() != null)
                    pObjects[i].gameObject.GetComponent<Collider>().enabled = true;
                if (pObjects[i].gameObject.GetComponent<ParticleSystem>() != null)
                {
                    pObjects[i].gameObject.GetComponent<ParticleSystem>().Play();
                }

            }
        }
        else if (isFuture)
        {

            for (int i = 0; i < fObjects.Length; i++)
            {
                if (fObjects[i].gameObject.GetComponent<MeshRenderer>() != null)
                    fObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                if (fObjects[i].gameObject.GetComponent<Collider>() != null)
                    fObjects[i].gameObject.GetComponent<Collider>().enabled = true;
                if (fObjects[i].gameObject.GetComponent<ParticleSystem>() != null)
                {
                    fObjects[i].gameObject.GetComponent<ParticleSystem>().Play();
                }
            }

            for (int i = 0; i < pObjects.Length; i++)
            {
                if (pObjects[i].gameObject.GetComponent<MeshRenderer>() != null)
                    pObjects[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                if (pObjects[i].gameObject.GetComponent<Collider>() != null)
                    pObjects[i].gameObject.GetComponent<Collider>().enabled = false;
                if (pObjects[i].gameObject.GetComponent<ParticleSystem>() != null)
                {
                    pObjects[i].gameObject.GetComponent<ParticleSystem>().Stop();
                }
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

        yield return new WaitForSecondsRealtime(10);

        for (int i = 0; i < messages.Count; i++)
        {
            if (messagesB[i]) { }
            
            messagesB[i] = false;
            readoutText.text = "";

           
        }
     }

    public IEnumerator ResponseOne()
    {
        yield return new WaitForSecondsRealtime(10.1f);
        
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Poison" && isFuture)
            hp -= Time.deltaTime * 5; ;

        if (other.gameObject.tag == "Entropy")
        {
            hp -= Time.deltaTime * 20;
        }

        if (other.tag == "Gravity Field")
        {
            gravity = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gravity Field")
        {
            gravity = 30;
        }
    }


    public void TextReadout(string text)
    {
        readoutText.text = text;
    }
}




//    void OnControllerColliderHit(ControllerColliderHit hit)
//    {

//        myAng = Vector3.Angle(Vector3.up, hit.normal);
//        Debug.Log(myAng);
//    }
//}
