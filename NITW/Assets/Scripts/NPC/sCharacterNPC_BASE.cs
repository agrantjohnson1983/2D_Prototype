using TMPro;
using UnityEngine;

public class sCharacterNPC_BASE : sInteractable
{
    [Space]
    [Header("Dialogue")]
    public GameObject canvasDialogue;

    public TextMeshProUGUI characterDialogueTextMesh;

    public TextMeshProUGUI textButtonYes, textButtonNo;
    [Space]
    public GameObject buttonYes, buttonNo;

    //public TextMeshProUGUI characterTextMeshGigOffer;
    public uTypewriter typewriter;
}
