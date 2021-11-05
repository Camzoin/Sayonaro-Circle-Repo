using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialog System/QuestionSO")]
public class QuestionSO : ScriptableObject
{
    [System.Serializable]
    public class QuestionOption
    {
        public DialogSO dialog;
        [Range(-180, 180)]
        public float angle = 0;
        public float size = 1;
    }

    public QuestionSO timeOutQuestion;
    public string question = "";
    public float timeToRespond = 10;
    public float writeInTime = 1f;
    public List<QuestionOption> questionOptions = new List<QuestionOption>();
}