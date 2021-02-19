using UnityEngine;
using UnityEngine.UI;
using MoonscraperChartEditor.Song;

public class HandshapePropertiesPanelController : PropertiesPanelController
{
    public Handshape currentHs { get { return (Handshape)currentSongObject; } set { currentSongObject = value; } }
    bool toggleBlockingActive;

    [SerializeField]
    Text sustainText;
    [SerializeField]
    InputField fret1, fret2, fret3, fret4, fret5, fret6;

    [SerializeField]
    PlaceHandshape handshapeToolController;

   Handshape prevHs = new Handshape(0, 0);

    // Start is called before the first frame update
    void Start()
    {
    }

    protected override void Update()
    {
        UpdateInputFieldsDisplay();

        UpdateStringsInfo();
        Controls();

        prevHs = currentHs;
    }

    bool IsInTool()
    {
        return editor.toolManager.currentToolId == EditorObjectToolManager.ToolID.Starpower;
    }

    void Controls()
    {
    }


    void UpdateStringsInfo()
    {
        positionText.text = "Position: " + currentHs.tick.ToString();
        sustainText.text = "Length: " + currentHs.length.ToString();
    }

    void UpdateInputFieldsDisplay()
    {

        fret1.text = currentHs.string1fret.ToString();
        fret2.text = currentHs.string2fret.ToString();
        fret3.text = currentHs.string3fret.ToString();
        fret4.text = currentHs.string4fret.ToString();
        fret5.text = currentHs.string5fret.ToString();
        fret6.text = currentHs.string6fret.ToString();

        bool inTool = IsInTool();

        if (!inTool && currentHs == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("No handshape loaded into note inspector");
        }
    }

    void UpdateFrets()
    {
        currentHs.string1fret = int.Parse(fret1.text);
        currentHs.string2fret = int.Parse(fret2.text);
        currentHs.string3fret = int.Parse(fret3.text);
        currentHs.string4fret = int.Parse(fret4.text);
        currentHs.string5fret = int.Parse(fret5.text);
        currentHs.string6fret = int.Parse(fret6.text);
    }
        

}
