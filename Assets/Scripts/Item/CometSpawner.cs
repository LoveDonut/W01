using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    [SerializeField] GameObject comet;
    bool spawnCoolTime = false;

    // Update is called once per frame
    void Update()
    {
        if(!spawnCoolTime){
            spawnCoolTime = true;
            spawnComet();
            StartCoroutine(CometCoolDown());
        }
    }
    
    void spawnComet(){
        for(int i = 0; i<3; i++){
            float randomY = Random.Range(900,930);
            float randomX = Random.Range(-100, 100);
            Instantiate(comet, new Vector2(randomX + transform.position.x, randomY),Quaternion.identity);
        }
        
    }

    IEnumerator CometCoolDown(){
        yield return new WaitForSeconds(2);
        spawnCoolTime = false;
    }
}
