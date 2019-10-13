using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Network : MonoBehaviour
{

    public int numX;
    public int numY;
    public int dimSize;
    public float panelSize;
    public float learningRate = 0.01f;
    public float window = 0.2f;
    public int samplesNum;

    public Transform panelParent;
    public Transform cubeParent;
    public Node node;
    public GameObject input;
    public Transform inputParent;

    public MaterialPropertyBlock props;

    [HideInInspector]
    public Node[][] nodes;

    [HideInInspector]
    public List<Input> samples;
    private float m_originalWindow;

    // Use this for initialization
    void Start()
    {
        m_originalWindow = window;
        //GenerateRandomSamples(samplesNum);
        //inputParent.gameObject.SetActive(false);
        samples = new List<Input>();
        Initialize();

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (UnityEngine.Input.GetKeyDown(KeyCode.M))
        {
            var obj = Instantiate(input, inputParent);
            samples.Add(obj.GetComponent<Input>());
            samples.Last().Initialize(dimSize);
            obj.transform.localPosition = new Vector3(samples.Last().values[0], samples.Last().values[1], samples.Last().values[2]);
            window = m_originalWindow;
        }

        if (Time.frameCount % 1 == 0)
        {
            if (samples.Count > 0)
            {
                window *= 0.995f;
                Train(samples);
                for (int i = 0; i < nodes.Length; i++)
                {
                    for (int j = 0; j < nodes[i].Length; j++)
                    {
                        nodes[i][j].PlaceCubes();
                        nodes[i][j].PlacePanels();
                    }
                }
            }
        }

    }

    public void ResetWindow()
    {
        window = m_originalWindow;
    }

    public void GenerateRandomSamples(int samplesNum)
    {

        for (int i = 0; i < samplesNum; i++)
        {
            var obj = Instantiate(input, inputParent);
            samples.Add(obj.GetComponent<Input>());
            samples[i].Initialize(dimSize);
            obj.transform.localPosition = new Vector3(samples[i].values[0], samples[i].values[1], samples[i].values[2]);
        }
    }

    public void Initialize()
    {
        nodes = new Node[numX][];
        props = new MaterialPropertyBlock();
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node[numY];
            for (int j = 0; j < nodes[i].Length; j++)
            {
                nodes[i][j] = Instantiate(node, transform);
                nodes[i][j].gameObject.name = "Node_" + i + "_" + j;
                nodes[i][j].cubeParent = cubeParent;
                nodes[i][j].panelParent = panelParent;
                nodes[i][j].Initialize(i, j, panelSize, dimSize);
                nodes[i][j].props = props;
            }
        }
    }

    public void Train(List<Input> samples)
    {
        for (int i = 0; i < samples.Count; i++)
        {
            TrainOne(samples[i]);
        }
    }

    public void TrainOne(Input sample)
    {
        Node win = FindClosest(sample);
        TrainNeighbours(sample, win);
    }

    public Node FindClosest(Input sample)
    {
        float minDist = float.MaxValue;
        Node win = null;

        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i].Length; j++)
            {
                float distance = nodes[i][j].dist(sample);
                if (distance < minDist)
                {
                    minDist = distance;
                    win = nodes[i][j];
                }
            }
        }
        return win;
    }

    public void TrainNeighbours(Input sample, Node win)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i].Length; j++)
            {
                float distance = nodes[i][j].dist(win);
                //print(string.Format("distance between {0} and {1} is: {2} with Gauss: {3}", win.gameObject.name, nodes[i][j].gameObject.name, distance, Gauss(distance, 0, window)));
                for (int k = 0; k < nodes[i][j].weight.Length; k++)
                {
                    nodes[i][j].weight[k] += learningRate * Gauss(distance, 0, window) * (sample.values[k] - nodes[i][j].weight[k]);

                }
            }
        }
        //print("Trained " + win.gameObject.name);
    }


    /// <summary>
    /// Returns from [0,1]
    /// </summary>
    /// <param name="x">the position of x</param>
    /// <param name="m">the position of the pick</param>
    /// <param name="s">the spread(window)</param>
    /// <returns></returns>
    public static float Gauss(float x, float m, float s)
    {
        float a = 1;
        float b = m;
        float c = s;

        float ePowTop = (x - b) * (x - b);
        float ePowBottom = 2 * c * c;
        float ePow = -ePowTop / ePowBottom;

        float f = Mathf.Exp(ePow) * a;

        return f;
    }
}
