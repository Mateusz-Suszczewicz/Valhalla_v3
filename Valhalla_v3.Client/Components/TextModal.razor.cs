using Microsoft.AspNetCore.Components;

namespace Valhalla_v3.Client.Components;

public partial class TextModal
{
    [Parameter]
    public string Question { get; set; }

    [Parameter]
    public string Answer { get; set; }

    [Parameter]
    public bool isChoiceOpen { get; set; }

    [Parameter]
    public EventCallback<string> SubmitChoice { get; set; }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        Console.WriteLine($"answer: {Answer}");
        await SubmitChoice.InvokeAsync(Answer);
    }

    private void Close()
    {
        isChoiceOpen = false;
        Answer = "";
    }
}
