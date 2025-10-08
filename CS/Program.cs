using AiGrammarChecker.Components;
using Microsoft.Extensions.AI;
using OpenAI;

string OpenAIKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
string ModelId = "gpt-4o-mini";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor(options =>
{
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});
builder.Services.AddMvc();

var chatClient = new OpenAIClient(OpenAIKey)
    .GetChatClient(ModelId)
    .AsIChatClient();

builder.Services.AddScoped<IChatClient>((provider) => chatClient);
builder.Services.AddDevExpressAI();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();