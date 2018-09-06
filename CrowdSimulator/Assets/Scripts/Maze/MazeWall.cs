using UnityEngine;

public class MazeWall : MonoBehaviour
{
    public virtual void Initialize(Vector3 startPosition, Vector3 endPostition, Vector3 midpoint, Quaternion rotation)
    {
        var distance = Vector3.Distance(startPosition, endPostition);
        transform.position = midpoint;
        transform.rotation = rotation;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
    }
}