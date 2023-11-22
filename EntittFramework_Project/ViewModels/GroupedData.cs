using EntittFramework_Project.Models;

namespace EntittFramework_Project.ViewModels
{
    public class GroupedData
    {
        public string Key { get; set; } = default!;
        public IEnumerable<Sale> Data { get; set; } = default!;
    }
}
