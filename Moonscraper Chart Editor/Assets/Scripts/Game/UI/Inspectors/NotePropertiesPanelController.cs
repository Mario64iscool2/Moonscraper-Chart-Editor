// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using UnityEngine;
using UnityEngine.UI;
using MoonscraperChartEditor.Song;
using System.Text;

public class NotePropertiesPanelController : PropertiesPanelController {
    public Note currentNote { get { return (Note)currentSongObject; } set { currentSongObject = value; } }

    public Text sustainText;
    public Text fretText;
    public Text stringText;
    public Toggle tapToggle;
    public Toggle forcedToggle;
    public Toggle cymbalToggle;
    public Toggle doubleKickToggle;


    public GameObject scrollView;
    public Toggle palmMuteToggle;
    public Toggle fretHandMuteToggle;
    public Toggle hammerOnToggle;
    public Toggle pullOffToggle;
    public Toggle slideToggle;
    public Toggle unpitchedSlideToggle;
    public Toggle bendToggle;
    public Toggle harmonicToggle;
    public Toggle pinchHarmToggle;
    public Toggle tremoloToggle;
    public Toggle slapToggle;
    public Toggle popToggle;
    public Toggle ignoreToggle;
    public Toggle accentToggle;
    public Toggle rsTapToggle;
    public Toggle linkNextToggle;

    public GameObject noteToolObject;
    PlaceNoteController noteToolController;

    Note prevNote = null;
    Note prevClonedNote = new Note(0, 0);

    bool toggleBlockingActive = false;
    bool initialised = false;

    private void Start()
    {
        if (!initialised)
        {
            noteToolController = noteToolObject.GetComponent<PlaceNoteController>();
            editor.events.toolChangedEvent.Register(OnToolChanged);
            initialised = true;
        }

        ChartEditor.Instance.events.drumsModeOptionChangedEvent.Register(UpdateTogglesInteractable);
    }

    void OnToolChanged()
    {
    }

    void OnEnable()
    {
        if (!initialised)
        {
            Start();
        }

        Update();
    }

    protected override void Update()
    {
        UpdateTogglesInteractable();
        UpdateTogglesDisplay();

        UpdateNoteStringsInfo();
        Controls();

        prevNote = currentNote;
    }

    uint lastKnownKeysModePos = uint.MaxValue;
    void UpdateNoteStringsInfo()
    {
        bool hasCurrentNote = currentNote != null;
        bool hasPreviousNote = prevClonedNote != null;
        bool valuesAreTheSame = hasCurrentNote && hasPreviousNote && prevClonedNote.AllValuesCompare(currentNote);

        if (IsInNoteTool() && Globals.gameSettings.keysModeEnabled)
        {
            // Don't update the string unless the position has actually changed. Results in per-frame garbage otherwise
            if (lastKnownKeysModePos != editor.currentTickPos)
            {
                positionText.text = "Position: " + editor.currentTickPos;
                lastKnownKeysModePos = editor.currentTickPos;
            }

            fretText.text = "Fret: N/A";
            sustainText.text = "Length: N/A";
        }
        else if (currentNote != null && (prevClonedNote != currentNote || !valuesAreTheSame))
        {
            string fretString = string.Empty;
            string noteTypeString = string.Empty;
            if (Globals.drumMode)
            {
                noteTypeString = currentNote.GetDrumString(editor.laneInfo);
            }
            else if (Globals.ghLiveMode)
                noteTypeString = currentNote.ghliveGuitarFret.ToString();
            else if (Globals.RSMode)
            {
                noteTypeString = currentNote.realGuitarFret.ToString();
                fretString = currentNote.GetRSFret().ToString();
            }
            else
                noteTypeString = currentNote.guitarFret.ToString();

            if (!Globals.RSMode)
            {
                stringText.gameObject.SetActive(false);
                fretText.text = "Fret: " + noteTypeString;
                positionText.text = "Position: " + currentNote.tick.ToString();
                sustainText.text = "Length: " + currentNote.length.ToString();
                scrollView.SetActive(false);
            }
            else
            {
                stringText.gameObject.SetActive(true);
                stringText.text = "String: " + noteTypeString;
                fretText.text = "Fret: " + fretString;
                positionText.text = "Position: " + currentNote.tick.ToString();
                sustainText.text = "Length: " + currentNote.length.ToString();
                scrollView.SetActive(true);
            }
            

            prevClonedNote.CopyFrom(currentNote);
            lastKnownKeysModePos = uint.MaxValue;
        }
    }

