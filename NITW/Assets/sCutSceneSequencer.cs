using System.Collections;
using UnityEngine;

public class sCutSceneSequencer : MonoBehaviour
{   
    [System.Serializable]
    public class Cutscene
    {
        public GameObject[] objectsToTurnOn;
        public GameObject[] objectsToTurnOff;

        public CharacterMovement[] characterMovements;

        [HideInInspector] public int characterMovementIndex = 0;

        public SO_Dialogue startDialogue;
    }
    
    [System.Serializable]
    public class CharacterMovement
    {
        [System.Serializable]
        public class Movements
        {
            // length of time for movement
            public float duration;

            // this gets added to a characters position to set the new position
            public Vector2 movementOffset;
        }

        public GameObject objectToMove;

        public Movements[] movements;

        [HideInInspector] public int movementsIndex;

        // set this to true if the animation should play out before triggering the next one
        public bool isBlocking = false;

        public string dialoguePopupText;
    }

    public Cutscene[] cutScenes;

    int sceneIndex = 0;

    private void Start()
    {
        StartSequence();
    }

    void StartSequence()
    {
        // turns off stuff
        for (int i = 0; i < cutScenes[sceneIndex].objectsToTurnOff.Length; i++)
        {
            cutScenes[sceneIndex].objectsToTurnOff[i].SetActive(false);
        }

        // turns on stuff
        for (int i = 0; i < cutScenes[sceneIndex].objectsToTurnOn.Length; i++)
        {
            cutScenes[sceneIndex].objectsToTurnOff[i].SetActive(true);
        }

        // checks if there is a dialogue SO
        if(cutScenes[sceneIndex].startDialogue != null)
        {
            // toggles dialogue in GM
            sGameManager.gm.ToggleDialogue(true);

            // starts dialogue
            //sDialogueManager.dialogueManagerGlobal.StartDialogue(cutScenes[sceneIndex].startDialogue, eDialogueBoxLocation.center);

            // checks character movement array
            for (int i = 0; i < cutScenes[sceneIndex].characterMovements.Length; i++)
            {
                
            }
        }
    }

    void NextScene()
    {
        sceneIndex++;

        StartSequence();
    }

    void NextCharacterMove()
    {
        cutScenes[sceneIndex].characterMovementIndex++;
    }

    void NextMovement(CharacterMovement _characterMovement)
    {
        // increments index
        _characterMovement.movementsIndex++;

        // checks to see if index is greater than array
        if(_characterMovement.movementsIndex < _characterMovement.movements.Length)
        {
            // starts next movement sequence
            StartCoroutine(MoveCharacter(cutScenes[sceneIndex].characterMovements[cutScenes[sceneIndex].characterMovementIndex]));
        }

        else
        {
            NextCharacterMove();
        }
    }

    IEnumerator MoveCharacter(CharacterMovement _characterMovement)
    {
        float counter = 0f;

        Vector3 movementOffset = new Vector3(_characterMovement.movements[_characterMovement.movementsIndex].movementOffset.x, _characterMovement.movements[_characterMovement.movementsIndex].movementOffset.y, 0f);

        while(counter < _characterMovement.movements[_characterMovement.movementsIndex].duration)
        {
            _characterMovement.objectToMove.transform.position = 
                Vector3.Lerp(_characterMovement.objectToMove.transform.position, _characterMovement.objectToMove.transform.position + movementOffset, (counter/_characterMovement.movements[_characterMovement.movementsIndex].duration));

            counter += Time.deltaTime;

            yield return null;
        }

        NextMovement(_characterMovement);
    }
}
