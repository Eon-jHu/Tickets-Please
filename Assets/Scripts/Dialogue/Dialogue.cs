using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string m_Name;

    [TextArea(1, 10)]
    public string[] m_Sentences;

    [TextArea(0, 9)]
    public string[] m_Responses;

    // Write your single-sentence response in the corresponding element
    [TextArea(0, 9)]
    public string[] m_ReactionToResponse;

    public float m_TextSpeed = 0.05f;
}
