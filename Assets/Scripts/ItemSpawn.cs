using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] buffItems;
    [SerializeField] GameObject[] debuffItems;
    [SerializeField] GameObject[] comets;

    int maxX = 0;

    // Start is called before the first frame update
    void Start()
    {
        // spawn buff
        for(int i = 0; i<100; i++){
            int index = Random.Range(0,buffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600),ramdomObjectPosY());
            Instantiate(buffItems[index],tmpPosition,transform.rotation);
        }

        // spawn debuff
        for(int i = 0; i<75; i++){
            int index = Random.Range(0,debuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600),ramdomObjectPosY());
            Instantiate(buffItems[index],tmpPosition,transform.rotation);
        }

        // spawn comet
        for(int i = 0; i < 10; i++){
            int index = Random.Range(0,comets.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600), randomcometPosY());
            Instantiate(comets[index],tmpPosition,transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((int)player.transform.position.x/200 > maxX){
            maxX = (int)player.transform.position.x/200;
            spawnItems(maxX);
        }
    }

    void spawnItems(int currX){
        int tmp = (currX+2)*200;

        // spawn buff
        for(int i = 0; i<100; i++){
            int index = Random.Range(0,buffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),ramdomObjectPosY());
            Instantiate(buffItems[index],tmpPosition,transform.rotation);
        }

        // spawn debuff
        for(int i = 0; i<75; i++){
            int index = Random.Range(0,debuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),ramdomObjectPosY());
            Instantiate(debuffItems[index],tmpPosition,transform.rotation);
        }

        // spawn comet
        for(int i = 0; i < 10; i++){
            int index = Random.Range(0,comets.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomcometPosY());
            Instantiate(comets[index],tmpPosition,transform.rotation);
        }
    }

    float randomX(){
        return Random.Range(0, 200.0f);
    }

    float ramdomObjectPosY(){
        return Random.Range(10, 1000);
    }

    float randomcometPosY(){
        return Random.Range(1000, 1500);
    }
}
