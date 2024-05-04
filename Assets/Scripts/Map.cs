using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Scriptable Objects/Map")]
public class Map : ScriptableObject
{
    public int levelIndex;
    public string mapName;
    public Color nameColor;
    public Sprite mapImage;
    public Object sceneToLoad;
}