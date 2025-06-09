using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    // ----------------------- Singleton -----------------------
    public static StoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<StoryManager>();
        }
        else if (Instance != FindObjectOfType<StoryManager>())
        {
            Destroy(FindObjectOfType<StoryManager>());
        }
    }

    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------

    private string Scenario = "You are in a train and the player is the train conductor," +
        "looking to collect your tickets." +
        "The player has approached you to do so now. " +
        "Don't give the player your ticket until after the player responds. ";

    private string StoryEvent = ""; // eg. "the player has turned off the lights in the carriage"



    // Crafts a prompt for the NPC based on the current scenario and player attitude
    public string CraftDialoguePrompt(ConversableObject _npc, PlayerController _player)
    {
        // Initial NPC prompt
        string prompt = $"Roleplay as a {_npc.m_InitialDialogue.m_Name} NPC " +
            $"who is {_npc.m_Temperament.TemperamentPrompt[_npc.m_Temperament.CurrentTemperament]}. " +
            Scenario +
            $"The player has acted {_player.PlayerAttitude.AttitudePrompts[_player.PlayerAttitude.AttitudeValue]} so far. ";
    
        // Add any relevant events to the prompt (rumors)
        if (!string.IsNullOrEmpty(StoryEvent))
        {
            prompt += $"(Additionally, {StoryEvent}, which you can mention if relevant.)";
        }

        return prompt;
    }

    public string CraftDialoguePromptContinued(Response _response)
    {
        // Continued NPC prompt
        string prompt = $"Alright, the player will reply with: " +
            $"\"{_response.m_PlayerResponse}\".";

        return prompt;
    }

    // Crafts a prompt for the player to respond to the NPC's dialogue
    public string CraftResponsesPrompt()
    {
        return "Generate 4 one-sentence responses which the player could say in response to the above dialogue. " +
            "The first response should be the most straightforwards and confrontational, " +
            "and the rest of the responses should get increasingly more softer-spoken, up until the fourth response, " +
            "which is the most roundabout and gentle.\n" +
            "Encapsulate each response with quotation marks.";
    }
}
