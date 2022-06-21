using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotColor;
    Color originalDotColor;


    void Start(){
        Cursor.visible = false;
        originalDotColor = dot.color;

    }

    void Update()
    {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
    }

    public void DetectTargets(Ray ray){
        if(Physics.Raycast(ray,100,targetMask)){
                dot.color = dotColor;
        }
        else{
            dot.color = originalDotColor;
        }
    }
}
