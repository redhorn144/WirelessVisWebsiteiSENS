using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    public static Options Instance;
    [SerializeField] GameObject Start;
    [SerializeField] GameObject LineToggle;
    [SerializeField] GameObject PacketToggle;
    [SerializeField] GameObject CameraToggle;
    [SerializeField] GameObject ColormapDropdown;
    //[SerializeField]
    [SerializeField] GameObject TInput;
    [SerializeField] GameObject RInput;
    [SerializeField] GameObject Selector;

    private Button s;
    private Toggle lines;
    private Toggle packets;
    private TMPro.TMP_Dropdown colormaps;
    private TMPro.TMP_InputField Tx;
    private TMPro.TMP_InputField Rx;

    public int Tnum;
    public int Rnum;

    public bool linesOn;
    public bool packetsOn;
    public string mapselection;
    public string data;

    

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        s = Start.GetComponent<Button>();
        s.onClick.AddListener(StartVisualization);

        lines = LineToggle.GetComponent<Toggle>();
        packets = PacketToggle.GetComponent<Toggle>();
        colormaps = ColormapDropdown.GetComponent<TMPro.TMP_Dropdown>();

        Tx = TInput.GetComponent<TMPro.TMP_InputField>();
        Rx = RInput.GetComponent<TMPro.TMP_InputField>();
        Tx.onEndEdit.AddListener(GetTnum);
        Rx.onEndEdit.AddListener(GetRnum);
    }

    void GetTnum(string num)
    {
        Tnum = int.Parse((Tx.text.ToString()));
    }

    void GetRnum(string num)
    {
        Rnum = int.Parse((Rx.text.ToString()));
    }

    void StartVisualization()
    {
        linesOn = lines.isOn;
        packetsOn = packets.isOn;

        if (colormaps.value == 0)
        {
            mapselection = "viridis";
        }
        else if(colormaps.value == 1)
        {
            mapselection = "plasma";
        }
        else
        {
            mapselection = "magma";
        }

        data = Selector.GetComponent<FileUpload>().data;
        
        if (CameraToggle.GetComponent<Toggle>().isOn)
        {
            SceneManager.LoadScene("fixedCameraScene", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("freeCameraScene", LoadSceneMode.Single);
        }
       
    }

}
