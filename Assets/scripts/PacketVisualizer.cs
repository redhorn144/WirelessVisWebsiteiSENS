using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PacketVisualizer : MonoBehaviour
{
    [SerializeField] CSVReader reader;
    [SerializeField] Colormap colormap;
    [SerializeField] float speed = 10.0f;
    [SerializeField] float frameRate = 2.0f;
    [SerializeField] float startWidth = 0.05f;
    [SerializeField] float endWidth = 0.025f;
    [SerializeField] float trailTime = 0.5f;
    [SerializeField] GameObject transmitter;
    [SerializeField] GameObject reciever;
    [SerializeField] int RecieverNumber;
    [SerializeField] bool SeeLines = true;
    [SerializeField] bool SeePackets = true;
    [SerializeField] string mapName = "viridis";
    [SerializeField] GameObject PmaxText;
    [SerializeField] GameObject PminText;
    [SerializeField] GameObject ViridisImage;
    [SerializeField] GameObject PlasmaImage;
    [SerializeField] GameObject MagmaImage;
    [SerializeField] int threshNum;

    private GameObject options;
    private Color startColor = Color.red;
    private Color colorChange;
    private bool firstCheck = false;
    private float time = 0.0f;
    private List<CSVReader.Path> paths;
    
    private List<LineRenderer> lines;
    private List<Packet> packets;
    private float frameTime;
    private float minPower;
    private float maxPower;
    private bool moved = false;

    public class Packet
    {
        public GameObject sphere;
        public List<Vector3> path;
        public float length;
        public int subdivisions;
        public List<Vector3> dividedPath;
        public int idx;
        public float power;

        public void MakeDividedPath()
        {
            idx = 0;
            //Debug.Log(idx);
            float divisionLength = length / (float)subdivisions;
            List<float> partials = new List<float>();
            List<Vector3> normalizedPartials = new List<Vector3>();
            dividedPath = new List<Vector3>();
            

            for (int i = 0; i < path.Count - 1; i++)
            {
                partials.Add((path[i+1] - path[i]).magnitude);
                normalizedPartials.Add((path[i+1] - path[i]).normalized);
            }
            
            dividedPath.Add(path[0]);
            float sumLength = 0;
            int pathIdx = 0;

            for (int i = 0; i < subdivisions; i++)
            {
                if (sumLength < partials[pathIdx])
                {
                    dividedPath.Add(dividedPath.Last() + divisionLength*normalizedPartials[pathIdx]);
                    sumLength += divisionLength;
                    //Debug.Log(dividedPath.Last());
                }
                else
                {
                    sumLength = 0;
                    pathIdx++;
                    i--;
                }
            }
            dividedPath.Add(path.Last());

        }
    }

    private void Start()
    {

        options = GameObject.FindGameObjectsWithTag("options")[0];
        var scrpt = options.GetComponent<Options>();
        RecieverNumber = scrpt.Rnum;
        SeeLines = scrpt.linesOn;
        SeePackets = scrpt.packetsOn;
        mapName = scrpt.mapselection;
        if (mapName == "viridis")
        {
            ViridisImage.SetActive(true);
        }
        else if (mapName == "plasma")
        {
            PlasmaImage.SetActive(true);
        }
        else
        {
            MagmaImage.SetActive(true);
        }

        //
        packets = new List<Packet>();
        frameTime = 1.0f / frameRate;
        //colorChange = new Color(-0.005f, 0.0f, 0.005f, 0.0f);
        lines = new List<LineRenderer>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (true)
        {


            
            //For creating line renderer object
            if (!firstCheck)
            {   
                var pathsTemp = reader.pathList;
                paths = ProcessReader(pathsTemp, RecieverNumber); 

                threshNum = Math.Min(threshNum, paths.Count);
                float thresh = PathThresh(paths, threshNum);
                minPower = paths[0].Path_power;
                maxPower = paths[0].Path_power;
                
                foreach (CSVReader.Path path in paths)
                {
                    if (path.Path_power >= thresh)
                    {
                        if (path.Path_power < minPower)
                        {
                            minPower = path.Path_power;
                        }
                        if (path.Path_power > maxPower)
                        {
                            maxPower = path.Path_power;
                        }
                    }
                }
                
                PmaxText.GetComponent<TMPro.TMP_Text>().text = $"{maxPower}";
                PminText.GetComponent<TMPro.TMP_Text>().text = $"{minPower}";

                foreach (CSVReader.Path path in paths)
                {
                    if (path.Path_power >= thresh)
                    {
                        if (!moved)
                        {
                            transmitter.transform.position = SimToUnity(path.coordinates[0]);
                            reciever.transform.position = SimToUnity(path.coordinates.Last());
                            moved = true;
                        }
                        if (SeePackets)
                        {
                            SpawnPacket(PathProcessing(path.coordinates), path.Path_power);
                        }
                        if (SeeLines)
                        {
                            RenderLine(PathProcessing(path.coordinates), path.Path_power);
                        }
                    }
                }
                
                AddTrails();
                StartCoroutine(PacketTrajector());
                firstCheck = true;
            }
            else if (time > 10)
            {
                GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("packet");
                foreach (var dest in toDestroy)
                {
                    Destroy(dest);
                }
                time = 0.0f;
                
                packets = new List<Packet>();

                float thresh = PathThresh(paths, threshNum);

                foreach (CSVReader.Path path in paths)
                {
                    if (path.Path_power >= thresh)
                    {
                        if (!moved)
                        {
                            transmitter.transform.position = SimToUnity(path.coordinates[0]);
                            reciever.transform.position = SimToUnity(path.coordinates.Last());
                            moved = true;
                        }
                        if (SeePackets)
                        {
                            SpawnPacket(PathProcessing(path.coordinates), path.Path_power);
                        }
                        
                    }
                }
                
                AddTrails();
                StartCoroutine(PacketTrajector());
                firstCheck = true;
            }

        }
    }

    private List<CSVReader.Path> ProcessReader(List<CSVReader.Path> pathList, int Rnum)
    {
        var outList = new List<CSVReader.Path>();
        foreach(var path in pathList)
        {
            if (path.Rx_Number == Rnum)
            {
                outList.Add(path);
            }
        }
        return outList;
    }

    private float PathThresh(List<CSVReader.Path> pathList, int numPaths)
    {
        
        pathList.Sort((x, y) => x.Path_power.CompareTo(y.Path_power));
        pathList.Reverse();
        return pathList[numPaths].Path_power;

    }


    private void SpawnPacket(List<Vector3> coordList, float power)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var scale = new Vector3(0.025f, 0.025f, 0.025f);
        sphere.transform.localScale = scale;
        sphere.transform.position = coordList[0];
        sphere.tag = "packet";
        
        Packet pack = new Packet();
        pack.power = power;
        pack.sphere = sphere;
        pack.path = coordList;
        pack.length = PathLength(coordList);
        pack.subdivisions = (int)Math.Floor(pack.length*frameRate/speed);
        pack.MakeDividedPath();
        packets.Add(pack);
    }

    IEnumerator PacketTrajector()
    {
        
        
        for(;;)
        {
            foreach (Packet pack in packets)
            {
                Debug.Log("iterating through packets");
                if (pack.idx < pack.dividedPath.Count)
                {
                    pack.sphere.transform.position = pack.dividedPath[pack.idx];
                    pack.idx++;
                    //pack.sphere.GetComponent<TrailRenderer>().startColor = startColor;
                    //pack.sphere.GetComponent<TrailRenderer>().endColor = startColor;

                    

                }
            }
            yield return new WaitForSeconds(frameTime);
        }

    }

    private float PathLength(List<Vector3> coordList)
    {
        float dist = 0.0f;
        
        for (int i = 0; i < coordList.Count - 1; i++)
        {
            dist += (coordList[i + 1] - coordList[i]).magnitude;
        }

        return dist;
    }

    private void AddTrails()
    {
        foreach(Packet p in packets)
        {
            p.sphere.AddComponent<TrailRenderer>();
            TrailRenderer tr = p.sphere.GetComponent<TrailRenderer>();
            tr.material = new Material(Shader.Find("Sprites/Default"));
            Color c = colormap.GetColor(p.power, minPower, maxPower, mapName);
            //Color c = new Color(0,0,0);
            
            tr.startColor = c;
            tr.endColor = c;
            tr.startWidth = startWidth;
            tr.endWidth = endWidth;
            tr.time = trailTime;
        }
    }

    private float GetAlpha(float power)
    {
        return (1/((maxPower - minPower)*0.75f))*(power - minPower) + 0.75f;
    }

    private void RenderLine(List<Vector3> coordList, float power)
    {
        Color c = colormap.GetColor(power, minPower, maxPower, mapName);
        //Debug.Log(c);
        c.a = GetAlpha(power);
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lines.Add(lineRenderer);
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = coordList.Count;
        lineRenderer.SetPositions(coordList.ToArray());
    }

    private List<Vector3> PathProcessing(List<Vector3> path)
    {
        List<Vector3> outPath = new List<Vector3>();
        foreach (var vec in path)
        {
            outPath.Add(SimToUnity(vec));
        }
        
        return outPath;
    }

    private Vector3 SimToUnity(Vector3 input)
    {
        var converted = new Vector3(-1*input[0], input[2], -1*input[1]);
        return converted;
    }

    private Vector3 ExtraTransform(Vector3 input)
    {
        //var converted = new Vector3(-1*input[2], input[0], input[1]);
        var converted = new Vector3(-1f*input[2], input[0], input[1]);
        return converted;
    }
}
