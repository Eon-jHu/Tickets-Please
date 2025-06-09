using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attitude : MonoBehaviour
{
    public string[] AttitudePrompts =
    {
        "very rude",
        "quite rude",
        "neutrally",
        "quite kindly",
        "very kindly"
    };

    public int AttitudeValue = 3; // Default to neutral attitude
}
