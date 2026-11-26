using System.Windows;

namespace AnnotationsExtractor.Views;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();
    }

    public void SetStatus(string message)
    {
        StatusTextBlock.Text = message;
    }
}