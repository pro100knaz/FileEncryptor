using Microsoft.Extensions.DependencyInjection;

namespace FileEncryptor.ViewModel
{
    static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>();

    }
}
