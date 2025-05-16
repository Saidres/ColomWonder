using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class QuizSceneHandler : MonoBehaviour
{
    public QuizInfo quizInfo;

    public GameObject quizTimeText;

    public GameObject quizParent;
    public TMP_Text questionNumber;
    public TMP_Text questionText;
    public GameObject answerContainer;
    public GameObject answerPrefab;
    public Button nextButton;

    private int answerNumberCounter = 0;
    private string selectedAnswer;

    [Header("Current Quiz Stats")]
    public int correctAnswers;
    public int currentQuestionIndex;

    [Header("Quiz Finished")]
    public GameObject quizFinishedPanel;
    public TMP_Text quizFinishedText;
    public Button backToMainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayMusic(SoundManager.instance.quizMusic);
        StartCoroutine(QuizTimeCoroutine());
    }

    private IEnumerator QuizTimeCoroutine()
    {
        // if (ES3.KeyExists("QuizInfo"))
        // {
        //     ES3.LoadInto("QuizInfo", quizInfo);
        // }

        quizInfo = GameManager.instance.GetQuizInfo();

        SoundManager.instance.PlaySfx(SoundManager.instance.quizStart);

        quizTimeText.GetComponent<CanvasGroup>().alpha = 0;
        quizParent.GetComponent<CanvasGroup>().alpha = 0;

        quizTimeText.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(1.5f);
        quizTimeText.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        if (quizInfo != null)
        {
            SetQuizInfo(quizInfo);
        }
        else
        {
            Debug.LogError("QuizInfo is not set!");
        }

        currentQuestionIndex = 0;
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (ES3.KeyExists("QuizInfo"))
            {
                ES3.Load("QuizInfo", quizInfo);
                Debug.Log("QuizInfo loaded from ES3");
            }
            else
            {
                Debug.LogError("QuizInfo is not set!");
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(answerContainer.GetComponent<RectTransform>(), Input.mousePosition))
            {
                nextButton.gameObject.SetActive(false);
            }
        }

    }

    public void SetQuizInfo(QuizInfo quiz)
    {
        nextButton.gameObject.SetActive(false);

        quizInfo = quiz;
        questionNumber.text = "Pregunta " + (currentQuestionIndex + 1) + "/" + quizInfo.questions.Count;
        questionText.text = quizInfo.questions[currentQuestionIndex].questionText;

        // Clear previous answers
        foreach (Transform child in answerContainer.transform)
        {
            Destroy(child.gameObject);
        }

        answerNumberCounter = 0;

        // Instantiate new answer buttons
        foreach (string answer in quizInfo.questions[currentQuestionIndex].answers)
        {
            GameObject answerObject = Instantiate(answerPrefab, answerContainer.transform);
            TMP_Text answerNumberText = answerObject.transform.Find("AnswerNumber").GetComponent<TMP_Text>();
            answerNumberText.text = (answerNumberCounter + 1).ToString() + ".";
            answerNumberCounter++;
            TMP_Text answerText = answerObject.transform.Find("AnswerText").GetComponent<TMP_Text>();
            answerText.text = answer;
            Button button = answerObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnAnswerSelected(answer));
        }

        quizParent.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        quizParent.GetComponent<CanvasGroup>().interactable = true;
        quizParent.GetComponent<CanvasGroup>().blocksRaycasts = true;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(answerContainer.GetComponent<RectTransform>());
    }

    public void OnAnswerSelected(string answer)
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.answerClickSounds);
        // Check if the answer is correct
        selectedAnswer = answer;

        // enable the next button
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = true;
    }

    public void OnNextButtonClicked()
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.popSounds);
        // Disable the next button to prevent multiple clicks
        nextButton.interactable = false;

        // Start the coroutine to handle the next question
        StartCoroutine(OnNextButtonClickedCoroutine());
    }

    public IEnumerator OnNextButtonClickedCoroutine()
    {
        int correctAnswerIndex = quizInfo.questions[currentQuestionIndex].correctAnswerIndex;
        if (selectedAnswer == quizInfo.questions[currentQuestionIndex].answers[correctAnswerIndex])
        {
            Debug.Log("Correct Answer!");
            correctAnswers++;
            // Handle correct answer logic here
        }
        else
        {
            Debug.Log("Incorrect Answer!");
            // Handle incorrect answer logic here
        }

        selectedAnswer = null;

        quizParent.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        quizParent.GetComponent<CanvasGroup>().interactable = false;
        quizParent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        yield return new WaitForSeconds(0.5f);

        // Move to the next question or finish the quiz
        currentQuestionIndex++;
        if (currentQuestionIndex < quizInfo.questions.Count)
        {
            SetQuizInfo(quizInfo);
        }
        else
        {
            Debug.Log("Quiz Finished!");
            OnQuizFinished();
        }
    }

    public void OnQuizFinished()
    {
        SoundManager.instance.PlaySfx(SoundManager.instance.quizEnd);
        // Disable the next button
        nextButton.gameObject.SetActive(false);
        nextButton.interactable = false;

        // Start the coroutine to handle the quiz finished logic
        StartCoroutine(OnQuizFinishedCoroutine());
    }

    public IEnumerator OnQuizFinishedCoroutine()
    {
        quizParent.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        quizParent.GetComponent<CanvasGroup>().interactable = false;
        quizParent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        yield return new WaitForSeconds(0.5f);

        // Show the quiz finished panel
        quizFinishedPanel.SetActive(true);
        quizFinishedText.text = "¡Respondiste correctamente " + correctAnswers + " de " + quizInfo.questions.Count + " preguntas!";
        quizFinishedPanel.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        quizFinishedPanel.GetComponent<CanvasGroup>().interactable = true;
        quizFinishedPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClicked);
    }
    public void OnBackToMainMenuButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
