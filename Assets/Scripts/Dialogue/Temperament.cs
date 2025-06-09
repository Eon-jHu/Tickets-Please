using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperament : MonoBehaviour
{
    public enum TemperamentType
    {
        Calm,
        Agitated,
        Outgoing,
        Shy,
        Anxious,
        Aggressive
    }

    public Dictionary<TemperamentType, string> TemperamentPrompt = new Dictionary<TemperamentType, string>
    {
        { TemperamentType.Calm, " rather calm." },
        { TemperamentType.Agitated, " getting agitated." },
        { TemperamentType.Outgoing, " bold and outgoing." },
        { TemperamentType.Shy, " shy and quiet." },
        { TemperamentType.Anxious, " aquite anxious and worried." },
        { TemperamentType.Aggressive, " frustrated and aggressive." }
    };

    public TemperamentType CurrentTemperament { get; private set; }

    public void SetRandomTemperament()
    {
        CurrentTemperament = (TemperamentType)Random.Range(0, System.Enum.GetNames(typeof(TemperamentType)).Length);
    }
}
