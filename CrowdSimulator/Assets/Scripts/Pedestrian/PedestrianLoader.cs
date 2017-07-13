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

    public void LoadFromFile(Maze maze, string foldername)
    {
        var sr = new StreamReader(Application.dataPath + "/" + foldername + "pedestrians.txt");
        var fileContent = sr.ReadToEnd();
        sr.Close();

        var entities = fileContent.Split("\n"[0]);
        var init = entities[0].Split(':');
        var timestepsNumber = int.Parse(init[0]);
        var timestepLenght = decimal.Parse(init[1]);

        var pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
        pc.total_time = timestepsNumber * timestepLenght;
        population = new int[timestepsNumber];
        int id = 1;
        foreach (var entity in entities)
        {
            var timestep = 0;
            var parts = entity.Split(';');
            if (parts.Length != 2) continue;
            var startPos = parts[0].Split(':');
            var startVector = new IntVector2(int.Parse(startPos[0]), maze.size.z - int.Parse(startPos[1]));
            var mov = parts[1].Split(',').Select(x => (MovementDirection)int.Parse(x));
            var positions = new List<PedestrianPosition>();
            var mazePos = maze.GetPostionOnMaze(startVector);
            population[timestep]++;
            positions.Add(new PedestrianPosition(id++, timestep++, mazePos.x, mazePos.z));
            
            positions.AddRange(mov.Select(x =>
            {
                startVector += x.ToIntVector2();
                
                var nextPos = maze.GetPostionOnMaze(startVector);
                population[timestep]++;
                return new PedestrianPosition(id, timestep++, nextPos.x, nextPos.z);
            }));

            var p = (GameObject)Instantiate(Resources.Load("Pedestrian"));
            p.transform.parent = null;
            var pedestrianPostions = new SortedList();
            positions.ForEach(pos => pedestrianPostions.Add(pos.getTime(), pos));
            p.GetComponent<Pedestrian>().setPositions(pedestrianPostions);
            p.GetComponent<Pedestrian>().setID(id);
            pedestrians.Add(p);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
