// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using UnityEngine;
using MoonscraperChartEditor.Song;

[RequireComponent(typeof(HandshapeController))]
public class PlaceHandshape : PlaceSongObject {
    public Handshape handshape { get { return (Handshape)songObject; } set { songObject = value; } }
    new public HandshapeController controller { get { return (HandshapeController)base.controller; } set { base.controller = value; } }

    Handshape lastPlacedHS = null;
    Renderer hsRen;

    protected override void SetSongObjectAndController()
    {
        handshape = new Handshape(0, 0);

        controller = GetComponent<HandshapeController>();
        controller.handshape = handshape;
        hsRen = GetComponent<Renderer>();
    }

    protected override void Controls()
    {
        if (!Globals.gameSettings.keysModeEnabled)
        {
            if (Input.GetMouseButton(0))
            {
                if (lastPlacedHS == null)
                {
                    AddObject();
                }
                else
                {
                    UpdateLastPlacedHs();
                }
            }
        }
        else if (MSChartEditorInput.GetInput(MSChartEditorInputActions.AddSongObject))
        {
            if (MSChartEditorInput.GetInputDown(MSChartEditorInputActions.AddSongObject))
            {
                var searchArray = editor.currentChart.handShape;
                int pos = SongObjectHelper.FindObjectPosition(handshape, searchArray);
                if (pos == SongObjectHelper.NOTFOUND)
                {
                    AddObject();
                }
                else
                {
                    editor.commandStack.Push(new SongEditDelete(searchArray[pos]));
                }
            }
            else if (lastPlacedHS != null)
            {
                UpdateLastPlacedHs();
            }
        }
    }

    void UpdateLastPlacedHs()
    {
        uint prevHsLength = lastPlacedHS.length;

        uint newLength = lastPlacedHS.GetCappedLengthForPos(objectSnappedChartPos);

        if (prevHsLength != newLength)
        {
            editor.commandStack.Pop();
            editor.commandStack.Push(new SongEditAdd(new Handshape(lastPlacedHS.tick, newLength)));
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        handshape.chart = editor.currentChart;
        if (!Globals.gameSettings.keysModeEnabled)
            hsRen.enabled = lastPlacedHS == null;

        base.Update();

        if ((Input.GetMouseButtonUp(0) && !Globals.gameSettings.keysModeEnabled) || (Globals.gameSettings.keysModeEnabled && Input.GetButtonUp("Add Object")))
        {
            // Reset
            lastPlacedHS = null;
        } 
    }

    protected override void OnEnable()
    {
        editor.selectedObjectsManager.currentSelectedObject = handshape;

        base.OnEnable();
    }

    protected override void AddObject()
    {
        editor.commandStack.Push(new SongEditAdd(new Handshape(handshape)));

        int insertionIndex = SongObjectHelper.FindObjectPosition(handshape, editor.currentChart.handShape);
        Debug.Assert(insertionIndex != SongObjectHelper.NOTFOUND, "Song event failed to be inserted?");
        lastPlacedHS = editor.currentChart.handShape[insertionIndex];
    }
}
