using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpawn : MonoBehaviour
{
    private int [] spawnPoint = {-120, 120};
    public GameObject[] staff ;
    private int count;
    private Vector2 attenuator;
    public GameObject oxygen;
    public int counter;



    void Start()
    {
        counter = 0;
        count = 20;
        OxygenSpawner(true);
        staff = new GameObject[20];
    }


    void Update()
    {
        OxygenSpawner(false);
        Debug.Log(counter);
        //goTo();
    }

    private void OxygenSpawner(bool isStarted)
    {

            for (int i = counter; i < count; i++)
            {
                if (counter < count)
                {
                    Vector2 startPoints = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
                    Vector2 newPoints = new Vector2(spawnPoint[Random.Range(0, spawnPoint.Length)], spawnPoint[Random.Range(0, spawnPoint.Length)]);
                    attenuator = new Vector2(Random.Range(-15f, 15f), Random.Range(-15f, 15f));
                    if (isStarted == true)
                    {
                    Instantiate(oxygen, startPoints, (Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(-180, 180))));
                    counter += 1;
                    }
                    else 
                    {
                        GameObject gameObject = Instantiate(oxygen, newPoints + attenuator, transform.rotation);
                        gameObject.GetComponent<Items>().isNewItem = true;
                        counter += 1;


                    }

                }
                else if (counter == count)
                {
                    isStarted = false;
                }
        }
    }

}
