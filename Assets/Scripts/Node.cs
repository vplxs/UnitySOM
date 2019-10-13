using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public GameObject panel;
    public Transform panelParent;
    public GameObject cube;
    public Transform cubeParent;
    public Material material;
    public MaterialPropertyBlock props;

    [HideInInspector]
    public float[] weight;

    private int idX = -1;
    private int idY = -1;

    public Vector3 panelPos { get; set; }
    public Vector3 cubePos { get; set; }

    private Material m_material;

    private GameObject m_panel;
    private GameObject m_cube;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Public Methods
    public void Initialize(int idX, int idY, float size, int dimSize)
    {
        this.idX = idX;
        this.idY = idY;

        panelPos = new Vector3(idX * size + size / 2.0f, idY * size + size / 2.0f);

        weight = new float[dimSize];
        for (int i=0; i<weight.Length; i++)
        {
            weight[i] = Random.Range(0.0f, 1.0f);
        }

        m_material = new Material(material);
        m_material.color = new Color(weight[0], weight[1], weight[2]);

        m_panel = Instantiate(panel, panelParent);
        m_panel.GetComponent<MeshRenderer>().material = m_material;
        m_panel.transform.localEulerAngles = new Vector3(-90, 0, 0);
        m_panel.transform.localPosition = panelPos;
        m_panel.transform.localScale = panel.transform.localScale;

        m_cube = Instantiate(cube, cubeParent);
        m_cube.GetComponent<MeshRenderer>().material = m_material;
        m_cube.transform.localEulerAngles = Vector3.zero;
        m_cube.transform.localPosition = cubePos;
        m_cube.transform.localScale = cube.transform.localScale;


    }

    public float dist(Input sample)
    {
        float squares = 0;
        for (int i=0; i<weight.Length; i++)
        {
            squares += (weight[i] - sample.values[i]) * (weight[i] - sample.values[i]);
        }
        return Mathf.Sqrt(squares);
    }

    public float dist(Node n)
    {
        float sqrX = (idX - n.idX) * (idX - n.idX);
        float sqrY = (idY - n.idY) * (idY - n.idY);
        return Mathf.Sqrt(sqrX+sqrY);
    }

    public void PlacePanels()
    {
        m_material.color = new Color(weight[0], weight[1], weight[2]);
        m_panel.transform.localPosition = panelPos;
    }

    public void PlaceCubes()
    {
        cubePos = new Vector3(weight[0], weight[1], weight[2]);
        m_material.color = new Color(weight[0], weight[1], weight[2]);
        m_material.SetColor("_EmissionColor", m_material.color);
        m_cube.transform.localPosition = cubePos;
    }
    #endregion
}
