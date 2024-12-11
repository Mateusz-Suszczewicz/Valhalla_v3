using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

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
    [Parameter]
    public EventCallback<string> CloseModal { get; set; }

    // Obsługa walidacji formularza i wywołanie callbacku
    private async Task HandleValidStationSubmit()
    {
        Console.WriteLine($"answer: {Answer}");
        await SubmitChoice.InvokeAsync(Answer);
    }
    
    private async Task Close()
    {
        Answer = "";
        isChoiceOpen = false;
        await CloseModal.InvokeAsync();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            await Close();
        }
        else if(e.CtrlKey &&  e.Key == "s")
        {
           await  HandleValidStationSubmit();
        }
    }

}
