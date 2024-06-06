using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponseButton : MonoBehaviour
{
    public Response m_Response;

    public TextMeshProUGUI m_ButtonText;

    [NonSerialized]
    public bool m_IsSuccessfulReply = false;
}
