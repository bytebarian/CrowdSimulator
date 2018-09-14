using UnityEngine;

public class MazeWall : MonoBehaviour
{
    public virtual void Initialize(Vector3 startPosition, Vector3 endPostition, Vector3 midpoint)
    {
        var distance = Vector3.Distance(startPosition, endPostition);
        var direction = endPostition - startPosition;
        transform.position = midpoint;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
    }
}