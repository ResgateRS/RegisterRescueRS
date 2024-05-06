namespace RegisterRescueRS.Domain.Application.Services
{
    public class BaseService
    {
        protected readonly IServiceProvider _serviceProvider;

        public BaseService(IServiceProvider serviceProvider) { this._serviceProvider = serviceProvider; }
    }
}

