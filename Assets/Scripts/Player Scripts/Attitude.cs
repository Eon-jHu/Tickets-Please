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

    public int AttitudeValue = 2; // Default to neutral attitude

    public void AddAttitudeValue(int _val)
    {
        AttitudeValue += _val;

        // Clamp values
        if (AttitudeValue < 0)
        {
            AttitudeValue = 0;
        }
        else if (AttitudeValue >= AttitudePrompts.Length)
        {
            AttitudeValue = AttitudePrompts.Length - 1;
        }
    }
}
