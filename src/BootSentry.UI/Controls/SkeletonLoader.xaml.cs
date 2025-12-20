using System.Windows;

namespace BootSentry.UI.Controls;

/// <summary>
/// A skeleton loading placeholder control.
/// </summary>
public partial class SkeletonLoader : System.Windows.Controls.UserControl
{
    public static readonly DependencyProperty RowCountProperty =
        DependencyProperty.Register(nameof(RowCount), typeof(int), typeof(SkeletonLoader),
            new PropertyMetadata(5, OnRowCountChanged));

    public int RowCount
    {
        get => (int)GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    public SkeletonLoader()
    {
        InitializeComponent();
        GenerateRows();
    }

    private static void OnRowCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SkeletonLoader loader)
        {
            loader.GenerateRows();
        }
    }

    private void GenerateRows()
    {
        var random = new Random();
        var widths = new List<double>();

        for (int i = 0; i < RowCount; i++)
        {
            // Random width between 100 and 250
            widths.Add(100 + random.NextDouble() * 150);
        }

        SkeletonRows.ItemsSource = widths;
    }
}
