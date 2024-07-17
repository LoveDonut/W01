using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject feather;
    [SerializeField] GameObject[] seaBuffItems;
    [SerializeField] GameObject[] skyBuffItems;
    [SerializeField] GameObject[] spaceBuffItems;
    [SerializeField] GameObject[] debuffItems;
    [SerializeField] GameObject[] comets;
    [SerializeField] GameObject[] cloud;
    [SerializeField] GameObject[] star;

    int maxX = 0;

    // Start is called before the first frame update
    void Start()
    {
        // spawn cloud
        for(int i = 0; i < 30; i++){
            int index = Random.Range(0,cloud.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600), randomSkyPosY());
            Instantiate(cloud[index],tmpPosition,transform.rotation);
        }

        // spawn star
        for(int i = 0; i < 210; i++){
            int index = Random.Range(0,star.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600), randomStarPosY());
            Instantiate(star[index],tmpPosition,transform.rotation);
        }

        // spawn feather;
        for(int i = 0; i<45; i++){
            Vector2 tmpPosition = new Vector2(Random.Range(10,600), randomFeatherPosY());
            Instantiate(feather,tmpPosition,transform.rotation);
        }

        // spawn comet
        for(int i = 0; i < 15; i++){
            int index = Random.Range(0,comets.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600), randomcometPosY());
            Instantiate(comets[index],tmpPosition,transform.rotation);
        }

        /*
        // spawn debuff
        for(int i = 0; i<75; i++){
            int index = Random.Range(0,debuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600),randomSeaPosY());
            Instantiate(debuffItems[index],tmpPosition,transform.rotation);
        }
        */

        //sea
        // spawn buff
        for(int i = 0; i<75; i++){
            int index = Random.Range(0,seaBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600),randomSeaPosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }

        //sky
        // spawn buff
        for(int i = 0; i<75; i++){
            int index = Random.Range(0,skyBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600),randomSkyPosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }

        //space
        // spawn buff
        for(int i = 0; i<30; i++){
            int index = Random.Range(0,spaceBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(10,600),randomSpacePosY());
            Instantiate(spaceBuffItems[index],tmpPosition,transform.rotation);
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

        // spawn cloud
        for(int i = 0; i < 20; i++){
            int index = Random.Range(0,cloud.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomSkyPosY());
            Instantiate(cloud[index],tmpPosition,transform.rotation);
        }

        // spawn star
        for(int i = 0; i < 140; i++){
            int index = Random.Range(0,star.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomStarPosY());
            Instantiate(star[index],tmpPosition,transform.rotation);
        }

        // spawn feather;
        for(int i = 0; i<30; i++){
            Vector2 tmpPosition = new Vector2(tmp+randomX(), randomFeatherPosY());
            Instantiate(feather,tmpPosition,transform.rotation);
        }

        // spawn comet
        for(int i = 0; i < 10; i++){
            int index = Random.Range(0,comets.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomcometPosY());
            Instantiate(comets[index],tmpPosition,transform.rotation);
        }

        /*
        // spawn debuff
        for(int i = 0; i<50; i++){
            int index = Random.Range(0,debuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSpacePosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }
        */
        
        //sea
        // spawn buff
        for(int i = 0; i<50; i++){
            int index = Random.Range(0,seaBuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSeaPosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }

        //sky
        // spawn buff
        for(int i = 0; i<50; i++){
            int index = Random.Range(0,skyBuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSkyPosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }

        //space
        // spawn buff
        for(int i = 0; i<50; i++){
            int index = Random.Range(0,spaceBuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSpacePosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }
    }

    float randomX(){
        return Random.Range(0, 200.0f);
    }

    float randomSeaPosY(){
        return Random.Range(10, 300);
    }

    float randomSkyPosY(){
        return Random.Range(300, 500);
    }

    float randomSpacePosY(){
        return Random.Range(500, 750);
    }
    

    float randomcometPosY(){
        return Random.Range(800, 1000);
    }


    float randomStarPosY(){
        return Random.Range(100, 225);
    }

    float randomFeatherPosY(){
        return Random.Range(0, 500);
    }
}
