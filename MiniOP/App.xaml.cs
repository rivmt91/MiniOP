using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MiniOP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<ViewModels.MainViewModel>()
                .BuildServiceProvider()
                );
        }
    }
}
