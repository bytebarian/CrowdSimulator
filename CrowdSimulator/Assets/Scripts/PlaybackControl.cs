using System;
using UnityEngine;

public class PlaybackControl : MonoBehaviour
{
    public bool playing = true;
    public decimal current_time;
    public decimal slider_value;
    public decimal total_time;
    public int tiles = 0;
    public bool drawLine;
    public TileColoringMode tileColoringMode = TileColoringMode.TileColoringNone;
    public bool trajectoriesShown;
    public float threshold;

    bool lineIsDrawn;

    private Map mazeInstance;

	private void Start () {
        current_time = 0;
        total_time = 0;
        playing = true;
        threshold = 2.0f;
        StartSimulaton();
	}

    void OnGUI()
    {
        string btnText = "PLAY";
        if (playing) btnText = "PAUSE";
        if (GUI.Button(new Rect(40, 20, 120, 30), btnText))
        {
            playing = !playing;
        }

        current_time = (decimal)GUI.HorizontalSlider(new Rect(170, 30, 400, 30), (float)current_time, 0.0f, (float)total_time);

        btnText = "show trajectories";
        if (trajectoriesShown) btnText = "hide trajectories";

        if (GUI.Button(new Rect(40, 60, 120, 30), btnText))
        {
            PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
            if (trajectoriesShown)
            {
                foreach (GameObject p in pl.pedestrians)
                {
                    p.GetComponent<Pedestrian>().hideTrajectory();
                }
                trajectoriesShown = false;
            }
            else
            {
                foreach (GameObject p in pl.pedestrians)
                {
                    p.GetComponent<Pedestrian>().showTrajectory();
                }
                trajectoriesShown = true;
            }
        }

        tileColoringMode = TileColoringMode.TileColoringDensity;

        GUIStyle style = new GUIStyle();
        style.normal.background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        style.normal.background.SetPixel(0, 0, new Color(1f, 0, 0));
        style.normal.background.Apply();
        GUI.Label(new Rect(40, 150, 5, 30), string.Empty, style);
        GUI.Label(new Rect(65, 145, 30, 30), "1,50");

        style.normal.background.SetPixel(0, 0, new Color(1f, 0.6f, 0));
        style.normal.background.Apply();
        GUI.Label(new Rect(40, 180, 5, 30), string.Empty, style);
        GUI.Label(new Rect(65, 175, 30, 30), "1,25");

        style.normal.background.SetPixel(0, 0, new Color(1f, 1f, 0));
        style.normal.background.Apply();
        GUI.Label(new Rect(40, 210, 5, 30), string.Empty, style);
        GUI.Label(new Rect(65, 205, 30, 30), "1,00");

        style.normal.background.SetPixel(0, 0, new Color(0.6f, 1f, 0));
        style.normal.background.Apply();
        GUI.Label(new Rect(40, 240, 5, 30), string.Empty, style);
        GUI.Label(new Rect(65, 235, 30, 30), "0,75");

        style.normal.background.SetPixel(0, 0, new Color(0, 1f, 0));
        style.normal.background.Apply();
        GUI.Label(new Rect(40, 270, 5, 30), string.Empty, style);
        GUI.Label(new Rect(65, 265, 30, 30), "0,50");

        style.normal.background.SetPixel(0, 0, new Color(0, 0.6f, 1f));
        style.normal.background.Apply();
        GUI.Label(new Rect(40, 300, 5, 30), string.Empty, style);
        GUI.Label(new Rect(65, 295, 30, 30), "0,25");
    }

    public void lineDrawn()
    {
        drawLine = false;
        lineIsDrawn = true;
    }

    private void Update ()
    {
        if (playing)
        {
            try
            {
                current_time = (current_time + (decimal)Time.deltaTime) % total_time;
            }
            catch (DivideByZeroException)
            {
                current_time = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playing = !playing;
        }
    }

	private void StartSimulaton ()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            path = "Data/";
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            path = "../../";
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = "Data/";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path = "../";
        }

        var gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
        gl.LoadGeometry(path);

        var pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
        pl.LoadFromFile(gl.mapInstance, path);
    }

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		StartSimulaton();
	}
}