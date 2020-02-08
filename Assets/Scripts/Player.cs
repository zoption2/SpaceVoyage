using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform hands; // руки с оружием нашего ГГ    
    public float FlySpeed;
    private float moveSpeed = 2f;
    private float moveInput;
    public Rigidbody2D playerRB;
    private Vector2 moveVelocity;
    private Vector2 moveOnPlanetVelocity;
    public Transform player;
    private SpriteRenderer playerFlip;
    Vector2 playerPos;
    Vector2 cursorPos;
    HingeJoint2D angler;
    private Rigidbody2D gamer;
    public Transform SpaceObjects;
    public Rigidbody2D pointOfGravity;
    private bool gravityIsActive;
    public GameObject target;
    private SpriteRenderer _target;
    private float rotateZ;

    private Vector2 fromPlayerToMouse;
    private Vector2 fromPlayerToUp;
    private Animator playerAnimation;
    private ParticleSystem jetPack;
    private ParticleSystem jetPackVertical;
    private GameObject jetPackVector;
    private GameObject jetPackVectorVertical;
    private GameObject o2FillBar;
    public Image fillBar;
    public float fillOxygen;
    private bool fromPlanetToSpace;
    private int state;

    void Start()
    {
        
        hands = transform.Find("hands"); //руки это дочерний обджект основного плеера
        player = transform.Find("idle");
        playerRB = player.GetComponent<Rigidbody2D>();
        playerFlip = player.GetComponent<SpriteRenderer>();
        angler = player.GetComponent<HingeJoint2D>();
        gamer = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<Animator>();
        jetPackVector = GameObject.Find("jetPack");
        jetPack = jetPackVector.GetComponent<ParticleSystem>();
        jetPackVectorVertical = GameObject.Find("jetPackVertical");
        jetPackVertical = jetPackVectorVertical.GetComponent<ParticleSystem>();
        gravityIsActive = false;
        o2FillBar = GameObject.Find("Canvas");
        fillBar = o2FillBar.GetComponentInChildren<Image>();
        fillOxygen = 1f;
    }

    void Update()
    {
        playerMove();
        playerFlipper();
        lookAtCursor();
        jumpOnPlanet();
        playerFlipperOnPlanet();
        oxigen();
        
    }

    private void FixedUpdate()
    {
        planetGravityEffect();
        playerMoveOnPlanet();
        jetPackVerticalWork();
        jetPackHorizontalWork();
    }
    private void LateUpdate()
    {
        rotateToSpace();

    }

    private void playerFlipper()
    {
        if (gravityIsActive == false)
        {
            SpriteRenderer weaponFlip = hands.GetComponent<SpriteRenderer>();
            playerPos = player.transform.position;
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (cursorPos.x > playerPos.x)
            {
                playerFlip.flipX = true;
                weaponFlip.flipY = false;
            }
            else
            {
                playerFlip.flipX = false;
                weaponFlip.flipY = true;
            }
        }
    }
    private void playerFlipperOnPlanet()
    {
        if ( gravityIsActive == true)
        {
            SpriteRenderer weaponFlip = hands.GetComponent<SpriteRenderer>();
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Transform sG = SpaceObjects.Find("Texture");
            
            Vector2 spaceGround = new Vector2(sG.right.x, sG.right.y);
            fromPlayerToMouse = new Vector2(cursorPos.x - SpaceObjects.position.x, cursorPos.y - SpaceObjects.position.y);
            fromPlayerToUp = new Vector2(transform.position.x - SpaceObjects.position.x, transform.position.y - SpaceObjects.position.y);

            float cursorAngle = Vector2.Angle(spaceGround, fromPlayerToMouse);
            float playerAngle = Vector2.Angle(spaceGround, fromPlayerToUp);
            
            if (transform.position.y >= SpaceObjects.position.y)
            {
                if (playerAngle >= cursorAngle)
                {
                    playerFlip.flipX = true;
                    weaponFlip.flipY = false;
                    if (cursorPos.y < SpaceObjects.position.y && transform.position.x < SpaceObjects.position.x)
                    {
                        playerFlip.flipX = false;
                        weaponFlip.flipY = true;
                    }
                }
                else if (playerAngle < cursorAngle)
                {
                    playerFlip.flipX = false;
                    weaponFlip.flipY = true;
                    
                    if (cursorPos.y < SpaceObjects.position.y && transform.position.x > SpaceObjects.position.x)
                    {
                        playerFlip.flipX = true;
                        weaponFlip.flipY = false;
                    }
                }
            }
            else
            {
                spaceGround = new Vector2(-sG.right.x, sG.right.y);
                if (playerAngle >= cursorAngle)
                {
                    playerFlip.flipX = false;
                    weaponFlip.flipY = true;
                    if (cursorPos.y > SpaceObjects.position.y && transform.position.x < SpaceObjects.position.x)
                    {
                        playerFlip.flipX = true;
                        weaponFlip.flipY = false;
                    }
                }
                if (playerAngle < cursorAngle)
                {
                    playerFlip.flipX = true;
                    weaponFlip.flipY = false;

                    if (cursorPos.y > SpaceObjects.position.y && transform.position.x > SpaceObjects.position.x)
                    {
                        playerFlip.flipX = false;
                        weaponFlip.flipY = true;
                    }
                }
            }
        }
    }
    private void lookAtCursor() //вращение рук за курсором
    {

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hands.transform.position;
        rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        hands.transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);
    }
    private void playerMove()
    {
        if (gravityIsActive == false)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveVelocity = moveInput * FlySpeed;
            gamer.AddForce((moveInput) * Time.fixedDeltaTime * 100);
            playerAnimation.SetBool("isLanding", false);

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                fillOxygen -= 0.01f * Time.deltaTime; 
                if (FlySpeed < 3f)
                {
                    FlySpeed += Time.deltaTime;
                }
            }
            else
            {
                FlySpeed = 0;
                jetPack.Stop();
            }
        }
    }
    private void playerMoveOnPlanet()
    {
        bool isWalkNow = false;
        bool onTopSide = false;
        

        if (gravityIsActive == true)
        {
            Vector3 down = Vector3.Project(gamer.velocity, transform.forward);
            Vector3 forward = transform.right * Input.GetAxis("Horizontal") * 4;

           
            if (Input.GetAxis("Horizontal") != 0 )
            {
                isWalkNow = true;
                playerAnimation.SetBool("isRunning", true);
            }
            else
            {
                isWalkNow = false;
                playerAnimation.SetBool("isRunning", false);
            }
            if (transform.position.y >= SpaceObjects.position.y)
            {
                onTopSide = true;
            }
            else
            {
                onTopSide = false;
            }
            if (Input.GetAxis("Horizontal") == 0 && onTopSide == true)
            {
                state = 0;
            }
            if (Input.GetAxis("Horizontal") == 0 && onTopSide == false)
            {
                state = 1;
            }

            if (state == 0)
            {
                if (isWalkNow == true && Input.GetAxis("Horizontal") != 0)
                {
                    gamer.velocity = down + forward;
                }
                else if (isWalkNow == true && onTopSide == false)
                {
                    gamer.velocity = down + forward;
                }
            }
            if (state == 1)
            {
                if (isWalkNow == true && Input.GetAxis("Horizontal") != 0)
                {
                    gamer.velocity = down - forward;
                }
                else if (isWalkNow == true && onTopSide == false)
                {
                    gamer.velocity = down - forward;
                }
            }
            



        }

        
    }
    private void jumpOnPlanet()
    {
        if (gravityIsActive == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                gamer.AddForce(transform.up * 1500f);
                jetPackVectorVertical.transform.rotation = Quaternion.FromToRotation(-transform.up, SpaceObjects.position - transform.position); ;
                jetPackVertical.Play();
            }
        }
    }
    private void JetPackOverdrive()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if ( Input.GetKey(KeyCode.Space))
        {

        }

    }
    private void OnTriggerStay2D (Collider2D graviator)
    {
        if (graviator.gameObject.tag == "SpaceObject" )
        {
            pointOfGravity = graviator.GetComponent<Rigidbody2D>();
            SpaceObjects = graviator.GetComponent<Transform>();
            gravityIsActive = true;
            fromPlanetToSpace = false;
        }
    }
    private void OnTriggerExit2D (Collider2D graviator)
    {
        fromPlanetToSpace = true;
        pointOfGravity = null;
        SpaceObjects = null;
        gravityIsActive = false;
        playerAnimation.SetBool("isRunning", false);
    }
    private void rotateToSpace()
    {
        if (fromPlanetToSpace == true)
        {
            Quaternion goTrans = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goTrans, 2f);

            if (transform.rotation == goTrans)
            {
                fromPlanetToSpace = false;
            }
        }
    }
    private void planetGravityEffect()
    {
        if (gravityIsActive == true)
        {
            Quaternion rotation = Quaternion.FromToRotation(-transform.up, SpaceObjects.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation*transform.rotation, 10f);
            playerAnimation.SetBool("isLanding", true);
        }
    }
    private void jetPackHorizontalWork()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && gravityIsActive ==false)
        {
            jetPack.Play();
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                jetPackVector.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                jetPackVector.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            }
        }
        else
        {
            jetPack.Stop();
        }
    }
    private void jetPackVerticalWork()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && gravityIsActive ==false)
        {
            jetPackVertical.Play();
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                jetPackVectorVertical.transform.rotation = Quaternion.Euler(90f, 0f, 90f);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                jetPackVectorVertical.transform.rotation = Quaternion.Euler(-90f, 0f, 90f);
            }
        }
        else
        {
            jetPackVertical.Stop();
        }
    }

    public void oxigen()
    {
        fillOxygen -= 0.01f * Time.deltaTime; 
        fillBar.fillAmount = fillOxygen;
        if (fillOxygen > 1)
        {
            fillOxygen = 1;
        }
    }

    private void OnTriggerExit2d (Collider2D area)
    {
        area.tag = "PlayArea";
        Debug.Log("Стоямба - Хуямба!");
        
    }
}
