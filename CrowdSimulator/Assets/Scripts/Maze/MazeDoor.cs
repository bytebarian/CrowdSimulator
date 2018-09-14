using UnityEngine;

public class MazeDoor : MazePassage {

	public Transform hinge;

	private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}

    //public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
    //	base.Initialize(primary, other, direction);
    //	if (OtherSideOfDoor != null) {
    //		hinge.localScale = new Vector3(-1f, 1f, 1f);
    //		Vector3 p = hinge.localPosition;
    //		p.x = -p.x;
    //		hinge.localPosition = p;
    //	}
    //}

    public virtual void Initialize(Vector3 startPosition, Vector3 endPostition, Vector3 midpoint)
    {
        var distance = Vector3.Distance(startPosition, endPostition);
        var direction = endPostition - startPosition;
        transform.position = midpoint;
        transform.rotation = transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
    }
}