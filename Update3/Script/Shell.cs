using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 탄피 구현
public class Shell : MonoBehaviour
{   
    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;
    
    float lifeTime = 4f;
    float fadeTime = 2f;


    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        myRigidbody.AddForce(transform.right *force); 
        myRigidbody.AddTorque(Random.insideUnitSphere * force);
    }


    IEnumerator Fade(){
        yield return new WaitForSeconds(lifeTime);

        float percent = 0;
        float fadeSpeed = 1/fadeTime;
        Material mat =GetComponent<Renderer>().material;
        Color initialColour = mat.color;

        while(percent < 1){
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColour, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}
