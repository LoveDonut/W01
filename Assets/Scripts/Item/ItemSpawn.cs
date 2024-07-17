using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject feather;
    
    [SerializeField] GameObject comet;
    [SerializeField] GameObject cometSpawner;
    [SerializeField] GameObject[] seaBuffItems;
    [SerializeField] GameObject[] skyBuffItems;
    [SerializeField] GameObject[] spaceBuffItems;
    [SerializeField] GameObject[] debuffItems;
    [SerializeField] GameObject[] cloud;
    [SerializeField] GameObject[] star;

    int maxX = 0;

    public float sunHeight = 930;
    public float spaceHeight = 430;
    public float minCloudHeight = 10;
    public float maxCloudHeight = 400;
    public float starHeight = 430;
    public float skyHeight = 250;
    public float cometHeight = 30;

    // Start is called before the first frame update
    void Start()
    {
        // spawn cloud
        for(int i = 0; i < 270; i++){
            int index = Random.Range(0,cloud.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(-50,600), randomCloudPosY());
            Instantiate(cloud[index],tmpPosition,transform.rotation);
        }

        // spawn star
        for(int i = 0; i < 510; i++){
            int index = Random.Range(0,star.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(-100,600), randomStarPosY());
            Instantiate(star[index],tmpPosition,transform.rotation);
        }

        // spawn feather;
        for(int i = 0; i<45; i++){
            Vector2 tmpPosition = new Vector2(Random.Range(50,600), randomFeatherPosY());
            Instantiate(feather,tmpPosition,transform.rotation);
        }

        // spawn comet
        for(int i = 0; i<250; i++){
            Vector2 tmpPosition = new Vector2(Random.Range(100,1000), Random.Range(500, 930));
            Instantiate(comet, tmpPosition, Quaternion.identity);
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
        for(int i = 0; i<360; i++){
            int index = Random.Range(0,seaBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(50,600),randomSeaPosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }

        //sky
        // spawn buff
        for(int i = 0; i<360; i++){
            int index = Random.Range(0,skyBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(50,600),randomSkyPosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }

        //space
        // spawn buff
        for(int i = 0; i<300; i++){
            int index = Random.Range(0,spaceBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(50,600),randomSpacePosY());
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
        Instantiate(cometSpawner,new Vector2((currX+10)*200,430),Quaternion.identity);
        // spawn cloud
        for(int i = 0; i < 180; i++){
            int index = Random.Range(0,cloud.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomCloudPosY());
            Instantiate(cloud[index],tmpPosition,transform.rotation);
        }

        // spawn star
        for(int i = 0; i < 340; i++){
            int index = Random.Range(0,star.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomStarPosY());
            Instantiate(star[index],tmpPosition,transform.rotation);
        }

        // spawn feather;
        for(int i = 0; i<30; i++){
            Vector2 tmpPosition = new Vector2(tmp+randomX(), randomFeatherPosY());
            Instantiate(feather,tmpPosition,transform.rotation);
        }

        /*
        // spawn comet
        for(int i = 0; i < 15; i++){
            int index = Random.Range(0,comets.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(), randomcometPosY());
            Instantiate(comets[index],tmpPosition,transform.rotation);
        }

        // spawn debuff
        for(int i = 0; i<50; i++){
            int index = Random.Range(0,debuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSpacePosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }
        */
        
        //sea
        // spawn buff
        for(int i = 0; i<200; i++){
            int index = Random.Range(0,seaBuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSeaPosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }

        //sky
        // spawn buff
        for(int i = 0; i<200; i++){
            int index = Random.Range(0,skyBuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSkyPosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }

        //space
        // spawn buff
        for(int i = 0; i<160; i++){
            int index = Random.Range(0,spaceBuffItems.Length);
            Vector2 tmpPosition = new Vector2(tmp + randomX(),randomSpacePosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }
    }

    float randomX(){
        return Random.Range(0, 200.0f);
    }

    float randomSeaPosY(){
        return Random.Range(10, skyHeight);
    }

    float randomSkyPosY(){
        return Random.Range(skyHeight, spaceHeight);
    }

    float randomSpacePosY(){
        return Random.Range(spaceHeight, sunHeight);
    }
    

    /*
    float randomcometPosY(){
        return Random.Range(sunHeight - cometHeight, sunHeight);
    }
    */

    float randomStarPosY(){
        return Random.Range(starHeight, sunHeight);
    }

    float randomCloudPosY(){
        return Random.Range(minCloudHeight, maxCloudHeight);
    }

    float randomFeatherPosY(){
        return Random.Range(10, sunHeight);
    }
}