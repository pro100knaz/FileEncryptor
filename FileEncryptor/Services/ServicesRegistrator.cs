using FileEncryptor.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FileEncryptor.Services
{
    static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IUserDialog, UserDialogService>()
            .AddTransient<IEncryptor, Rfc2898Encryptor>();

    }
}
