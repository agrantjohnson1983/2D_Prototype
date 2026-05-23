using UnityEngine;

public class sCharacterSignSpinGig : sCharacterGigMaster
{
    private void OnEnable()
    {
        gigData.onGigComplete.AddListener(SetComplete);
    }

    private void OnDisable()
    {
        gigData.onGigComplete.RemoveListener(SetComplete);
    }

    void SetComplete()
    {
        ResetCanvas();
        characterDialogueTextMesh.text = gigData.gigCompleteText;
    }
}
