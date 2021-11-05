using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInput : MonoBehaviour
{
    public Rigidbody circleRB;
    public GameObject inputIndicator;
    public float forceMulti = 10;

    public float cursorRotation;

    public bool isFlicked = true;

    Vector3 force;

    float meme, oldMeme;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(isFlicked = false && )
        //{

        //}

        force = new Vector3(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"), 0.0f);


        //circleRB.AddForce(force * forceMulti);



        oldMeme = meme;

        if(Mathf.Abs(Input.GetAxis("MoveHorizontal")) > Mathf.Abs(Input.GetAxis("MoveVertical")))
        {
            meme = Mathf.Abs(Input.GetAxis("MoveHorizontal")) + Mathf.Abs(Input.GetAxis("MoveVertical")) / 4;
        }

        else if (Mathf.Abs(Input.GetAxis("MoveHorizontal")) < Mathf.Abs(Input.GetAxis("MoveVertical")))
        {
            meme =Mathf.Abs(Input.GetAxis("MoveHorizontal")) / 4 + Mathf.Abs(Input.GetAxis("MoveVertical"));
        }

        if (meme >= 0.7f && !(oldMeme >= 0.7f) && !isFlicked)
        {
            cursorRotation = -Mathf.Atan2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical")) * 180 / Mathf.PI;
            isFlicked = true;
        }
        else
        {
            isFlicked = false;
        }


        if (isFlicked)
        {
            Instantiate(inputIndicator).GetComponent<MeshRenderer>().material.SetFloat("_Rotation", cursorRotation);
            circleRB.AddForce(force * forceMulti);
        }
        
    }
}