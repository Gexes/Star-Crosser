using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName = "Collectibles/CollectibleData")]
public class CollectibleData : ScriptableObject
{
    public string collectibleName;
    public Sprite collectibleIcon; // Icon to display in the inventory
    public int value; // value for scoring
    public AudioClip collectibleSound; // Audio that plays
}
