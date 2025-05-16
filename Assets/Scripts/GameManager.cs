using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deathScreen;
    public QuizInfo quizInfo;
    [Header("Mobile Controls")]
    public GameObject mobileControlsScreen;
    // public Button leftButton;
    // public Button rightButton;
    // public Button jumpButton;
    // public Button attackButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the death screen to be inactive at the start
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
            deathScreen.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            Debug.LogError("Death screen is not assigned in the GameManager.");
        }
    }

    public void GameOver()
    {
        StartCoroutine(ShowDeathScreenCoroutine());
    }

    private IEnumerator ShowDeathScreenCoroutine()
    {
        deathScreen.SetActive(true);
        deathScreen.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    public void RestartButtonClicked()
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.answerClickSounds);
        // Restart the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        deathScreen.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        deathScreen.SetActive(false);

    }

    public void MainMenuButtonClicked()
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.answerClickSounds);
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        deathScreen.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        deathScreen.SetActive(false);
    }

    public void SetQuizInfo(QuizInfo quiz)
    {
        quizInfo = quiz;
    }

    public QuizInfo GetQuizInfo()
    {
        return quizInfo;
    }
}
