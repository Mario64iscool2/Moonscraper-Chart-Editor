// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using UnityEngine;
using System.Collections;

public class HandshapeTailController : SelectableClick {
    public HandshapeController hsCon;
    public ChartEditor editor;

    void Awake()
    {
        editor = ChartEditor.Instance;
    }

    public override void OnSelectableMouseDown()
    {
        if (!Input.GetMouseButtonDown(0) && Input.GetMouseButton(1))
        {
            hsCon.Reset();
            OnSelectableMouseDrag();
        }
    }

    public override void OnSelectableMouseDrag()
    {
        // Update sustain
        hsCon.DragCheck();
    }

    public override void OnSelectableMouseUp()
    {
        hsCon.OnSelectableMouseUp();
    }
}
