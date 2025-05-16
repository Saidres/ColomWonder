using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "LevelInfo", order = 0)]
public class LevelInfo : ScriptableObject
{
    public string levelSceneName;
    public string levelName;
    public string levelDescription;
    public List<Sprite> possibleEnemies;
    public string localFlora;
    public string localFauna;
    public string typicalFood;
    public Sprite character;
}