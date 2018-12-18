using UnityEngine;

public class GeometryLoader : MonoBehaviour
{
    public Groundplane groundplane;  
    public Map mapInstance;
    public Map mapPrefab;

    public void LoadGeometry(string folderpath)
    {
        groundplane = new Groundplane();
        mapInstance = Instantiate(mapPrefab) as Map;
        mapInstance.LoadFromFile(folderpath + "map.xml");
    }
}

