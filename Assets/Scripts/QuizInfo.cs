using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizInfo", menuName = "QuizInfo", order = 0)]
public class QuizInfo : ScriptableObject
{
    public string quizSceneName;
    public string quizName;
    public string quizDescription;
    public List<QuizQuestion> questions;
}

[System.Serializable]
public class QuizQuestion
{
    [TextArea(3, 10)]
    public string questionText;
    public Sprite questionImage;
    [TextArea(2, 10)]
    public List<string> answers;
    public int correctAnswerIndex;
}
