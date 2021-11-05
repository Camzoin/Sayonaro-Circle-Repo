using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    public GameManager gameManager;
    float cursorRotation;
    CircleInput circle;
    Animator circleAnimator;
    public float myRotation;
    public MeshRenderer indicator;
    public float Great, Good, Bad;
    public Transform textDaddy;
    public TextMeshProUGUI wordText;

    public bool isDialogOption = false;
    public DialogSO newDialog;
    public AudioClip greatSound, goodSound, badSound, failSound;

    // Start is called before the first frame update
    void Start()
    {
        circle = GameObject.Find("Circle").GetComponent<CircleInput>();

        circleAnimator = circle.gameObject.GetComponent<Animator>();

        textDaddy.rotation = Quaternion.Euler(0, 0, myRotation);

        //myRotation = Random.Range(-180f, 180f);
        indicator.material.SetFloat("_Rotation", myRotation);
    }

    // Update is called once per frame
    void Update()
    {
        cursorRotation = circle.cursorRotation;

        wordText.transform.rotation = Quaternion.identity;

        if (circle.isFlicked && !isDialogOption)
        {
            if (Mathf.Abs(myRotation - cursorRotation) < Great || (Mathf.Abs(myRotation - cursorRotation) > 270 && Mathf.Abs(myRotation - cursorRotation) - 360 < Great))
            {
                Debug.Log("Great " + Mathf.Abs(myRotation - cursorRotation));

                gameManager.NoteHit(true, greatSound, "great", 5, wordText.text, this.gameObject);
            }

            else if (Mathf.Abs(myRotation - cursorRotation) < Good || (Mathf.Abs(myRotation - cursorRotation) > 270 && Mathf.Abs(myRotation - cursorRotation) - 360 < Good))
            {
                Debug.Log("Good " + Mathf.Abs(myRotation - cursorRotation));

                gameManager.NoteHit(true, goodSound, "great", 3, wordText.text, this.gameObject);
            }
            else if (Mathf.Abs(myRotation - cursorRotation) < Bad || (Mathf.Abs(myRotation - cursorRotation) > 270 && Mathf.Abs(myRotation - cursorRotation) - 360 < Bad))
            {
                Debug.Log("Bad" + Mathf.Abs(myRotation - cursorRotation));

                gameManager.NoteHit(true, badSound, "great", 1, wordText.text, this.gameObject);
            }
            else
            {
                Debug.Log("Failure" + Mathf.Abs(myRotation - cursorRotation));

                gameManager.NoteHit(false, failSound, "fail", -3, wordText.text, this.gameObject);
            }
        }

        if (circle.isFlicked && isDialogOption)
        {
            if (Mathf.Abs(myRotation - cursorRotation) < Bad)
            {
                Debug.Log("Dialog Select Success");

                gameManager.SetNewDialog(newDialog);

                gameManager.PlaySound(greatSound, this.gameObject);

                gameObject.GetComponent<Animator>().SetTrigger("great");

                foreach (Note dialogOptions in FindObjectsOfType<Note>())
                {
                    if (dialogOptions.isDialogOption == true)
                    {
                        dialogOptions.gameObject.GetComponent<Animator>().SetTrigger("great");
                    }
                }

                gameManager.SpawnNextNote();
            }
            else
            {
                Debug.Log("Dialog Select Failure");

                gameManager.PlaySound(failSound, this.gameObject);
            }
        }
    }
}
