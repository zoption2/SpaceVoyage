using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameObject asteroid;
    private int count = 50 ;
    private Vector2 attenuator;
    private int[] coordinates = { -100, -80, -60, -40, -20, 0, 20, 40, 60, 80, 100  };
    public  List<Vector2> vectors = new List<Vector2>();

    void Awake()
    {
        AsteroidPlacer(0);
    }

       
    private void AsteroidPlacer(int currentCount)
    {
        for (int i = currentCount; i < count; i++)
        {
            float scaleX = Random.Range(1f, 5f);  //коеф. размера ГО
            float scaleY = scaleX;
            int posX = coordinates[Random.Range(0, coordinates.Length)]; 
            int posY = coordinates[Random.Range(0, coordinates.Length)];
           
            attenuator = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));  //варификатор позиции ГО
            Vector2 coords = new Vector2(posX, posY); 
           
            if (!vectors.Contains(coords))     // Создаёт объект в координате, если она свободна 
            {
                asteroid.transform.localScale = new Vector2(scaleX, scaleY);
                Instantiate(asteroid, coords + attenuator, Quaternion.identity);
                vectors.Add(coords);
            }
            else
            {
                break;
            }
        }
       
        if (vectors.Count < count)
        {
            AsteroidPlacer(vectors.Count);
        }
    } 


 }

