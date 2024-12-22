using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;

namespace Valhalla_v3.Client.Pages.Admin;

public partial class OperatorModal
{
    [Parameter]
    public Operator oper { get; set; } = new();

    [Parameter]
    public EventCallback<Operator> OperSubmit { get; set; }
    
    [Parameter]
    public EventCallback<bool> OnClose { get; set; }
    
    private async Task HandleValidOperSubmit()
    {
        await OperSubmit.InvokeAsync(oper);
        oper = new();
    }
    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            await Close();
        }
        else if (e.CtrlKey && e.Key == "s")
        {
            await HandleValidOperSubmit();
        }
    }
    private async Task Close()
    {
        oper = new Operator(); // Czyszczenie danych
        await OnClose.InvokeAsync(); // Informacja o zamknięciu
    }
}
