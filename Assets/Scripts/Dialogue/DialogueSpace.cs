using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpace : MonoBehaviour
{
    [SerializeField]
    private CameraFollow m_FollowCam;

    private float m_Offset = 400.0f;

    // Start is called before the first frame update
    void Start()
    {
        Disable();
    }

    public void Enable()
    {
        // Adjust the offset of cam so you can see the characters
        m_FollowCam.yOffset -= m_Offset;
        m_FollowCam.MinPosition.y -= m_Offset;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        // Revert offset shift
        m_FollowCam.yOffset += m_Offset;
        m_FollowCam.MinPosition.y += m_Offset;
        gameObject.SetActive(false);
    }
}
