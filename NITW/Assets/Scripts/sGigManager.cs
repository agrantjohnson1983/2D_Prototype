using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class sGigManager : MonoBehaviour
{
    SO_Gig currentGig = null;

    public static sGigManager gigManagerGlobal;

    // list of gig types that have been unlocked/made available
    List<SO_Gig> gigTypesUnlocked;

    private void Awake()
    {
        gigManagerGlobal = this;

        gigTypesUnlocked = new List<SO_Gig>();
    }

    public void GetGig(SO_Gig _gig)
    {
        // when you get a gig it adds it too the unlocked
        gigTypesUnlocked.Add(_gig);
    }

    public void StartGig(SO_Gig _gig)
    {
        // sets the current gig type
        currentGig = _gig;

        // Turns on main canvas
        sGameManager.gm.ToggleCanvasMain(false);
    }

    public void FinishGig()
    {
        // triggers event
        //currentGig.TriggerOnGigComplete();

        // Turns on main canvas
        sGameManager.gm.ToggleCanvasMain(true);

        // sets current get type to none
        currentGig = null;
            
    }

    // this returns the current gig type
    public SO_Gig ReturnCurrentGigType()
    {
        return currentGig;
    }
}
