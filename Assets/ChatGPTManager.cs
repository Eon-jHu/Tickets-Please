using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using System.Threading.Tasks;

public class ChatGPTManager : MonoBehaviour
{
    private OpenAIApi openAI = new OpenAIApi();

    private readonly List<ChatMessage> messages = new List<ChatMessage>();

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
        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4.1-nano";

        // Create the response
        try
        {
            var response = await openAI.CreateChatCompletion(request);

            if (response.Choices != null && response.Choices.Count > 0)
            {
                var choice = response.Choices[0];
                var chatResponse = choice.Message;
                messages.Add(chatResponse); // Add to history
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
}