    bool IsInNoteTool()
    {
        return editor.toolManager.currentToolId == EditorObjectToolManager.ToolID.Note;
    }

    public Note.Flags GetDisplayFlags()
    {
        Note.Flags flags = Note.Flags.None;
        bool inNoteTool = IsInNoteTool();

        if (inNoteTool)
        {
            flags = noteToolController.GetDisplayFlags();
        }
        else if (currentNote != null)
        {
            flags = currentNote.flags;
        }

        return flags;
    }

    void UpdateTogglesDisplay()
    {
        toggleBlockingActive = true;

        Note.Flags flags = GetDisplayFlags();
        bool inNoteTool = IsInNoteTool();

        if (!inNoteTool && currentNote == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("No note loaded into note inspector");
        }

        forcedToggle.isOn = (flags & Note.Flags.Forced) != 0;
        tapToggle.isOn = (flags & Note.Flags.Tap) != 0;
        cymbalToggle.isOn = (flags & Note.Flags.ProDrums_Cymbal) != 0;
        doubleKickToggle.isOn = (flags & Note.Flags.DoubleKick) != 0;
        palmMuteToggle.isOn =     (flags & Note.Flags.RS_PalmMute) != 0;
        fretHandMuteToggle.isOn = (flags & Note.Flags.RS_FretMute) != 0;
        hammerOnToggle.isOn = (flags & Note.Flags.RS_HammerOn) != 0;
        pullOffToggle.isOn = (flags & Note.Flags.RS_PullOff) != 0;
        slideToggle.isOn = (flags & Note.Flags.RS_Slide) != 0;
        bendToggle.isOn = (flags & Note.Flags.RS_Bend) != 0;
        harmonicToggle.isOn = (flags & Note.Flags.RS_Harmonic) != 0;
        pinchHarmToggle.isOn = (flags & Note.Flags.RS_PinchHarmonic) != 0;
        tremoloToggle.isOn = (flags & Note.Flags.RS_Tremolo) != 0;
        ignoreToggle.isOn = (flags & Note.Flags.RS_Ignore) != 0;
        accentToggle.isOn = (flags & Note.Flags.RS_Accent) != 0;
        slapToggle.isOn = (flags & Note.Flags.RS_Slap) != 0;
        popToggle.isOn = (flags & Note.Flags.RS_Pop) != 0;
        rsTapToggle.isOn = (flags & Note.Flags.RS_Tap) != 0;
        linkNextToggle.isOn = (flags & Note.Flags.RS_LinkNext) != 0;


        toggleBlockingActive = false;
    }

    void UpdateTogglesInteractable()
    {
        // Prevent users from forcing notes when they shouldn't be forcable but retain the previous user-set forced property when using the note tool
        bool drumsMode = Globals.drumMode;
        bool proDrumsMode = drumsMode && Globals.gameSettings.drumsModeOptions == GameSettings.DrumModeOptions.ProDrums;
        bool proStringsMode = Globals.RSMode;

        forcedToggle.gameObject.SetActive(!(proStringsMode | drumsMode));
        tapToggle.gameObject.SetActive(!(drumsMode | proStringsMode));
        cymbalToggle.gameObject.SetActive(proDrumsMode);
        doubleKickToggle.gameObject.SetActive(proDrumsMode);

        palmMuteToggle.gameObject.SetActive(proStringsMode);
        fretHandMuteToggle.gameObject.SetActive(proStringsMode);
        hammerOnToggle.gameObject.SetActive(proStringsMode);
        pullOffToggle.gameObject.SetActive(proStringsMode);
        slideToggle.gameObject.SetActive(proStringsMode);
        bendToggle.gameObject.SetActive(proStringsMode);
        harmonicToggle.gameObject.SetActive(proStringsMode);
        pinchHarmToggle.gameObject.SetActive(proStringsMode);
        tremoloToggle.gameObject.SetActive(proStringsMode);
        ignoreToggle.gameObject.SetActive(proStringsMode);
        accentToggle.gameObject.SetActive(proStringsMode);
        slapToggle.gameObject.SetActive(proStringsMode);
        popToggle.gameObject.SetActive(proStringsMode);
        rsTapToggle.gameObject.SetActive(proStringsMode);
        linkNextToggle.gameObject.SetActive(proStringsMode);


        if (!drumsMode)
        {
            if (!proStringsMode)
            {
                if (IsInNoteTool() && (noteToolObject.activeSelf || Globals.gameSettings.keysModeEnabled))
                {
                    forcedToggle.interactable = noteToolController.forcedInteractable;
                    tapToggle.interactable = noteToolController.tapInteractable;
                }
                else if (!IsInNoteTool())
                {
                    forcedToggle.interactable = !(currentNote.cannotBeForced && !Globals.gameSettings.keysModeEnabled);
                    tapToggle.interactable = !currentNote.IsOpenNote();
                }
                else
                {
                    forcedToggle.interactable = true;
                    tapToggle.interactable = true;
                }
            }
            else
            {
                if (IsInNoteTool() && (noteToolObject.activeSelf || Globals.gameSettings.keysModeEnabled))
                {
                    forcedToggle.interactable = noteToolController.forcedInteractable;
                    tapToggle.interactable = noteToolController.tapInteractable;
                }
                else if (!IsInNoteTool())
                {
                    forcedToggle.interactable = !(currentNote.cannotBeForced && !Globals.gameSettings.keysModeEnabled);
                    tapToggle.interactable = !currentNote.IsOpenNote();
                }
                else
                {
                    forcedToggle.interactable = true;
                    tapToggle.interactable = true;
                }
            }
        }
        else
        {
            if (IsInNoteTool() && noteToolObject.activeSelf)
            {
                cymbalToggle.interactable = noteToolController.cymbalInteractable;
                doubleKickToggle.interactable = noteToolController.doubleKickInteractable;
            }
            else if (!IsInNoteTool())
            {
                cymbalToggle.interactable = NoteFunctions.AllowedToBeCymbal(currentNote);
                doubleKickToggle.interactable = NoteFunctions.AllowedToBeDoubleKick(currentNote, editor.currentDifficulty);
            }
            else
            {
                cymbalToggle.interactable = true;
                doubleKickToggle.interactable = true;
            }
        }
    }

