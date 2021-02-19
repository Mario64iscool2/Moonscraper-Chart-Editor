// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using UnityEngine;
using MoonscraperChartEditor.Song;

public class HandshapePool : SongObjectPool
{
    public HandshapePool(GameObject parent, GameObject prefab, int initialSize) : base(parent, prefab, initialSize)
    {
        if (!prefab.GetComponentInChildren<HandshapeController>())
            throw new System.Exception("No HandshapeController attached to prefab");
    }

    protected override void Assign(SongObjectController sCon, SongObject songObject)
    {
        HandshapeController controller = sCon as HandshapeController;

        // Assign pooled objects
        controller.handshape = (Handshape)songObject;
        controller.gameObject.SetActive(true);
    }
}
