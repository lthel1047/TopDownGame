using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzleflash : MonoBehaviour
{
    public GameObject flashHolder;
    public Sprite[] flashSprite;
    public SpriteRenderer[] spriteRenderer;

    public float flashTime;
 

    public void Activate(){
        flashHolder.SetActive(true);

        int flashSpriteIndex = Random.Range(0, flashSprite.Length);
        for(int i=0;i<spriteRenderer.Length;i++){
            spriteRenderer[i].sprite = flashSprite[flashSpriteIndex];
        }
        Invoke("Deactivate", flashTime);
    }

    void  Deactivate(){
        flashHolder.SetActive(false);
    }
}
