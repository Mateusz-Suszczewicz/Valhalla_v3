using Microsoft.AspNetCore.Components;

namespace Valhalla_v3.Client.Components;

public partial class ChoicModal
{
    [Parameter]
    public string Question { get; set; }
    
    [Parameter]
    public bool isChoiceOpen { get; set; }

    [Parameter]
    public EventCallback<bool> SubmitChoice { get; set; }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit(bool answer)
    {
        Console.WriteLine($"answer: {answer}");
        await SubmitChoice.InvokeAsync(answer);
    }
}