using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PedestrianLoader : MonoBehaviour
{
    private List<PedestrianPosition> positions = new List<PedestrianPosition>();
    public List<GameObject> pedestrians = new List<GameObject>();
    public int[] population;

    // Use this for initialization
    void Start()
    {

    }

    public void LoadFromFile(Map maze, string foldername)
    {
        var sr = new StreamReader(Application.dataPath + "/" + foldername + "pedestrians.txt");
        var fileContent = sr.ReadToEnd();
        sr.Close();

    }


    // Update is called once per frame
    void Update()
    {

    }
}
