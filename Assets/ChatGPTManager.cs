using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using System.Threading.Tasks;

public class ChatGPTManager : MonoBehaviour
{
    private OpenAIApi openAI = new OpenAIApi();

    private List<ChatMessage> messages = new List<ChatMessage>();

    public async void AskChatGPT(string _prompt)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = _prompt;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4o-mini";

        // Create the response
        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            Debug.Log("ChatGPT Response: " + chatResponse.Content);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
