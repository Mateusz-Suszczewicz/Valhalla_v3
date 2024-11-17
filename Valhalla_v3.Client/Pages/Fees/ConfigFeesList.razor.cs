using Valhalla_v3.Shared.Fees;
namespace Valhalla_v3.Client.Pages.Fees;
public partial class ConfigFeesList
{
    public List<FeeConfig> fees { get; set; } = new List<FeeConfig>();
    public bool isModalOpen = false;
    public int Id { get; set; }


    protected override async Task OnInitializedAsync()
    {

        fees = new List<FeeConfig>(){new FeeConfig(){
            Id = 1,
            Name = "prąd",
            DefultAmount = 200,
            CycleOf = 1,
            disable = false,
            DateStart = DateTime.Now,
            DateEnd = DateTime.Now.AddMonths(5)
        },
        new FeeConfig(){
            Id = 2,
            Name = "Czynsz",
            DefultAmount = 5000,
            CycleOf = 2,
            disable = true,
            DateStart = DateTime.Now,
            DateEnd = DateTime.Now.AddMonths(5)
        }
        };
    }
    public void CloseModal()
    {
        isModalOpen = false;
    }

    public void OpenModal(int id)
    {
        Id = id;
        isModalOpen = true;
    }

    public void delete(int id)
    {

    }
}
