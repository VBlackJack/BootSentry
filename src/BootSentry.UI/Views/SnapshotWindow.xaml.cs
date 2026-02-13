using BootSentry.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Controls;

namespace BootSentry.UI.Views;

public partial class SnapshotWindow : FluentWindow
{
    public SnapshotWindow()
    {
        InitializeComponent();
        // Resolve ViewModel from App.Services
        DataContext = App.Services.GetRequiredService<SnapshotViewModel>();
    }
}
