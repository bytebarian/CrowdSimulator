using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Maze;
using System.Xml.Linq;
using System.Linq;

public class Maze : MonoBehaviour
{

	public IntVector2 size;

	public MazeCell cellPrefab;

	public float generationStepDelay;

	public MazePassage passagePrefab;

	public MazeDoor doorPrefab;

	[Range(0f, 1f)]
	public float doorProbability;

	public MazeWall wallPrefab;

	private MazeCell[,] cells;

    public List<MazeDoor> doors;

    public List<MazeWall> walls;

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

    public void LoadFromFile(string filename)
    {
        var document = XDocument.Load(Application.dataPath + "/" + filename);
        var root = document.Root;

        var size = root.Element("Size");

        var walls = from shape in document.Descendants("Shape")
                    where (string)shape.Element("Type") == "line"
                    select shape;

        var doors = from shape in document.Descendants("Shape")
                    where (string)shape.Element("Type") == "doors"
                    select shape;

    }

    public Vector3 GetPostionOnMaze(IntVector2 coordinates)
    {
        return new Vector3(coordinates.x - size.x * 0.5f + 0f, 0.5f, coordinates.z - size.z * 0.5f + 0.5f);
    }


	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}

	private void CreateDoor (Vector3 startpoint, Vector3 endpoint, Vector3 midpoint, double rotation) {
		MazeDoor prefab = doorPrefab;
        MazeDoor door = Instantiate(prefab) as MazeDoor;
        door.Initialize(startpoint, endpoint, midpoint);
	}

	private void CreateWall (Vector3 startpoint, Vector3 endpoint, Vector3 midpoint, double rotation) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(startpoint, endpoint, midpoint);
	}
}