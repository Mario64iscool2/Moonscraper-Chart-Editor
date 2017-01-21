﻿using UnityEngine;
using System.Collections;

public abstract class Snapable : MonoBehaviour {
    protected ChartEditor editor;
    
    protected uint objectSnappedChartPos = 0;
    protected Renderer objectRen;

    protected virtual void Awake()
    {
        editor = GameObject.FindGameObjectWithTag("Editor").GetComponent<ChartEditor>();
        objectRen = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        UpdateSnappedPos();

        transform.position = new Vector3(transform.position.x, editor.currentSong.ChartPositionToWorldYPosition(objectSnappedChartPos), transform.position.z);
    }

    protected virtual void Controls()
    {
    }

    protected void UpdateSnappedPos()
    {
        // Read in mouse world position
        if (Mouse.world2DPosition != null && ((Vector2)Mouse.world2DPosition).y < editor.mouseYMaxLimit.position.y)
        {
            Vector2 mousePos = (Vector2)Mouse.world2DPosition;
            float ypos = mousePos.y;

            objectSnappedChartPos = editor.currentSong.WorldPositionToSnappedChartPosition(ypos, Globals.step);
        }
        else
        {
            objectSnappedChartPos = editor.currentSong.WorldPositionToSnappedChartPosition(editor.mouseYMaxLimit.position.y, Globals.step);
        }
    }

    protected void LateUpdate()
    {
        if (objectRen)
        {
            objectRen.sortingOrder = 5;
        }

        Controls();
    }

    public static uint ChartPositionToSnappedChartPosition(uint chartPosition, int step, float resolution)
    {
        // Snap position based on step
        float factor = Globals.FULL_STEP / (float)step * resolution / Globals.STANDARD_BEAT_RESOLUTION;
        float divisor = chartPosition / factor;
        float lowerBound = (int)divisor * factor;
        float remainder = divisor - (int)divisor;

        if (remainder > 0.5f)
            chartPosition = (uint)Mathf.Round(lowerBound + factor);
        else
            chartPosition = (uint)Mathf.Round(lowerBound);

        return chartPosition;
    }
}
