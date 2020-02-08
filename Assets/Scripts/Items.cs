using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    private Vector3 scaleChange;
    private GameObject player;
    private Player playerFind;
    private DistanceJoint2D goToPlayer;
    private StaffSpawn staffSpawn;
    private Vector2 newLocation;
    private Vector2 startLoc;
    public bool isNewItem = false;

    void Start()
    {
        player = GameObject.Find("Player");
        playerFind = player.GetComponentInChildren<Player>();
        goToPlayer = GetComponent<DistanceJoint2D>();
        goToPlayer.enabled = false;
        staffSpawn = FindObjectOfType<StaffSpawn>();
        scaleChange = new Vector2(0.8f, 0.8f);
        newLocation = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

    }

    private void Update()
    {
        goTo();
    }

    public void OnCollisionEnter2D(Collision2D colli)
    {
       

        if (colli.gameObject.tag == "Hook")
            {
            Rigidbody2D rb = colli.rigidbody;
            goToPlayer.enabled = true;
            goToPlayer.connectedBody = rb;
            }
    }

    public void OnTriggerStay2D(Collider2D gamer)
    {
        if (gamer.tag == "Player")
        {
            transform.position = Vector2.MoveTowards(transform.position, gamer.transform.position, 3f * Time.deltaTime);
            transform.localScale -= scaleChange * Time.deltaTime ;
            float dist = Vector2.Distance(transform.position, gamer.transform.position);
            if (dist <= 1.5f)
            {
                playerFind.fillOxygen += 0.1f;
                Destroy(gameObject);
            }
        }
    }


    public void goTo()
    {
        if (isNewItem)
        {
            transform.position = Vector2.MoveTowards(transform.position, newLocation, 10f * Time.deltaTime);
        }
         
    }
    private void OnDestroy()
    {
        staffSpawn.counter -= 1;
    }
}
