using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CircleInput circle;

    public GameObject note;

    public TextMeshProUGUI questionText;

    List<Note> notes;

    public QuestionSO currentQuestionSO;

    public DialogSO currentDialogSO;

    public int curDialogNum = 0;

    public Slider questionTimer;

    float timeSpent = 0;

    float score = 0;

    List<Note> activeNotes;

    public List<string> stammers;

    public TextMeshProUGUI saidSentance;

    string saidSentanceText = "";

    public float[] shapeAmmounts;

    float questionWritingTimer = 0;

    float wordWritingTimer = 0f;

    public AudioClip greatSound, goodSound, badSound, failSound;

    public bool dialogSelected = false;

    public List<Texture2D> colorPalletes;

    public int curColor = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDialogOptions();

        shapeAmmounts = new float[circle.circleRB.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh.blendShapeCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(curColor < colorPalletes.Count - 1)
            {
                curColor = curColor + 1;
            }
            else
            {
                curColor = 0;
            }
        }

        Shader.SetGlobalTexture("_Colors", colorPalletes[curColor]);

        questionTimer.maxValue = currentQuestionSO.timeToRespond;

        questionTimer.value = currentQuestionSO.timeToRespond - timeSpent;

        saidSentance.text = saidSentanceText;     


        //fuggggg
        for (int i = 0; i < shapeAmmounts.Length-1; i++)
        {
            circle.circleRB.gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, shapeAmmounts[i]);
        }


        //writing question over time

        questionWritingTimer = questionWritingTimer + Time.deltaTime;

        float questionWritenPercentage = questionWritingTimer / currentQuestionSO.writeInTime;

        int questionWritenPercentageInt = Mathf.RoundToInt((float)currentQuestionSO.question.Length * questionWritenPercentage);

        //place holder for fiding out when I need to "write" new sentance over time
        if (questionText.text != currentQuestionSO.question)
        {
            Debug.Log("Write Sentance");
            questionText.text = currentQuestionSO.question.Substring(0, questionWritenPercentageInt);
        }
        //if question fully typed
        else
        {
            timeSpent = timeSpent + Time.deltaTime;
        }


        //questionText.text = currentQuestionSO.question;



        //If you run out of time
        if (timeSpent >= currentQuestionSO.timeToRespond)
        {
            Debug.Log("Question timed out");

            //remove all notes
            foreach (Note dialogOptions in FindObjectsOfType<Note>())
            {
                dialogOptions.gameObject.GetComponent<Animator>().SetTrigger("great");
            }

            //add ... to said sentance
            saidSentanceText = saidSentanceText + "...";

            //backup question
            currentQuestionSO = currentQuestionSO.timeOutQuestion;
            SpawnDialogOptions();
            curDialogNum = 0;
            ResetTimer();

        }
    }

    public void NoteHit(bool success, AudioClip audioClip, string accuracy,int score,string wordSaid ,GameObject thisNote)
    {
        if(success == true)
        {
            SpawnNextNote();
        }

        PlaySound(audioClip, thisNote);

        AddScore(score);
        SpeakWord(wordSaid, success);
        circle.gameObject.GetComponent<Animator>().SetTrigger(accuracy);
        thisNote.GetComponent<Animator>().SetTrigger(accuracy);
    }

    public void PlaySound(AudioClip audioClip, GameObject thisNote)
    {
        

        if (thisNote.GetComponent<Note>().isDialogOption == false)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
        }
        else
        {
            //dialog sounds ;-;
            gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
        }
    }

    public void NoteMiss()
    {

    }


    public void SpawnNextNote()
    {
        //if I finish a dialog option
        if (curDialogNum > currentDialogSO.dialogOptions.Count - 1)
        {
            currentQuestionSO = currentDialogSO.nextQuestion;
            SpawnDialogOptions();
            curDialogNum = 0;
            ResetTimer();
        }
        else
        {          
            if ((int)currentDialogSO.dialogOptions[curDialogNum].shape > 0)
            {
                shapeAmmounts[(int)currentDialogSO.dialogOptions[curDialogNum].shape - 1] = shapeAmmounts[(int)currentDialogSO.dialogOptions[curDialogNum].shape - 1] + currentDialogSO.dialogOptions[curDialogNum].shapeAmount;
            }

            GameObject newNote = Instantiate(note, Vector3.zero, Quaternion.identity);
            newNote.GetComponent<Note>().myRotation = currentDialogSO.dialogOptions[curDialogNum].angle /*Next dialog word.rotation*/;
            newNote.GetComponent<Note>().wordText.text = currentDialogSO.dialogOptions[curDialogNum].word;
            newNote.GetComponent<Note>().gameManager = this;
          
            curDialogNum = curDialogNum + 1;
        }      
    }



    public void SpawnDialogOptions()
    {
        foreach (QuestionSO.QuestionOption option in currentQuestionSO.questionOptions)
        {
            GameObject newNote = Instantiate(note, Vector3.zero, Quaternion.identity);
            newNote.GetComponent<Note>().myRotation = option.angle /*Next dialog word.rotation*/;
            newNote.GetComponent<Note>().wordText.text = option.dialog.fullSentance;
            newNote.GetComponent<Note>().gameManager = this;
            newNote.GetComponent<Note>().isDialogOption = true;
            newNote.GetComponent<Note>().newDialog = option.dialog;

            //activeNotes.Add(newNote.GetComponent<Note>());
        }
    }

    public void SetNewDialog(DialogSO newDialog)
    {
        currentDialogSO = newDialog;

        dialogSelected = false;

        saidSentanceText = "";
    }

    public void AddScore(float addedScore)
    {
        score = score + addedScore;
    }

    public void SpeakWord(string saidWord, bool success)
    {      
        string stammer = "";

        int stammerInt = Random.Range(0, 2); 

        if (stammerInt == 0)
        {
            stammer = " " + stammers[Random.Range(0, stammers.Count)];
        }
        else if(stammerInt == 1 && saidWord.Length >= 2)
        {
            stammer = " " + saidWord.Substring(0, Random.Range(1, 3)) + "-";
        }
        else
        {
            stammer = " " + saidWord.Substring(0, 1) + "-";
        }



        if (success)
        {
            saidSentanceText = saidSentanceText + " " + saidWord;
        }

        else
        {
            saidSentanceText = saidSentanceText + stammer;
        }
        
    }

    void ResetTimer()
    {
        timeSpent = 0;
        questionWritingTimer = 0;
    }
}
