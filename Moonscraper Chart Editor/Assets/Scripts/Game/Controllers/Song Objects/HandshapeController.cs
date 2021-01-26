// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using UnityEngine;
using MoonscraperChartEditor.Song;


public class HandshapeController : SongObjectController
{
    public GameObject tail;
    public Handshape handshape { get { return (Handshape)songObject; } set { Init(value, this); } }
    public const float position = -3.0f;
    
    Handshape unmodifiedHS = null;
    bool wantPop = false;

    new void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Reset();
    }

    protected override void UpdateCheck()
    {
        if (handshape != null)
        {
            uint endPosition = handshape.tick + handshape.length;

            if ((handshape.tick >= editor.minPos && handshape.tick < editor.maxPos) ||
                    (endPosition > editor.minPos && endPosition < editor.maxPos) ||
                    (handshape.tick < editor.minPos && endPosition >= editor.maxPos))
            {
                ChartEditor.State currentState = editor.currentState;
                if (currentState == ChartEditor.State.Editor)
                {
                    UpdateSongObject();
                }
            }
            else
                gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(false);
    }

    public override void UpdateSongObject()
    {
        if (handshape.song != null)
        {
            transform.position = new Vector3(CHART_CENTER_POS + position, desiredWorldYPosition, 0);

            UpdateTailLength();
        }
    }

    public void UpdateTailLength()
    {
        float length = handshape.song.TickToWorldYPosition(handshape.tick + handshape.length) - desiredWorldYPosition;

        Vector3 scale = tail.transform.localScale;
        scale.y = length;
        tail.transform.localScale = scale;

        Vector3 position = transform.position;
        position.y += length / 2.0f;
        tail.transform.position = position;
    }

    void TailDrag()
    {
        uint snappedChartPos;

        if (editor.services.mouseMonitorSystem.world2DPosition != null && ((Vector2)editor.services.mouseMonitorSystem.world2DPosition).y < editor.mouseYMaxLimit.position.y)
        {
            snappedChartPos = Snapable.TickToSnappedTick(handshape.song.WorldYPositionToTick(((Vector2)editor.services.mouseMonitorSystem.world2DPosition).y), Globals.gameSettings.step, handshape.song);           
        }
        else
        {
            snappedChartPos = Snapable.TickToSnappedTick(handshape.song.WorldYPositionToTick(editor.mouseYMaxLimit.position.y), Globals.gameSettings.step, handshape.song);
        }

        // Cap to within the range of the song
        snappedChartPos = (uint)Mathf.Min(editor.maxPos, snappedChartPos);

        uint newLength = handshape.GetCappedLengthForPos(snappedChartPos);
        if (newLength != handshape.length)
        {
            if (wantPop)
                editor.commandStack.Pop();

            editor.commandStack.Push(new SongEditModify<Handshape>(handshape, new Handshape(handshape.tick, newLength)));

            wantPop = true;
        }
    }

    public override void OnSelectableMouseDown()
    {
        Reset();
        base.OnSelectableMouseDown();
    }

    public override void OnSelectableMouseDrag()
    {
        // Move note
        if (!DragCheck())
            base.OnSelectableMouseDrag();
    }

    public bool DragCheck()
    {
        if (editor.currentState == ChartEditor.State.Editor && Input.GetMouseButton(1))
        {
            if (unmodifiedHS == null)
                unmodifiedHS = (Handshape)handshape.Clone();

            TailDrag();
            return true;
        }

        return false;
    }

    public override void OnSelectableMouseUp()
    {
        Reset();
    }

    public void Reset()
    {
        unmodifiedHS = null;
        wantPop = false;
    }
}