    void Controls()
    {
        if (MSChartEditorInput.GetInputDown(MSChartEditorInputActions.ToggleNoteTap) && tapToggle.interactable)
        {
            tapToggle.isOn = !tapToggle.isOn;
        }

        if (MSChartEditorInput.GetInputDown(MSChartEditorInputActions.ToggleNoteForced) && forcedToggle.interactable)
        {
            forcedToggle.isOn = !forcedToggle.isOn;
        }

        if (MSChartEditorInput.GetInputDown(MSChartEditorInputActions.ToggleNoteCymbal) && cymbalToggle.interactable)
        {
            cymbalToggle.isOn = !cymbalToggle.isOn;
        }

        if (MSChartEditorInput.GetInputDown(MSChartEditorInputActions.ToggleNoteDoubleKick) && doubleKickToggle.interactable)
        {
            doubleKickToggle.isOn = !doubleKickToggle.isOn;
        }
    }

    new void OnDisable()
    {
        currentNote = null;
    }
	
    public void setTap()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetTapNoteTool();
        }
        else
        {
            SetTapNote();
        }
    }

    void SetTapNoteTool()
    {
        if (tapToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, tapToggle, Note.Flags.Tap);
    }

    void SetTapNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (tapToggle.isOn)
                    newFlags |= Note.Flags.Tap;
                else
                    newFlags &= ~Note.Flags.Tap;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }


    #region ROCKSMITH_FLAGS

    public void setPalmMute()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetPalmMuteNoteTool();
        }
        else
        {
            SetPalmMuteNote();
        }
    }
    void SetPalmMuteNoteTool()
    {
        if (palmMuteToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, palmMuteToggle, Note.Flags.RS_PalmMute);
    }

    void SetPalmMuteNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (palmMuteToggle.isOn)
                    newFlags |= Note.Flags.RS_PalmMute;
                else
                    newFlags &= ~Note.Flags.RS_PalmMute;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setFretMute()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetFretMuteNoteTool();
        }
        else
        {
            SetFretMuteNote();
        }
    }
    void SetFretMuteNoteTool()
    {
        if (palmMuteToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, fretHandMuteToggle, Note.Flags.RS_FretMute);
    }

    void SetFretMuteNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (fretHandMuteToggle.isOn)
                    newFlags |= Note.Flags.RS_FretMute;
                else
                    newFlags &= ~Note.Flags.RS_FretMute;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setRSTap()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetRSTapNoteTool();
        }
        else
        {
            SetRSTapNote();
        }
    }
    void SetRSTapNoteTool()
    {
        if (rsTapToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, rsTapToggle, Note.Flags.RS_Tap);
    }

    void SetRSTapNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (rsTapToggle.isOn)
                    newFlags |= Note.Flags.RS_Tap;
                else
                    newFlags &= ~Note.Flags.RS_Tap;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setHammerOn()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetHammerOnNoteTool();
        }
        else
        {
            SetHammerOnNote();
        }
    }
    void SetHammerOnNoteTool()
    {
        if (hammerOnToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, hammerOnToggle, Note.Flags.RS_HammerOn);
    }

    void SetHammerOnNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (hammerOnToggle.isOn)
                    newFlags |= Note.Flags.RS_HammerOn;
                else
                    newFlags &= ~Note.Flags.RS_HammerOn;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setPullOff()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetPullOffNoteTool();
        }
        else
        {
            SetPullOffNote();
        }
    }
    void SetPullOffNoteTool()
    {
        if (pullOffToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, pullOffToggle, Note.Flags.RS_PullOff);
    }

    void SetPullOffNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (pullOffToggle.isOn)
                    newFlags |= Note.Flags.RS_PullOff;
                else
                    newFlags &= ~Note.Flags.RS_PullOff;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setHarmonic()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetHarmonicNoteTool();
        }
        else
        {
            SetHarmonicNote();
        }
    }
    void SetHarmonicNoteTool()
    {
        if (harmonicToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, harmonicToggle, Note.Flags.RS_Harmonic);
    }

    void SetHarmonicNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (harmonicToggle.isOn)
                    newFlags |= Note.Flags.RS_Harmonic;
                else
                    newFlags &= ~Note.Flags.RS_Harmonic;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setPinchHarmonic()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetPinchHarmonicNoteTool();
        }
        else
        {
            SetPinchHarmonicNote();
        }
    }
    void SetPinchHarmonicNoteTool()
    {
        if (pinchHarmToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, pinchHarmToggle, Note.Flags.RS_PinchHarmonic);
    }

    void SetPinchHarmonicNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (pinchHarmToggle.isOn)
                    newFlags |= Note.Flags.RS_PinchHarmonic;
                else
                    newFlags &= ~Note.Flags.RS_PinchHarmonic;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setSlide()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetSlideNoteTool();
        }
        else
        {
            SetSlideNote();
        }
    }
    void SetSlideNoteTool()
    {
        if (slideToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, slideToggle, Note.Flags.RS_Slide);
    }

    void SetSlideNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (slideToggle.isOn)
                {
                    newFlags |= Note.Flags.RS_Slide;
                    newFlags &= ~Note.Flags.RS_UnpitchedSlide;
                }
                else
                    newFlags &= ~Note.Flags.RS_Slide;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setUnpitchedSlide()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetUnpitchedSlideNoteTool();
        }
        else
        {
            SetUnpitchedSlideNote();
        }
    }
    void SetUnpitchedSlideNoteTool()
    {
        if (unpitchedSlideToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, unpitchedSlideToggle, Note.Flags.RS_UnpitchedSlide);
    }

    void SetUnpitchedSlideNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (unpitchedSlideToggle.isOn)
                {
                    newFlags |= Note.Flags.RS_UnpitchedSlide;
                    newFlags &= ~Note.Flags.RS_Slide;
                }

                else
                    newFlags &= ~Note.Flags.RS_UnpitchedSlide;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setBend()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetBendNoteTool();
        }
        else
        {
            SetBendNote();
        }
    }
    void SetBendNoteTool()
    {
        if (bendToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, bendToggle, Note.Flags.RS_Bend);
    }

    void SetBendNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (bendToggle.isOn)
                    newFlags |= Note.Flags.RS_Bend;
                else
                    newFlags &= ~Note.Flags.RS_Bend;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setSlap()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetSlapNoteTool();
        }
        else
        {
            SetSlapNote();
        }
    }

    void SetSlapNoteTool()
    {
        if (slapToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, slapToggle, Note.Flags.RS_Slap);
    }

    void SetSlapNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (slapToggle.isOn)
                    newFlags |= Note.Flags.RS_Slap;
                else
                    newFlags &= ~Note.Flags.RS_Slap;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setPop()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetPopNoteTool();
        }
        else
        {
            SetPopNote();
        }
    }

    void SetPopNoteTool()
    {
        if (popToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, popToggle, Note.Flags.RS_Pop);
    }

    void SetPopNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (popToggle.isOn)
                    newFlags |= Note.Flags.RS_Pop;
                else
                    newFlags &= ~Note.Flags.RS_Pop;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setIgnore()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetIgnoreNoteTool();
        }
        else
        {
            SetIgnoreNote();
        }
    }

    void SetIgnoreNoteTool()
    {
        if (ignoreToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, ignoreToggle, Note.Flags.RS_Ignore);
    }

    void SetIgnoreNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (ignoreToggle.isOn)
                    newFlags |= Note.Flags.RS_Ignore;
                else
                    newFlags &= ~Note.Flags.RS_Ignore;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setTremolo()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetTremoloNoteTool();
        }
        else
        {
            SetTremoloNote();
        }
    }

    void SetTremoloNoteTool()
    {
        if (tremoloToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, tremoloToggle, Note.Flags.RS_Tremolo);
    }

    void SetTremoloNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (tremoloToggle.isOn)
                    newFlags |= Note.Flags.RS_Tremolo;
                else
                    newFlags &= ~Note.Flags.RS_Tremolo;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    public void setAccent()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetAccentNoteTool();
        }
        else
        {
            SetAccentNote();
        }
    }

    void SetAccentNoteTool()
    {
        if (accentToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, accentToggle, Note.Flags.RS_Accent);
    }

    void SetAccentNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (accentToggle.isOn)
                    newFlags |= Note.Flags.RS_Accent;
                else
                    newFlags &= ~Note.Flags.RS_Accent;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }


    public void setLinkNext()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetLinkNextNoteTool();
        }
        else
        {
            SetLinkNextNote();
        }
    }

    void SetLinkNextNoteTool()
    {
        if (linkNextToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, linkNextToggle, Note.Flags.RS_LinkNext);
    }

    void SetLinkNextNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (linkNextToggle.isOn)
                    newFlags |= Note.Flags.RS_LinkNext;
                else
                    newFlags &= ~Note.Flags.RS_LinkNext;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    #endregion


    void SetNoteToolFlag(ref Note.Flags flags, Toggle uiToggle, Note.Flags flagsToToggle)
    {
        if ((flags & flagsToToggle) == 0)
            flags |= flagsToToggle;
        else
            flags &= ~flagsToToggle;
    }


    public void setForced()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetForcedNoteTool();
        }
        else
        {
            SetForcedNote();
        }
    }

    public void setCymbal()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetCymbalNoteTool();
        }
        else
        {
            SetCymbalNote();
        }
    }

    public void setDoubleKick()
    {
        if (toggleBlockingActive)
            return;

        if (IsInNoteTool())
        {
            SetDoubleKickNoteTool();
        }
        else
        {
            SetDoubleKickNote();
        }
    }

    void SetForcedNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (forcedToggle.isOn)
                    newFlags |= Note.Flags.Forced;
                else
                    newFlags &= ~Note.Flags.Forced;
            }

            SetNewFlags(currentNote, newFlags);
        }
    }

    void SetForcedNoteTool()
    {
        if (forcedToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, forcedToggle, Note.Flags.Forced);
    }

    void SetCymbalNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (cymbalToggle.isOn)
                    newFlags |= Note.Flags.ProDrums_Cymbal;
                else
                    newFlags &= ~Note.Flags.ProDrums_Cymbal;
            }

            SetNewFlags(currentNote, newFlags);
        }

    }

    void SetCymbalNoteTool()
    {
        if (cymbalToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, cymbalToggle, Note.Flags.ProDrums_Cymbal);
    }

    void SetDoubleKickNote()
    {
        if (currentNote == prevNote)
        {
            var newFlags = currentNote.flags;

            if (currentNote != null)
            {
                if (doubleKickToggle.isOn)
                    newFlags |= Note.Flags.DoubleKick;
                else
                    newFlags &= ~Note.Flags.DoubleKick;
            }

            SetNewFlags(currentNote, newFlags);
        }

    }

    void SetDoubleKickNoteTool()
    {
        if (doubleKickToggle.interactable)
            SetNoteToolFlag(ref noteToolController.desiredFlags, doubleKickToggle, Note.Flags.DoubleKick);
    }

    void SetNewFlags(Note note, Note.Flags newFlags)
    {
        if (note.flags == newFlags)
            return;

        if (editor.toolManager.currentToolId == EditorObjectToolManager.ToolID.Cursor)
        {
            Note newNote = new Note(note.tick, note.rawNote, note.length, newFlags);
            SongEditModifyValidated command = new SongEditModifyValidated(note, newNote);
            editor.commandStack.Push(command);
        }
        else
        {
            // Updating note tool parameters and visuals
            noteToolController.desiredFlags = newFlags;
        }
    }

}
