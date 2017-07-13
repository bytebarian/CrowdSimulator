using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vectrosity;

public struct Entry
{
    public string name;
    public string unit;
    public int decimals;
    public float value;
    public bool graphable;
    public float maxValue;

    public Entry(string n, string u, float val, int d, bool g, float m)
    {
        name = n;
        unit = u;
        decimals = d;
        value = val;
        graphable = g;
        maxValue = m;
    }
}

public struct CurrentValue
{
    public decimal time;
    public float value;
    public CurrentValue(decimal t, float v)
    {
        time = t;
        value = v;
    }
}

public class InfoText : MonoBehaviour
{

    private List<Entry> infos;

    private float maxDensity = float.MinValue;
    private float minDenstiy = float.MaxValue;
    private int crossings;

    private int activeEntry = -1;
    private VectorLine dataPoints;
    private List<VectorLine> diagramLines = new List<VectorLine>();
    private VectorLine timeIndicator;
    private Rect diagramPosition;

    PlaybackControl pc;

    private List<Label> xLabels = new List<Label>();

    private List<CurrentValue> currentSpeed = new List<CurrentValue>();
    private List<CurrentValue> currentDensity = new List<CurrentValue>();


    public struct Label
    {
        public Rect rect;
        public string label;

        public Label(Rect r, string s)
        {
            label = s;
            rect = r;
        }
    }


    private float maxSpeed = float.MinValue;
    private float minSpeed = float.MaxValue;


    // Use this for initialization
    void Start()
    {
        pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
    }

    // Update is called once per frame
    void Update()
    {

        InvokeRepeating("UpdateSecond", 0, 1.0f);

    }

    void UpdateSecond()
    {
        if (activeEntry > -1)
        {
            PlaybackControl pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
            float tx = ((float)(pc.current_time / pc.total_time)) * diagramPosition.width + diagramPosition.x;
            timeIndicator.points2 = new Vector2[] {
                new Vector2(tx,diagramPosition.y-2),
                new Vector2(tx,diagramPosition.y+8+diagramPosition.height)
            };
            timeIndicator.depth = 100;
            timeIndicator.Draw();

            Vector2[] dp = dataPoints.points2;
            try
            {
                dp[((int)pc.current_time) - 1] = new Vector2(diagramPosition.x + diagramPosition.width * ((float)(pc.current_time / pc.total_time)), diagramPosition.y + Mathf.Min(1.0f, (infos[activeEntry].value / infos[activeEntry].maxValue)) * diagramPosition.height);
                dataPoints.points2 = dp;
                dataPoints.Draw();
            }
            catch (System.IndexOutOfRangeException)
            {
            }
        }
    }

    void OnGUI()
    {

        PlaybackControl pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
        PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
        GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
        Groundplane gp = gl.groundplane;

        infos = new List<Entry>();

        string text = "";
        string minutes = Mathf.Floor((float)pc.current_time / 60).ToString("00"); ;
        string seconds = ((float)pc.current_time % 60).ToString("00"); ;
        text += "current time: " + minutes + ":" + seconds + "\n";

        if (pl.population != null && pl.population.Length > 0)
        {

            infos.Add(new Entry("population", "", pl.population[(int)pc.current_time], 0, true, pl.pedestrians.Count));
        }

        if (gp.point1active && gp.point2active)
        {
            infos.Add(new Entry("line length", "m", Vector3.Distance(gp.point1, gp.point2), 2, false, 0));
            infos.Add(new Entry("line crossings", "", gp.lineCrossed, 0, true, pl.pedestrians.Count));
            infos.Add(new Entry("line flow", "/s", gp.crossings.Count, 0, true, 10.0f));
            infos.Add(new Entry("avg. crossing speed", "m/s", gp.crossingSpeed, 2, true, 3.0f));
            infos.Add(new Entry("current flow", "/ms", gp.crossings.Count / Vector3.Distance(gp.point1, gp.point2), 2, true, 3.0f));
        }

        if (pc.tileColoringMode == TileColoringMode.TileColoringSpeed)
        {
            infos.Add(new Entry("average speed", "m/s", currentValue(currentSpeed), 2, true, 3.0f));
        }
        else
        {
            maxSpeed = float.MinValue;
            minSpeed = float.MaxValue;
        }

        if (pc.tileColoringMode == TileColoringMode.TileColoringDensity)
        {
            infos.Add(new Entry("average density", "/m²", currentValue(currentDensity), 2, true, 3.0f));
        }
        else
        {
            maxDensity = float.MinValue;
            minDenstiy = float.MaxValue;
            crossings = 0;
        }

        for (int i = 0; i < infos.Count; i++)
        {
            Entry e = infos[i];
            text += infos[i].name + ": " + System.Math.Round(e.value, e.decimals) + e.unit + "\n";
            if (e.graphable)
            {
                if (GUI.Toggle(new Rect(Screen.width * (transform.position.x) - 20, Screen.height * (1 - transform.position.y) - (15 * (infos.Count - i) + 17), 100, 15), i == activeEntry, "") && i != activeEntry)
                {
                    activeEntry = i;
                    Rect position = new Rect(30, 30, 400, 300);
                    position.x = Screen.width - position.x - position.width;
                }
            }
        }
        GetComponent<GUIText>().text = text;
    }

    public void updateSpeed(float s)
    {
        currentSpeed.Insert(0, new CurrentValue(pc.current_time, s));
        maxSpeed = Mathf.Max(maxSpeed, s);
        minSpeed = Mathf.Min(minSpeed, s);
    }

    public void updateDensity(float d)
    {

        currentDensity.Insert(0, new CurrentValue(pc.current_time, d));
        crossings++;
        maxDensity = Mathf.Max(maxDensity, d);
        minDenstiy = Mathf.Min(minDenstiy, d);
    }

    private float currentValue(List<CurrentValue> l)
    {
        float v = 0;
        for (int i = l.Count - 1; i >= 0; i--)
        {
            if (l[i].time < pc.current_time - 1) l.RemoveAt(i);
            else v += l[i].value;
        }
        return v / l.Count;
    }

}
