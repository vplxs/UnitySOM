using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour {
    //public Material material;

    [HideInInspector]
    public float[] values;

    private Material m_material;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(int dimSize)
    {
        values = new float[dimSize];

        for (int i=0; i<values.Length; i++)
        {
            values[i] = Random.Range(0.0f, 1.0f);
        }

        m_material = new Material(GetComponent<MeshRenderer>().material);
        m_material.color = new Color(values[0], values[1], values[2]);
        GetComponent<MeshRenderer>().material = m_material;
    }

    public Color GetColor()
    {
        return m_material.color;
    }
    
    public void PlaceInSpace()
    {
        transform.localPosition = new Vector3(values[0], values[1], values[2]);
    }
}
