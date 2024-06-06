using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponseButton : MonoBehaviour
{
    public string m_Response;

    public TextMeshProUGUI m_ButtonText;

    // Start is called before the first frame update
    void Start()
    {
        m_ButtonText.text = m_Response;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
