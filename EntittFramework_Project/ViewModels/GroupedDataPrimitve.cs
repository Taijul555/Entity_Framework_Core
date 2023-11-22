using EntittFramework_Project.Models;

namespace EntittFramework_Project.ViewModels
{
    public class GroupedDataPrimitve<T>
    {
        public string Key { get; set; } = default!;
        public T Data { get; set; } = default!;
    }
}
