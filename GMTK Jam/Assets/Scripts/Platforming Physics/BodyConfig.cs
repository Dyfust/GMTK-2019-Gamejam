using UnityEngine;

[CreateAssetMenu(fileName = "Body Config", menuName = "Configurations/Body Config", order = 0)]
public class BodyConfig : ScriptableObject
{
    public float skinWidth;
    public int amountOfVerticalRays;
    public int amountOfHorizontalRays;
    public LayerMask collisionMask;
    public float wallDetectionDistance;
}