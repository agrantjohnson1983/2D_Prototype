using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AVSim.TextFX;

namespace AVSim.Dialogue
{
    public class sDialogueUIController : MonoBehaviour
    {
        [Header("Refs")]
        public GameObject RootPanel;
        public TMP_Text SpeakerLabel;
        public TMP_Text BodyText;
        public Button ContinueButton;
        public List<Button> ChoiceButtons; // size 3 to match Choice1-3 columns
        public List<TMP_Text> ChoiceLabels;

        [Header("Portrait")]
        public Image PortraitImage;
        public SO_CharacterPortraitLibrary PortraitLibrary;

        public sTextEffectsAnimator TextAnimator;

        public sTypewriter typewriter;

        private void OnEnable()
        {
            bool hasLineAlready = false;

            if (sDialogueSheetManager.Instance != null)
            {
                sDialogueSheetManager.Instance.OnLineShown += HandleLineShown;
                sDialogueSheetManager.Instance.OnConversationEnded += HandleConversationEnded;

                // If StartConversation already ran before this OnEnable (e.g. this
                // UI object was activated after the manager fired its first line),
                // catch up to the current line instead of missing it.
                hasLineAlready = sDialogueSheetManager.Instance.CurrentLine != null;
            }

            if (ContinueButton != null)
            {
                ContinueButton.onClick.AddListener(HandleContinueClicked);
            }

            for (int i = 0; i < ChoiceButtons.Count; i++)
            {
                int captured = i; // avoid closure-over-loop-variable bug
                ChoiceButtons[i].onClick.AddListener(() => HandleChoiceClicked(captured));
            }

            if (hasLineAlready)
            {
                HandleLineShown(sDialogueSheetManager.Instance.CurrentLine);
            }
            else if (RootPanel != null)
            {
                RootPanel.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (sDialogueSheetManager.Instance != null)
            {
                sDialogueSheetManager.Instance.OnLineShown -= HandleLineShown;
                sDialogueSheetManager.Instance.OnConversationEnded -= HandleConversationEnded;
            }
        }

        private void HandleLineShown(DialogueLine line)
        {
            if (RootPanel != null)
            {
                RootPanel.SetActive(true);
            }

            SpeakerLabel.text = line.Speaker;
            //BodyText.text = line.Text;
            //TextAnimator.SetText(line.Text);

            AVSim.TextFX.ParsedText parsed =
            sTextTagParser.Parse(line.Text);

            TextAnimator.SetParsedText(parsed);

            typewriter.BeginTyping();

            if (PortraitImage != null && PortraitLibrary != null)
            {
                Sprite portrait = PortraitLibrary.GetPortrait(line.Speaker);
                PortraitImage.sprite = portrait;
                PortraitImage.gameObject.SetActive(portrait != null);
            }

            // Panels that were just activated this frame don't always get their
            // TMP mesh / layout rebuilt in time to show text on the same frame.
            // Force it so the very first line renders immediately.
            Canvas.ForceUpdateCanvases();
            SpeakerLabel.ForceMeshUpdate();
            BodyText.ForceMeshUpdate();

            bool hasChoices = line.Choices.Count > 0;
            ContinueButton.gameObject.SetActive(!hasChoices);

            for (int i = 0; i < ChoiceButtons.Count; i++)
            {
                bool active = i < line.Choices.Count;
                ChoiceButtons[i].gameObject.SetActive(active);

                if (active)
                {
                    ChoiceLabels[i].text = line.Choices[i].Text;
                }
            }
        }

        private void HandleConversationEnded()
        {
            if (RootPanel != null)
            {
                RootPanel.SetActive(false);
            }
        }

        private void HandleContinueClicked()
        {
            sDialogueSheetManager.Instance.Advance();
        }

        private void HandleChoiceClicked(int index)
        {
            sDialogueSheetManager.Instance.SelectChoice(index);
        }
    }
}
