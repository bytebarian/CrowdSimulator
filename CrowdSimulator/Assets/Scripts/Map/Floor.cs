using UnityEngine;

public class Floor : MonoBehaviour
{
    public virtual void Initialize(float width, float height)
    {
        transform.localScale = new Vector3(height, transform.localScale.y, width);
    }
}
