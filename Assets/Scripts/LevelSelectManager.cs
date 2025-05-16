using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    public LevelInfo selectedLevel;

    public GameObject levelInfoPanel;
    public TMP_Text levelNameText;
    public TMP_Text levelDescriptionText;
    public GameObject possibleEnemiesParent;
    public TMP_Text localFloraText;
    public TMP_Text localFaunaText;
    public TMP_Text typicalFoodText;
    public GameObject characterParent;

    private void Start()
    {
        SoundManager.instance.PlayMusic(SoundManager.instance.mainMenuMusic);
        // Initialize the level info panel to be inactive
        levelInfoPanel.SetActive(false);
    }

    public void OnLevelSelected(LevelInfo level)
    {
        SoundManager.instance.PlaySfx(SoundManager.instance.selectLevel);
        selectedLevel = level;
        SetLevelInfo();
    }

    public void SetLevelInfo()
    {
        Canvas.ForceUpdateCanvases(); // Force the canvas to update its layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(possibleEnemiesParent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(characterParent.GetComponent<RectTransform>());

        levelInfoPanel.SetActive(true);
        // move the panel to the clicked position
        levelInfoPanel.transform.position = Input.mousePosition;
        levelNameText.text = selectedLevel.levelName;
        // levelDescriptionText.text = selectedLevel.levelDescription;

        // Clear previous enemies
        foreach (Transform child in possibleEnemiesParent.transform)
        {
            if (child.gameObject.name != "PossibleEnemiesText")
            {
                Destroy(child.gameObject);
            }
        }

        // Instantiate new enemy sprites
        foreach (Sprite enemySprite in selectedLevel.possibleEnemies)
        {
            GameObject enemyObject = new GameObject("Enemy");
            Image enemyObjectImage = enemyObject.AddComponent<Image>();
            enemyObjectImage.sprite = enemySprite;
            enemyObject.transform.SetParent(possibleEnemiesParent.transform);
            enemyObject.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32); // Set size for the enemy image
        }

        localFloraText.text = selectedLevel.localFlora;
        localFaunaText.text = selectedLevel.localFauna;
        typicalFoodText.text = selectedLevel.typicalFood;

        // Clear previous character
        foreach (Transform child in characterParent.transform)
        {
            if (child.gameObject.name != "YourCharacterText")
            {
                Destroy(child.gameObject);
            }
        }

        // Instantiate new character sprite
        GameObject characterObject = new GameObject("Character");
        Image characterObjectImage = characterObject.AddComponent<Image>();
        characterObjectImage.sprite = selectedLevel.character;
        characterObject.transform.SetParent(characterParent.transform);
        characterObject.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 38); // Set size for the character image


        Canvas.ForceUpdateCanvases(); // Force the canvas to update its layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(possibleEnemiesParent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(characterParent.GetComponent<RectTransform>());
    }

    public void CloseLevelInfo()
    {
        SoundManager.instance.PlaySfx(SoundManager.instance.deselectLevel);
        levelInfoPanel.SetActive(false);
    }

    public void LoadLevel()
    {
        // Load the selected level scene
        SoundManager.instance.PlaySfx(SoundManager.instance.startLevel);
        UnityEngine.SceneManagement.SceneManager.LoadScene(selectedLevel.levelSceneName);

        // play the level music
        switch (selectedLevel.levelSceneName)
        {
            case "Level1":
                SoundManager.instance.PlayMusic(SoundManager.instance.level1Music);
                break;
            case "Level2":
                SoundManager.instance.PlayMusic(SoundManager.instance.level2Music);
                break;
            case "Level3":
                SoundManager.instance.PlayMusic(SoundManager.instance.level3Music);
                break;
            case "Level4":
                SoundManager.instance.PlayMusic(SoundManager.instance.level4Music);
                break;
            default:
                Debug.LogError("No music assigned for this level.");
                break;
        }
    }
}
