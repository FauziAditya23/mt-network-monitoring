using mt_network_monitoring.Models;
using mt_network_monitoring.Repositories;

namespace mt_network_monitoring.Services;

public class RouterService
{
    private readonly RouterRepository _repository = new();

    public Task<List<Router>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }
}
