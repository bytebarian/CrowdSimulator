using UnityEngine;
using System.Collections.Generic;
using System.IO;

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

	public void Generate () {
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) {
			DoNextGenerationStep(activeCells);
		}
	}

    public void LoadFromFile(string filename)
    {
        var sr = new StreamReader(Application.dataPath + "/" + filename);
        var fileContent = sr.ReadToEnd();
        sr.Close();

        var entities = fileContent.Split("\n"[0]);

        // get size of maze from first entity
        var sizev = entities[0].Split(':');
        if (sizev.Length != 2) return;
        int sizex, sizey;
        if (!int.TryParse(sizev[0], out sizex) || !int.TryParse(sizev[1], out sizey)) return;
        size = new IntVector2(sizex, sizey);
        cells = new MazeCell[size.x, size.z];
        // initialize all cells with empty cell
        foreach (var v in IntVector2.Range(size))
        {
            CreateCell(v);
        }
        
        foreach (var entity in entities)
        {
            var v = entity.Split(':');
            if (v.Length != 4) continue;
            int type, x, y, dir;
            if(!int.TryParse(v[0], out type) 
                || !int.TryParse(v[1], out x)
                || !int.TryParse(v[2], out y)
                || !int.TryParse(v[3], out dir))
            {
                continue;
            }
            y = sizey - y;
            var coordinates = new IntVector2(x, y);
            if (!ContainsCoordinates(coordinates)) continue;
            var currentCell = GetCell(coordinates);

            if (type == 1)
            {
                CreateWall(currentCell, null, (MazeDirection)dir);
            }
            else
            {
                var ncoordinates = currentCell.coordinates + ((MazeDirection)dir).ToIntVector2();
                if (!ContainsCoordinates(ncoordinates)) continue;
                var ncell = GetCell(ncoordinates);
                CreatePassage(currentCell, ncell, (MazeDirection)dir, type);
            }
        }
    }

    public Vector3 GetPostionOnMaze(IntVector2 coordinates)
    {
        return new Vector3(coordinates.x - size.x * 0.5f + 0f, 0.5f, coordinates.z - size.z * 0.5f + 0.5f);
    }

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
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

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction, int type = 2) {
		MazePassage prefab = type == 3 ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		//MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		//wall.Initialize(cell, otherCell, direction);
		//if (otherCell != null) {
		//	wall = Instantiate(wallPrefab) as MazeWall;
		//	wall.Initialize(otherCell, cell, direction.GetOpposite());
		//}
	}
}