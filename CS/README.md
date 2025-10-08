<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/987706933/24.2.6%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1292610)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# DevExpress Blazor AI Chat - Grammar Checker Showcase

The [DevExpress Blazor AI Chat component](https://docs.devexpress.com/Blazor/405290) (`DxAIChat`) provides a powerful foundation for building AI-powered applications. This example demonstrates how to create an intelligent grammar checking and text improvement application using OpenAI's GPT models integrated with DevExpress Blazor components.

This showcase illustrates how you can leverage AI services to build a responsive grammar checker that corrects grammar, improves clarity, and provides detailed explanations for text modifications in your next Blazor application.

![AI Grammar Checker Chat Interface](images/ai-grammar-checker-chat.gif)

## Implementation Details

Configure the OpenAI client and register it as a service for AI integration:

````````csharp
builder.Services.AddSingleton<OpenAIClient>(sp => {
    var configuration = new OpenAICreateOptions(Configuration["OpenAI:ApiKey"]);
    return new OpenAIClient(configuration);
});
````````

Manage the application's state using a custom `AppState` class:

````````csharp
public class AppState {
    public List<Message> Messages { get; } = new();
    public void AddMessage(Message message) {
        Messages.Add(message);
    }
    public void ClearMessages() {
        Messages.Clear();
    }
}
````````

Register the `AppState` class as a singleton service:
````````csharp
builder.Services.AddSingleton<AppState>();
````````

Implement the grammar checking logic in the `SubmitMessage` method:

````````csharp
public async Task SubmitMessage() {
    if (string.IsNullOrWhiteSpace(MessageText)) return;

    // Add user message to chat
    var userMessage = new Message { Content = MessageText, IsUser = true };
    AppState.AddMessage(userMessage);

    // Call AI service to check grammar
    var aiResponse = await OpenAIClient.CreateCompletionAsync(new OpenAI.Models.CompletionRequest {
        Prompt = $"Correct the grammar and improve the clarity of the following text:\n\n{MessageText}",
        MaxTokens = 60
    });

    // Extract AI-generated message
    var aiMessage = new Message { Content = aiResponse.Choices[0].Text.Trim(), IsUser = false };
    AppState.AddMessage(aiMessage);

    // Clear input
    MessageText = string.Empty;
}
````````

Create a user interface with a chat-like input and display area:

````````razor
<DxCard>
    <DxCardHeader>
        <h2>Grammar Checker Chat</h2>
    </DxCardHeader>
    <DxCardContent>
        <div class="chat-container">
            @foreach (var message in AppState.Messages) {
                <div class="message @(message.IsUser ? "user-message" : "ai-message")">
                    @message.Content
                </div>
            }
        </div>
    </DxCardContent>
    <DxCardFooter>
        <div class="input-container">
            <DxTextBox @bind-Value="MessageText" Placeholder="Enter your text here..."
                       ShowClearButton="true">
            </DxTextBox>
            <DxButton Click="SubmitMessage" Text="Check Grammar" />
        </div>
    </DxCardFooter>
</DxCard>

<style>
    .chat-container {
        max-height: 400px;
        overflow-y: auto;
        display: flex;
        flex-direction: column;
    }
    .message {
        padding: 10px;
        border-radius: 5px;
        margin: 5px 0;
        max-width: 70%;
    }
    .user-message {
        background-color: #d1e7dd;
        align-self: flex-end;
    }
    .ai-message {
        background-color: #f8d7da;
        align-self: flex-start;
    }
    .input-container {
        display: flex;
        gap: 10px;
    }
</style>
````````

## Remarks

- This example uses the DevExpress Blazor components suite for the UI.
- The grammar checking functionality is powered by OpenAI's GPT models.
- Customize the `Prompt` in the `SubmitMessage` method to change the AI's behavior.
- Adjust `MaxTokens` to control the length of the AI's responses.
- Explore additional properties of the `CompletionRequest` class for advanced configurations.

## Conclusion

DevExpress Blazor AI Chat component, in conjunction with OpenAI's GPT models, enables the quick creation of AI-powered applications with advanced language processing capabilities. This example provided a glimpse into building a grammar checker application, opening avenues for more intelligent and responsive Blazor applications.

## Files to Review

- [Index.razor](CS/Expando/Components/Pages/Index.razor)
- [WeatherForecastService.cs](CS/Expando/Services/WeatherForecastService.cs)
- [Program.cs](CS/Expando/Program.cs)

## Documentation

- [DxGrid Class](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid)
- [Bind Blazor Grid to Data](https://docs.devexpress.com/Blazor/403737/components/grid/bind-to-data)
- [Editing and Validation in Blazor Grid](https://docs.devexpress.com/Blazor/403454/components/grid/editing-and-validation)

<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-editable-grid-with-expandoobject&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-editable-grid-with-expandoobject&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
