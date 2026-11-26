using System.Windows;
using AnnotationsExtractor.Views;

namespace AnnotationsExtractor;

public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var splashWindow = new SplashWindow();

        MainWindow = splashWindow;
        splashWindow.Show();

        // Solo para comprobar visualmente que el Splash funciona.
        await Task.Delay(2000);

        var mainWindow = new MainWindow();

        MainWindow = mainWindow;
        mainWindow.Show();

        splashWindow.Close();
    }
}