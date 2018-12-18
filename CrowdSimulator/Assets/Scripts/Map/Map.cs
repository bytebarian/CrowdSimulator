using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class Map : MonoBehaviour
{

	public Vector2 size;
	public Floor floorPrefab;
	public Door doorPrefab;
	public Wall wallPrefab;
    public List<Door> doors;
    public List<Wall> walls;
    public Floor floor;

    private const float scale = 10f;

    public void LoadFromFile(string filename)
    {
        var document = XDocument.Load(Application.dataPath + "/" + filename);
        var root = document.Root;

        CreateFloor(root);
        CreateWalls(document);
        //CreateDoors(document);
    }

    private void CreateFloor(XElement root)
    {
        var size = root.Element("Size");
        floor = Instantiate(floorPrefab) as Floor;
        this.size = new Vector2((float)size.Element("Width") / scale, (float)size.Element("Height") / scale);
        floor.Initialize(this.size.x, this.size.y);
    }

    private void CreateDoors(XDocument document)
    {
        var doorsCollection = from shape in document.Descendants("Shape")
                              where (string)shape.Element("Type") == "doors"
                              select shape;

        foreach (var door in doorsCollection)
        {
            var points = from point in door.Descendants("Point")
                         select new Vector3((float)point.Element("X") / scale, (float)point.Element("Y") / scale, 0);

            var mp = new Vector3(points.Sum(_ => _.x) / 4, points.Sum(_ => _.y) / 4, 0);

            Vector3 sp, ep;
            var width = (float)door.Element("Width");
            var height = (float)door.Element("Height");

            //CreateDoor(sp, ep, mp, 0);
        }
    }

    private void CreateWalls(XDocument document)
    {
        var wallsCollection = from shape in document.Descendants("Shape")
                              where (string)shape.Element("Type") == "line"
                              select shape;

        foreach (var wall in wallsCollection)
        {
            var x1 = (float)wall.Element("X1") / scale;
            var y1 = (float)wall.Element("Y1") / scale;
            var x2 = (float)wall.Element("X2") / scale;
            var y2 = (float)wall.Element("Y2") / scale;
            var mx = (float)wall.Element("MedianPoint").Element("X") / scale;
            var my = (float)wall.Element("MedianPoint").Element("Y") / scale;

            Vector3 sp = new Vector3(x1 - size.x / 2, 1, size.y - y1);
            Vector3 ep = new Vector3(x2 - size.x / 2, 1, size.y - y2);
            Vector3 mp = new Vector3(mx - size.x / 2, 1, size.y - my);

            CreateWall(sp, ep, mp, 0);
        }
    }

    private void CreateDoor (Vector3 startpoint, Vector3 endpoint, Vector3 midpoint, double rotation) {
		Door prefab = doorPrefab;
        Door door = Instantiate(prefab) as Door;
        door.Initialize(startpoint, endpoint, midpoint);
        doors.Add(door);
    }

	private void CreateWall (Vector3 startpoint, Vector3 endpoint, Vector3 midpoint, double rotation) {
		Wall wall = Instantiate(wallPrefab) as Wall;
		wall.Initialize(startpoint, endpoint, midpoint);
        walls.Add(wall);
    }
}