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
        // spawn feather;
        for(int i = 0; i<90; i++){
            Vector2 tmpPosition = new Vector2(Random.Range(15,600), randomFeatherPosY());
            Instantiate(feather,tmpPosition,transform.rotation);
        }

        // spawn comet
        for(int i = 0; i<250; i++){
            Vector2 tmpPosition = new Vector2(Random.Range(100,1000), Random.Range(500, 930));
            Instantiate(comet, tmpPosition, Quaternion.identity);
        }
        //sea
        // spawn buff
        for(int i = 0; i<360; i++){
            int index = Random.Range(0,seaBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(15,600),randomSeaPosY());
            Instantiate(seaBuffItems[index],tmpPosition,transform.rotation);
        }

        //sky
        // spawn buff
        for(int i = 0; i<360; i++){
            int index = Random.Range(0,skyBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(15,600),randomSkyPosY());
            Instantiate(skyBuffItems[index],tmpPosition,transform.rotation);
        }

        //space
        // spawn buff
        for(int i = 0; i<300; i++){
            int index = Random.Range(0,spaceBuffItems.Length);
            Vector2 tmpPosition = new Vector2(Random.Range(15,600),randomSpacePosY());
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

        // spawn feather;
        for(int i = 0; i<60; i++){
            Vector2 tmpPosition = new Vector2(tmp+randomX(), randomFeatherPosY());
            Instantiate(feather,tmpPosition,transform.rotation);
        }
        
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

    float randomFeatherPosY(){
        return Random.Range(10, sunHeight);
    }
}