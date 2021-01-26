// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using UnityEngine;
using MoonscraperChartEditor.Song;

// Resticts so that only one fret tye can be placed down
public class PlaceFret : PlaceNote {
    [SerializeField]
    Note.GuitarFret standardFret;

    [SerializeField]
    Note.GHLiveGuitarFret ghliveFret;

    [SerializeField]
    Note.RealGuitarFret rsFret;

    protected override void Awake()
    {
        base.Awake();

        note.guitarFret = standardFret;
    }

    protected override void UpdateFretType()
    {
        if (Globals.RSMode)
        {
            note.rawNote = (int)rsFret;
        }
        else if (Globals.ghLiveMode)
        {
            note.rawNote = (int)ghliveFret;
        }
        else
        {
            note.rawNote = (int)standardFret;
        }
    }

    public override void ToolDisable()
    {
        // Don't set the current songobject, let the controller do that
    }
}
