using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialog System/DialogSO")]

public class DialogSO : ScriptableObject
{
    [System.Serializable]
    public class DialogOption
    {
        public string word = "";
        [Range(-180,180)]
        public float angle = 0;
        public float size = 1;
        public float timeToSay = 0.2f;

        public enum Shapes
        {
            Default = 0,
            Heart = 1,
            Star = 2
        }

        [SerializeField]
        private Shapes Shape = Shapes.Default;
        public Shapes shape
        {
            get
            {
                return Shape;
            }
            set
            {
                Shape = value;
            }
        }
        [Range(-100, 100)]
        public float shapeAmount = 0;
    }

    public string fullSentance = "";
    public List<DialogOption> dialogOptions = new List<DialogOption>();

    public QuestionSO nextQuestion = null;

    [ContextMenu("AutoFill")]
    public void AutoFill()
    {
        dialogOptions.Clear();
        string[] words = fullSentance.Split(' ');
        foreach (string _word in words)
        {
            dialogOptions.Add(new DialogOption() { word = _word }); 
        }
    }

    [ContextMenu("RandomRotations")]
    public void RandomRotations()
    {
        foreach (var _word in dialogOptions)
        {
            _word.angle = Random.Range(-180f, 180f);
        }
    }
}
