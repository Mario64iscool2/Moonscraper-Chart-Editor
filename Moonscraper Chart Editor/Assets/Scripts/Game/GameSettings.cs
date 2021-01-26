// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using MoonscraperChartEditor.Song;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettings
{
    const string SECTION_NAME_SETTINGS = "Settings";
    const string SECTION_NAME_AUDIO = "Audio Volume";
    const string SECTION_NAME_GRAPHICS = "Graphics";
    const string SECTION_NAME_TUNINGS = "Tunings";

    [System.Flags]
    public enum ClapToggle
    {
        NONE = 0,
        
        STRUM           = 1 << 0,
        HOPO            = 1 << 1,
        TAP             = 1 << 2,

        STARPOWER       = 1 << 3,
        CHARTEVENT      = 1 << 4,

        BPM             = 1 << 5,
        TS              = 1 << 6,
        EVENT           = 1 << 7,
        SECTION         = 1 << 8,
    }

    public enum NotePlacementMode
    {
        Default, LeftyFlip
    }

    public enum DrumModeOptions
    {
        Standard,
        ProDrums,
    }

    public bool keysModeEnabled = false;
    public bool metronomeActive = false;
    public bool extendedSustainsEnabled = false;
    public bool sustainGapEnabled = false;
    public bool sustainGapIsTimeBased = false;
    public bool resetAfterPlay = false;
    public bool resetAfterGameplay = false;
    public bool slowdownPitchCorrectionEnabled = false;

    public int audioCalibrationMS = 0;                     // Increase to start the audio sooner
    public int clapCalibrationMS = 0;
    public int customBgSwapTime;
    public int targetFramerate = -1;
    public int drumsLaneCount = 5;
    public int realGLaneCount = 6;
    public int realBLaneCount = 4;
    public int realG22LaneCount = 6;
    public int realB22LaneCount = 4;
    public int bonusRealLaneCount = 6;
    public int realGTuning1 = 0;
    public int realGTuning2 = 0;
    public int realGTuning3 = 0;
    public int realGTuning4 = 0;
    public int realGTuning5 = 0;
    public int realGTuning6 = 0;
    public int realBTuning1 = 0;
    public int realBTuning2 = 0;
    public int realBTuning3 = 0;
    public int realBTuning4 = 0;
    public int realBTuning5 = 0;
    public int realBTuning6 = 0;
    public int realG22Tuning1 = 0;
    public int realG22Tuning2 = 0;
    public int realG22Tuning3 = 0;
    public int realG22Tuning4 = 0;
    public int realG22Tuning5 = 0;
    public int realG22Tuning6 = 0;
    public int realB22Tuning1 = 0;
    public int realB22Tuning2 = 0;
    public int realB22Tuning3 = 0;
    public int realB22Tuning4 = 0;
    public int realB22Tuning5 = 0;
    public int realB22Tuning6 = 0;
    public int bonusRealTuning1 = 0;
    public int bonusRealTuning2 = 0;
    public int bonusRealTuning3 = 0;
    public int bonusRealTuning4 = 0;
    public int bonusRealTuning5 = 0;
    public int bonusRealTuning6 = 0;

    public ArrangementTypeOptions realGArr = ArrangementTypeOptions.Lead;
    public ArrangementTypeOptions realBArr = ArrangementTypeOptions.Bass;
    public ArrangementTypeOptions realG22Arr = ArrangementTypeOptions.Rhythm;
    public ArrangementTypeOptions realB22Arr = ArrangementTypeOptions.Undefined;
    public ArrangementTypeOptions realBonusArr = ArrangementTypeOptions.Undefined;

    public DrumModeOptions drumsModeOptions = DrumModeOptions.Standard;
    public enum ArrangementTypeOptions
    {
        // Keep this in the same order as the UI
        Lead = 0,
        Bass = 1,
        Rhythm = 2,
        Combo = 3,
        Undefined = 4
    }

    public float hyperspeed = 5.0f;
    public float highwayLength = 0;
    float _sfxVolume = 1;
    public float gameSpeed = 1;
    public float gameplayStartDelayTime = 3.0f;
    public float sfxVolume
    {
        get { return _sfxVolume; }
        set
        {
            if (value < 0)
                _sfxVolume = 0;
            else if (value > 1)
                _sfxVolume = 1;
            else
                _sfxVolume = value;
        }
    }

    public ArrangementTypeOptions GetArrangementType(Song.Instrument inst)
    {
        switch(inst)
        {
            case Song.Instrument.RealGuitar:
                return realGArr;
            case Song.Instrument.RealBass:
                return realBArr;
            case Song.Instrument.RealGuitar22:
                return realG22Arr;
            case Song.Instrument.RealBass22:
                return realB22Arr;
            case Song.Instrument.BonusRealGuitar:
                return realBonusArr;
            default:
                return ArrangementTypeOptions.Undefined;
        }
        
    }

    public float vol_master, vol_song, vol_guitar, vol_bass, vol_rhythm, vol_keys, vol_drums, vol_drums2, vol_drums3, vol_drums4, vol_vocals, audio_pan, vol_crowd;

    public Step snappingStep = new Step(16);
    public int step { get { return snappingStep.value; } set { snappingStep.value = value; } }
    public Step sustainGapStep;
    public int sustainGap { get { return sustainGapStep.value; } set { sustainGapStep.value = value; } }
    public int sustainGapTimeMs = 0;

    public bool clapEnabled = false;

    const int c_defaultClapVal = (int)(ClapToggle.STRUM | ClapToggle.HOPO | ClapToggle.TAP);
    public ClapToggle clapProperties = (ClapToggle)c_defaultClapVal;
    public NotePlacementMode notePlacementMode = NotePlacementMode.Default;

    public SongValidate.ValidationOptions songValidatorModes = ~SongValidate.ValidationOptions.None;
    public bool autoValidateSongOnSave = true;
    public bool automaticallyCheckForUpdates = true;

    public MSChartEditorInput.MSChartEditorActionContainer controls = new MSChartEditorInput.MSChartEditorActionContainer();

    public bool GetBoolSetting(string identifier)
    {
        switch (identifier)
        {
            case ("keysModeEnabled"): return keysModeEnabled;
            case ("metronomeActive"): return metronomeActive;
            case ("extendedSustainsEnabled"): return extendedSustainsEnabled;
            case ("sustainGapEnabled"): return sustainGapEnabled;
            case ("resetAfterPlay"): return resetAfterPlay;
            case ("resetAfterGameplay"): return resetAfterGameplay;
            default: return false;
        }
    }

    public GameSettings()
    {
    }

    public void Load(string configFilepath, string controllerBindingsFilepath)
    {
        INIParser iniparse = new INIParser();

        try
        {
            Debug.Log("Loading game settings");
         
            iniparse.Open(configFilepath);

            // Check for valid fps values
            targetFramerate = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Framerate", 120);
            hyperspeed = (float)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Hyperspeed", 5.0f);
            highwayLength = (float)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Highway Length", 0);
            audioCalibrationMS = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Audio calibration", 0);
            clapCalibrationMS = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Clap calibration", 0);
            clapProperties = (ClapToggle)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Clap", c_defaultClapVal);
            extendedSustainsEnabled = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Extended sustains", false);
            clapEnabled = false;
            sustainGapEnabled = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Sustain Gap", false);
            sustainGapIsTimeBased = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Sustain Gap Is Time Based", false);
            sustainGapStep = new Step((int)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Sustain Gap Step", (int)16));
            sustainGapTimeMs = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Sustain Gap Time", (int)0);
            notePlacementMode = (NotePlacementMode)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Note Placement Mode", (int)NotePlacementMode.Default);
            gameplayStartDelayTime = (float)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Gameplay Start Delay", 3.0f);
            resetAfterPlay = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Reset After Play", false);
            resetAfterGameplay = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Reset After Gameplay", false);
            slowdownPitchCorrectionEnabled = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Slowdown Pitch Correction Enabled", false);
            customBgSwapTime = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Custom Background Swap Time", 30);
            drumsLaneCount = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Drums Lane Count", 5);
            realGLaneCount = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Guitar Lane Count", 6);
            realBLaneCount = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Bass Lane Count", 4);
            realG22LaneCount = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Guitar 22 Lane Count", 6);
            realB22LaneCount = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Bass 22 Lane Count", 4);
            bonusRealLaneCount = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Bonus Real Lane Count", 6);
            realGTuning1 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar String 1 Tuning", 0);
            realGTuning2 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar String 2 Tuning", 0);
            realGTuning3 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar String 3 Tuning", 0);
            realGTuning4 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar String 4 Tuning", 0);
            realGTuning5 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar String 5 Tuning", 0);
            realGTuning6 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar String 6 Tuning", 0);
            realBTuning1 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass String 1 Tuning", 0);
            realBTuning2 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass String 2 Tuning", 0);
            realBTuning3 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass String 3 Tuning", 0);
            realBTuning4 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass String 4 Tuning", 0);
            realBTuning5 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass String 5 Tuning", 0);
            realBTuning6 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass String 6 Tuning", 0);
            realG22Tuning1 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 1 Tuning", 0);
            realG22Tuning2 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 2 Tuning", 0);
            realG22Tuning3 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 3 Tuning", 0);
            realG22Tuning4 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 4 Tuning", 0);
            realG22Tuning5 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 5 Tuning", 0);
            realG22Tuning6 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 6 Tuning", 0);
            realB22Tuning1 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 1 Tuning", 0);
            realB22Tuning2 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 2 Tuning", 0);
            realB22Tuning3 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 3 Tuning", 0);
            realB22Tuning4 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 4 Tuning", 0);
            realB22Tuning5 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 5 Tuning", 0);
            realB22Tuning6 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 6 Tuning", 0);
            bonusRealTuning1 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Bonus Real Guitar String 1 Tuning", 0);
            bonusRealTuning2 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Bonus Real Guitar String 2 Tuning", 0);
            bonusRealTuning3 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Bonus Real Guitar String 3 Tuning", 0);
            bonusRealTuning4 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Bonus Real Guitar String 4 Tuning", 0);
            bonusRealTuning5 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Bonus Real Guitar String 5 Tuning", 0);
            bonusRealTuning6 = iniparse.ReadValue(SECTION_NAME_TUNINGS, "Bonus Real Guitar String 6 Tuning", 0);
            realGArr = (ArrangementTypeOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Real Guitar Arrangement", (int)ArrangementTypeOptions.Lead);
            realBArr = (ArrangementTypeOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Real Bass Arrangement", (int)ArrangementTypeOptions.Bass);
            realG22Arr = (ArrangementTypeOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Real Guitar 22 Arrangement", (int)ArrangementTypeOptions.Rhythm);
            realB22Arr = (ArrangementTypeOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Real Bass 22 Arrangement", (int)ArrangementTypeOptions.Bass);
            realBonusArr = (ArrangementTypeOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Real Bonus Arrangement", (int)ArrangementTypeOptions.Undefined);

            drumsModeOptions = (DrumModeOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Drums Mode", (int)DrumModeOptions.Standard);
            if (!System.Enum.IsDefined(typeof(DrumModeOptions), drumsModeOptions))
            {
                drumsModeOptions = DrumModeOptions.Standard;
            }
            songValidatorModes = (SongValidate.ValidationOptions)iniparse.ReadValue(SECTION_NAME_SETTINGS, "Song Validator Modes", (int)(~SongValidate.ValidationOptions.None));
            autoValidateSongOnSave = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Auto Validate Song On Save", true);
            automaticallyCheckForUpdates = iniparse.ReadValue(SECTION_NAME_SETTINGS, "Auto Check For Updates", true);

            gameplayStartDelayTime = Mathf.Clamp(gameplayStartDelayTime, 0, 3.0f);
            gameplayStartDelayTime = (float)(System.Math.Round(gameplayStartDelayTime * 2.0f, System.MidpointRounding.AwayFromZero) / 2.0f); // Check that the gameplay start delay time is a multiple of 0.5 and is

            // Audio levels
            vol_master = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Master", 0.5f);
            vol_song = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Music Stream", 1.0f);
            vol_guitar = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Guitar Stream", 1.0f);
            vol_bass = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Bass Stream", 1.0f);
            vol_rhythm = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Rhythm Stream", 1.0f);
			vol_keys = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Keys Stream", 1.0f);
			vol_vocals = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Vocals Stream", 1.0f);
            vol_drums = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Drum Stream", 1.0f);
            vol_drums2 = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Drum 2 Stream", 1.0f);
            vol_drums3 = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Drum 3 Stream", 1.0f);
            vol_drums4 = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Drum 4 Stream", 1.0f);
            vol_crowd = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Crowd Stream", 1.0f);
            audio_pan = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "Audio Pan", 0.0f);
            sfxVolume = (float)iniparse.ReadValue(SECTION_NAME_AUDIO, "SFX", 1.0f);
			
            // Need to fix old config values
            if ((int)clapProperties > (((int)ClapToggle.SECTION << 1) - 1))
            {
                clapProperties = (ClapToggle)c_defaultClapVal;
            }

            // Graphics Settings
            QualitySettings.antiAliasing = iniparse.ReadValue(SECTION_NAME_GRAPHICS, "AntiAliasingLevel", QualitySettings.antiAliasing);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error encountered when trying to load game settings. " + e.Message);
        }
        finally
        {
            iniparse.Close();
        }

        try
        {
            // Populate with all default controls first
            controls.LoadFromSaveData(InputManager.Instance.defaultControls);

            if (File.Exists(controllerBindingsFilepath))
            {
                // Override with custom controls if they exist.
                Debug.Log("Loading input settings");
                string controlsJson = File.ReadAllText(controllerBindingsFilepath);
                controls.LoadFromSaveData(JsonUtility.FromJson<MSChartEditorInput.MSChartEditorActionContainer>(controlsJson));
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Unable to load saved controls. " + e.Message);
        }
    }

    public void Save(string configFilepath, string controllerBindingsFilepath)
    {
        INIParser iniparse = new INIParser();

        try
        {
            Debug.Log("Saving game settings");

            iniparse.Open(configFilepath);

            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Framerate", targetFramerate);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Hyperspeed", hyperspeed);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Highway Length", highwayLength);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Audio calibration", audioCalibrationMS);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Clap calibration", clapCalibrationMS);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Clap", (int)clapProperties);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Extended sustains", extendedSustainsEnabled);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Sustain Gap", sustainGapEnabled);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Sustain Gap Is Time Based", sustainGapIsTimeBased);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Sustain Gap Step", sustainGap);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Sustain Gap Time", sustainGapTimeMs);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Note Placement Mode", (int)notePlacementMode);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Gameplay Start Delay", gameplayStartDelayTime);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Reset After Play", resetAfterPlay);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Reset After Gameplay", resetAfterGameplay);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Slowdown Pitch Correction Enabled", slowdownPitchCorrectionEnabled);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Custom Background Swap Time", customBgSwapTime);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Drums Lane Count", drumsLaneCount);

            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Guitar Lane Count", realGLaneCount);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Bass Lane Count", realBLaneCount);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Guitar 22 Lane Count", realG22LaneCount);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Bass 22 Lane Count", realB22LaneCount);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Bonus Real Lane Count", bonusRealLaneCount);

            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar String 1 Tuning", realGTuning1);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar String 2 Tuning", realGTuning2);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar String 3 Tuning", realGTuning3);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar String 4 Tuning", realGTuning4);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar String 5 Tuning", realGTuning5);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar String 6 Tuning", realGTuning6);

            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass String 1 Tuning", realBTuning1);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass String 2 Tuning", realBTuning2);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass String 3 Tuning", realBTuning3);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass String 4 Tuning", realBTuning4);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass String 5 Tuning", realBTuning5);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass String 6 Tuning", realBTuning6);

            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 1 Tuning", realG22Tuning1);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 2 Tuning", realG22Tuning2);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 3 Tuning", realG22Tuning3);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 4 Tuning", realG22Tuning4);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 5 Tuning", realG22Tuning5);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Guitar 22 String 6 Tuning", realG22Tuning6);

            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 1 Tuning", realB22Tuning1);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 2 Tuning", realB22Tuning2);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 3 Tuning", realB22Tuning3);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 4 Tuning", realB22Tuning4);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 5 Tuning", realB22Tuning5);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Real Bass 22 String 6 Tuning", realB22Tuning6);

            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Bonus Real String 1 Tuning", bonusRealTuning1);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Bonus Real String 2 Tuning", bonusRealTuning2);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Bonus Real String 3 Tuning", bonusRealTuning3);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Bonus Real String 4 Tuning", bonusRealTuning4);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Bonus Real String 5 Tuning", bonusRealTuning5);
            iniparse.WriteValue(SECTION_NAME_TUNINGS, "Bonus Real String 6 Tuning", bonusRealTuning6);

            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Real Guitar Arrangement", (int)realGArr);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Real Bass Arrangement", (int)realBArr);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Real Guitar 22 Arrangement", (int)realG22Arr);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Real Bass 22 Arrangement", (int)realB22Arr);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Real Bonus Arrangement", (int)realBonusArr);

            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Drums Mode", (int)drumsModeOptions);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Song Validator Modes", (int)songValidatorModes);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Auto Validate Song On Save", autoValidateSongOnSave);
            iniparse.WriteValue(SECTION_NAME_SETTINGS, "Auto Check For Updates", automaticallyCheckForUpdates);    

            // Audio levels
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Master", vol_master);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Music Stream", vol_song);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Guitar Stream", vol_guitar);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Bass Stream", vol_bass);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Rhythm Stream", vol_rhythm);
			iniparse.WriteValue(SECTION_NAME_AUDIO, "Keys Stream", vol_keys);
			iniparse.WriteValue(SECTION_NAME_AUDIO, "Vocals Stream", vol_vocals);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Drum Stream", vol_drums);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Drum 2 Stream", vol_drums2);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Drum 3 Stream", vol_drums3);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Drum 4 Stream", vol_drums4);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Crowd Stream", vol_crowd);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "Audio Pan", audio_pan);
            iniparse.WriteValue(SECTION_NAME_AUDIO, "SFX", sfxVolume);

            // Graphics Settings
            iniparse.WriteValue(SECTION_NAME_GRAPHICS, "AntiAliasingLevel", QualitySettings.antiAliasing);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error encountered when trying to save game settings. " + e.Message);
        }
        finally
        {
            iniparse.Close();
        }

        controls.UpdateSaveData();
        var controlsJson = JsonUtility.ToJson(controls, true);
        try
        {
            Debug.Log("Saving input settings");

            // Save to file
            File.WriteAllText(controllerBindingsFilepath, controlsJson, System.Text.Encoding.UTF8);
        }
        catch (System.Exception e)
        {
            Logger.LogException(e, "Error encountered while saving control bindings. " + e.Message);
        }
    }
}

