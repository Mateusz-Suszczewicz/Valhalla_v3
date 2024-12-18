using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Text;
using Valhalla_v3.Shared;
using Valhalla_v3.Shared.CarHistory;
using Valhalla_v3.Shared.ToDo;
using static System.Net.WebRequestMethods;
using static MudBlazor.CategoryTypes;
using System.Net.Http.Json;

namespace Valhalla_v3.Client.Pages.Admin;

public partial class Panel
{
    private List<Project>  = new List<Project>();
    private List<GasStation> gasStations = new List<GasStation>();
    private List<Mechanic> mechanics = new List<Mechanic>();
    private List<Operator> operators = new List<Operator>();

    protected override async Task OnInitializedAsync()
    {

    }
    private void 
    }
}
