using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LevelEndDoor : MonoBehaviour
{
    public QuizInfo quizInfo;
    public LevelInfo levelInfo;

    public GameObject levelEndScreen;
    public TMP_Text collectedCoinsText;
    public Button continueButton;

    private void Start()
    {
        levelEndScreen.SetActive(false);
        levelEndScreen.GetComponent<CanvasGroup>().alpha = 0;
        levelEndScreen.GetComponent<CanvasGroup>().interactable = false;
        levelEndScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // stop the player from moving
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.StopPlayer();
            SoundManager.instance.PlaySfx(SoundManager.instance.useDoor);
            EnableLevelEndScreen();
        }
    }

    public void EnableLevelEndScreen()
    {
        levelEndScreen.SetActive(true);
        levelEndScreen.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        levelEndScreen.GetComponent<CanvasGroup>().interactable = true;
        levelEndScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        collectedCoinsText.text = "Monedas recolectadas: " + GameObject.FindObjectOfType<CurrencyController>().currencyAmount.ToString();
        continueButton.onClick.AddListener(ContinueButtonClicked);
    }

    public void ContinueButtonClicked()
    {
        SoundManager.instance.PlayRandomFromList(SoundManager.instance.answerClickSounds);
        GameManager.instance.SetQuizInfo(quizInfo);
        SceneManager.LoadScene("QuizScene");
    }
}