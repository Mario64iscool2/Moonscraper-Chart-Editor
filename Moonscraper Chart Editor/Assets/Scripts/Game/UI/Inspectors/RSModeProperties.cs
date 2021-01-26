// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using MoonscraperChartEditor.Song;

public class RSModeProperties : UpdateableService
{
    enum LaneCountOptions
    {
        // Keep this in the same order as the UI
        LaneCount6,
        LaneCount5,
        LaneCount4,     
    }
    

    [SerializeField]
    Dropdown m_laneCountDropdown;

    [SerializeField]
    Dropdown m_arrOptionDropdown;

    [SerializeField]
    Toggle isAltArr;



    readonly static Dictionary<LaneCountOptions, int> r_laneOptionToLaneCount = new Dictionary<LaneCountOptions, int>()
    {
        { LaneCountOptions.LaneCount6, 6 },
        { LaneCountOptions.LaneCount5, 5 },
        { LaneCountOptions.LaneCount4, 4 },
    };   

    readonly static Dictionary<int, LaneCountOptions> r_laneCountToLaneOption = r_laneOptionToLaneCount.ToDictionary((i) => i.Value, (i) => i.Key);

    protected override void Start()
    {
        base.Start();

        ChartEditor editor = ChartEditor.Instance;
        editor.events.chartReloadedEvent.Register(OnChartReload);

        
        OnChartReload();
    }

    public override void OnServiceUpdate()
    {
        
    }

    void OnChartReload()
    {
        bool isRS = ChartEditor.Instance.currentChart.gameMode == Chart.GameMode.RealInstrument;
        gameObject.SetActive(isRS);

        if (isRS)
        {
            LaneCountOptions option;
            Song.Instrument chartType = ChartEditor.Instance.currentInstrument;
            GameSettings.ArrangementTypeOptions arr;

            arr = Globals.gameSettings.GetArrangementType(chartType);

            switch (chartType)
            {
                case Song.Instrument.RealGuitar:
                    if (!r_laneCountToLaneOption.TryGetValue(Globals.gameSettings.realGLaneCount, out option))
                    {
                        option = LaneCountOptions.LaneCount6;
                    }
                    break;
                case Song.Instrument.RealBass:
                    if (!r_laneCountToLaneOption.TryGetValue(Globals.gameSettings.realBLaneCount, out option))
                    {
                        option = LaneCountOptions.LaneCount4;
                    }
                    break;
                case Song.Instrument.RealGuitar22:
                    if (!r_laneCountToLaneOption.TryGetValue(Globals.gameSettings.realG22LaneCount, out option))
                    {
                        option = LaneCountOptions.LaneCount6;
                    }
                    break;
                case Song.Instrument.RealBass22:
                    if (!r_laneCountToLaneOption.TryGetValue(Globals.gameSettings.realB22LaneCount, out option))
                    {
                        option = LaneCountOptions.LaneCount4;
                    }
                    break;
                case Song.Instrument.BonusRealGuitar:
                    if (!r_laneCountToLaneOption.TryGetValue(Globals.gameSettings.bonusRealLaneCount, out option))
                    {
                        option = LaneCountOptions.LaneCount6;
                    }
                    break;
                default:
                    option = LaneCountOptions.LaneCount6;
                        break;
            }

            int intLastKnownLaneCount = (int)option;
            bool forceReload = intLastKnownLaneCount != ChartEditor.Instance.laneInfo.laneCount;

                
            m_arrOptionDropdown.value = (int)arr;
            m_laneCountDropdown.value = intLastKnownLaneCount;

            if (forceReload)
            {
                int desiredLaneCount;
                if (r_laneOptionToLaneCount.TryGetValue(option, out desiredLaneCount))
                {
                    ChartEditor.Instance.uiServices.menuBar.SetLaneCount(desiredLaneCount);
                }
            }
        }
    }

    public void OnLaneCountDropdownValueChanged(int value)
    {
        LaneCountOptions option = (LaneCountOptions)value;
        ChartEditor editor = ChartEditor.Instance;
        Song.Instrument chartType = editor.currentInstrument;

        int desiredLaneCount;
        if (r_laneOptionToLaneCount.TryGetValue(option, out desiredLaneCount))
        {
            Debug.Log("Desired Lane Count:" + desiredLaneCount);
            switch (chartType)
            {
                
                case Song.Instrument.RealGuitar:
                Globals.gameSettings.realGLaneCount = desiredLaneCount;
                    
                    break;
                case Song.Instrument.RealBass:
                    Globals.gameSettings.realBLaneCount = desiredLaneCount;
                    break;
                case Song.Instrument.RealGuitar22:
                    Globals.gameSettings.realG22LaneCount = desiredLaneCount;
                    break;
                case Song.Instrument.RealBass22:
                    Globals.gameSettings.realB22LaneCount = desiredLaneCount;
                    break;
                case Song.Instrument.BonusRealGuitar:
                    Globals.gameSettings.bonusRealLaneCount = desiredLaneCount;
                    break;
                default:
                    Debug.LogAssertion("How did you get here? There's no possible way to have the RS UI enabled without having selected one of the RS instruments.");
                    break;
            }
            
            editor.uiServices.menuBar.SetLaneCount(desiredLaneCount);
            editor.uiServices.menuBar.LoadCurrentInstumentAndDifficulty();       
        }

    }

    public void OnModeOptionDropdownValueChanged(int value)
    {
        Song.Instrument chartType = ChartEditor.Instance.currentInstrument;
        switch (chartType)
        {
            case Song.Instrument.RealGuitar:
                Globals.gameSettings.realGArr = (GameSettings.ArrangementTypeOptions)value;
                break;
            case Song.Instrument.RealBass:
                Globals.gameSettings.realBArr = (GameSettings.ArrangementTypeOptions)value;
                break;
            case Song.Instrument.RealGuitar22:
                Globals.gameSettings.realG22Arr = (GameSettings.ArrangementTypeOptions)value;
                break;
            case Song.Instrument.RealBass22:
                Globals.gameSettings.realB22Arr = (GameSettings.ArrangementTypeOptions)value;
                break;
            case Song.Instrument.BonusRealGuitar:
                Globals.gameSettings.realBonusArr = (GameSettings.ArrangementTypeOptions)value;
                break;
            default:
                Debug.LogAssertion("How did you get here? There's no possible way to have the RS UI enabled without having selected one of the RS instruments.");
                break;
        }

        ChartEditor.Instance.events.chartReloadedEvent.Fire();
    }
}
