using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLightManager : MonoBehaviour {

    private List<Light> lights = new List<Light>();
    private List<Material> mats = new List<Material>();
    public float strobeRate = 1.3f;
    private float maxIntensity = 2.21f;

	// Use this for initialization
	void Start () {
        GameObject[] g = GameObject.FindGameObjectsWithTag("WarningLights");
        foreach (GameObject go in g)
        {
            lights.Add(go.GetComponentInChildren<Light>());
        }

        for (int i = 0; i < lights.Count; i++)
        {
            foreach (Material m in lights[i].transform.parent.GetComponentInChildren<Renderer>().materials)
            {
                if (m.name.Equals("Alarm_Light (Instance)") || m.name.Equals("Alarm_Light"))
                    mats.Add(m);
            }
        }

        // Debug.Log(mats.Count);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].intensity = Mathf.PingPong(Time.time * strobeRate, maxIntensity);
        }

        foreach (Material m in mats)
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", new Color(m.color.r, m.color.g, m.color.b, m.color.a) * Mathf.PingPong(Time.time * strobeRate, maxIntensity));

        }
    }
}
