using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}