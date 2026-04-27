using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class sGigManager : MonoBehaviour
{
    eGigType currentGig = eGigType.none;

    public static sGigManager gigManagerGlobal;

    // list of gig types that have been unlocked/made available
    List<eGigType> gigTypesUnlocked;

    private void Awake()
    {
        gigManagerGlobal = this;

        gigTypesUnlocked = new List<eGigType>();
    }

    public void GetGig(eGigType _gigType)
    {
        // when you get a gig it adds it too the unlocked
        gigTypesUnlocked.Add(_gigType);
    }

    public void StartGig(eGigType _type)
    {
        // sets the current gig type
        currentGig = _type;
    }

    public void FinishGig()
    {
        // sets current get type to none
        currentGig = eGigType.none;
    }

    // this returns the current gig type
    public eGigType ReturnCurrentGigType()
    {
        return currentGig;
    }
}
