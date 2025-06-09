using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

public class ChatGPTManager : MonoBehaviour
{
    // Single API Channel
    private OpenAIApi openAI = new();

    private Dictionary<string, List<ChatMessage>> NPCMessageHistories = new();

    // ----------------------- Singleton -----------------------
    public static ChatGPTManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<ChatGPTManager>();
        }
        else if (Instance != FindObjectOfType<ChatGPTManager>())
        {
            Destroy(FindObjectOfType<ChatGPTManager>());
        }
    }

    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------

    // Ask for a response with NPC name memory
    public async Task<string> AskChatGPT(string _npcName, string _prompt)
    {
        if (string.IsNullOrEmpty(_prompt))
        {
            Debug.LogError("Prompt cannot be null or empty.");
            return null; // Or throw an ArgumentException
        }

        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = _prompt;
        newMessage.Role = "user";
        
        string responseContent = null;
        List<ChatMessage> messageList = new();

        // Set message history based on NPC
        if (NPCMessageHistories.ContainsKey(_npcName))
        {
            messageList = NPCMessageHistories[_npcName];
            messageList.Add(newMessage);
        }
        else
        {
            // Create new message list
            messageList.Add(newMessage);
            NPCMessageHistories.TryAdd(_npcName, messageList);
        }

        CreateChatCompletionRequest request = new();

        request.Messages = messageList;
        request.Model = "gpt-4.1-nano";

        // Create the response
        try
        {
            var response = await openAI.CreateChatCompletion(request);

            if (response.Choices != null && response.Choices.Count > 0)
            {
                var choice = response.Choices[0];
                var chatResponse = choice.Message;
                NPCMessageHistories[_npcName].Add(chatResponse); // Add to history
                responseContent = chatResponse.Content;

                Debug.Log($"ChatGPT response: {responseContent}");
            }
            else
            {
                Debug.LogError("ChatGPT response is null.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error calling ChatGPT API: {ex.Message}\n{ex.StackTrace}");
        }

        return responseContent;
    }


    // Ask for a barebones response with NO memory
    public async Task<string> AskChatGPT(string _prompt)
    {
        if (string.IsNullOrEmpty(_prompt))
        {
            Debug.LogError("Prompt cannot be null or empty.");
            return null; // Or throw an ArgumentException
        }

        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = _prompt;
        newMessage.Role = "user";

        string responseContent = null;
        List<ChatMessage> newMessageList = new()
        {
            newMessage
        };

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = newMessageList;
        request.Model = "gpt-4.1-nano";

        // Create the response
        try
        {
            var response = await openAI.CreateChatCompletion(request);

            if (response.Choices != null && response.Choices.Count > 0)
            {
                var choice = response.Choices[0];
                var chatResponse = choice.Message;
                responseContent = chatResponse.Content;

                Debug.Log($"ChatGPT response: {responseContent}");
            }
            else
            {
                Debug.LogError("ChatGPT response is null.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error calling ChatGPT API: {ex.Message}\n{ex.StackTrace}");
        }

        return responseContent;
    }


    // Splits a given text into sentences based on punctuation
    public string[] SplitIntoSentences(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return new string[0];
        }

        string regex = @"(?<=[.?!])\s+";

        return Regex.Split(response, regex)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToArray();
    }

    // Extracts all quoted strings from a given text
    public string[] ExtractQuotedStrings(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return new string[0];
        }

        string regex = @"""(.+?)""";

        var matches = Regex.Matches(response, regex);

        return matches.Cast<Match>()
                      .Select(m => m.Groups[1].Value)
                      .ToArray();
    }
}
