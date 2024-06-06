using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueSpace : MonoBehaviour
{
    [SerializeField]
    private CameraFollow m_FollowCam;

    public ConversableObject m_ConversingObject { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Enable(ConversableObject _npc)
    {
        // Adjust the layers of the npc and the player
        m_ConversingObject = _npc;
        m_ConversingObject.GetComponent<SortingGroup>().sortingLayerName = "DialogueObjects";
        PlayerController.Instance.GetComponent<SortingGroup>().sortingLayerName = "DialogueObjects";

        // Adjust the offset of cam so you can see the characters
        m_FollowCam.DropForDialogue(true);
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        // Revert offset shift
        m_FollowCam.DropForDialogue(false);

        if (m_ConversingObject != null)
        {
            // Re-adjust the layers of the npc and the player
            m_ConversingObject.GetComponent<SortingGroup>().sortingLayerName = "Interactables";
            m_ConversingObject = null;
        }
        PlayerController.Instance.GetComponent<SortingGroup>().sortingLayerName = "Player";

        gameObject.SetActive(false);
    }
}
