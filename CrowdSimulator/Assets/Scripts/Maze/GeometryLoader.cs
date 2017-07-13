using UnityEngine;

public class GeometryLoader : MonoBehaviour
{
    public Groundplane groundplane;  
    public Maze mazeInstance;
    public Maze mazePrefab;

    public void LoadGeometry(string folderpath)
    {
        groundplane = new Groundplane();
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.LoadFromFile(folderpath + "geometry.txt");
    }
}

