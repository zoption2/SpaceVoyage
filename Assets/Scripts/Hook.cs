using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hook : MonoBehaviour
{
    private float timeBetweenHookShots;
    private float startShotHook =0.5f;

    //List<GameObject> gameObjects;

    private GameObject Player;
    private GameObject SpaceObject;
    public GameObject hook;
    private GameObject hookClone;
    public Transform ShootDirection;
    public bool hookIsBack;
    public bool isHookExist;
    public bool hookIsFlying;
    private bool wasEnemyHooked;
    private float hookDistance;
    private Vector2 hookClonePos;
    private Vector2 hookStartPosition;
    private hookCollision _hookCollision;
    private Animator playerAnimator;

    private LineRenderer lineRenderer;
    private Animator animus;




    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
        hook = Resources.Load("SpaceHook") as GameObject;
        //SpaceObject = GameObject.FindGameObjectWithTag("SpaceObject");
        isHookExist = false;
        hookIsBack = false;
        wasEnemyHooked = false;
        hookIsFlying = false;
        
    }
    void Start()
    {
        playerAnimator = Player.GetComponentInChildren<Animator>();
        lineRenderer.enabled = false;
        animus = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hookInstantiate();
        if (isHookExist == true)
        {
            _hookCollision = hookClone.GetComponent<hookCollision>();
            hookFly();
            hookBack();
            hookBackToYou();
        }
        hookGrabbing();
  
    }

    void LateUpdate()
    {
        if (isHookExist == true)
        {
            ropeWork();
        }
    }

    private void ropeWork()
    {
        lineRenderer.SetPosition(0, ShootDirection.position);
        lineRenderer.SetPosition(1, hookClone.transform.position);
    }

    private void hookInstantiate()   // метод появления хука
    {
        if (timeBetweenHookShots <= 0 && Input.GetMouseButtonDown(1) && isHookExist == false && wasEnemyHooked == false)
        {
                isHookExist = true;
                hookIsFlying = true;
                hookClone = Instantiate(hook, ShootDirection.position, transform.rotation);
                isHookExist = true; 
                lineRenderer.enabled = true;
                timeBetweenHookShots = startShotHook;
                animus.SetBool("hookON", false);
        }
        else 
        { 
            timeBetweenHookShots -= Time.deltaTime;
        }
    }

   private void hookFly()  // полёт хука после его появления
    {
        if (hookIsBack == false && hookIsFlying ==true && _hookCollision.canPool == false)
        {
            hookClone.transform.Translate(Vector2.right * Constants.HOOK_SPEED * Time.deltaTime);
        }
    }

    public void destroyHook()      //уничтожение хука
    {
        animus.SetBool("hookON", true);
        isHookExist = false;
        hookIsBack = false;
        hookIsFlying = false;
        lineRenderer.enabled = false;
        _hookCollision.canPool = false;
        Destroy(hookClone.gameObject);
        
    }
  
    private void hookBack()        // условие, если хук ни за что не зацепился - он возвращается
    {
            hookDistance = Vector2.Distance(ShootDirection.position, hookClone.transform.position);
            if (hookDistance > Constants.MAX_HOOK_DISTANCE && _hookCollision.canPool == false)
            {
            hookIsBack = true;
            hookIsFlying = false;
            }
    }

    private void hookBackToYou()   // метод возвращение хука в руку, если он ни за что не зацепился
    {
        if (hookIsBack == true || _hookCollision.canPool ==true)
        {
            hookIsFlying = false;
            hookClonePos = new Vector2(hookClone.transform.position.x, hookClone.transform.position.y);
            hookStartPosition = new Vector2(ShootDirection.position.x, ShootDirection.position.y);
            hookClone.transform.position = Vector2.MoveTowards(hookClonePos, hookStartPosition, Constants.HOOK_SPEED * Time.deltaTime);
            Vector2 difference = hookClonePos - hookStartPosition;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            hookClone.transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);
            if (Vector2.Distance(hookClone.transform.position, ShootDirection.position) <= Constants.HOOK_STOP_DISTANCE)

            {
                destroyHook();
            }
        }
    }

    private void hookGrabbing()
    {
        if (isHookExist == true)
        {
            _hookCollision = hookClone.GetComponent<hookCollision>();
     
            if (_hookCollision.canGrab == true && _hookCollision.canPool ==false)
            {
                hookIsBack = false;
                hookIsFlying = false;

                if (Vector2.Distance(hookClone.transform.position, Player.transform.position) > Constants.HOOK_STOP_DISTANCE)
                {
                    Player.transform.position = Vector2.MoveTowards(Player.transform.position, hookClone.transform.position, 20f * Time.deltaTime);
                    playerAnimator.SetBool("isGrabbing", true);
                }

                if (Vector2.Distance(hookClone.transform.position, Player.transform.position) <= Constants.HOOK_STOP_DISTANCE)
                {
                    playerAnimator.SetBool("isGrabbing", false);
                    destroyHook();
                }
            }
            
        }
    }
}
