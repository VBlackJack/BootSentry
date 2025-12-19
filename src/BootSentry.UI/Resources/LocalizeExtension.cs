using System.Windows.Data;
using System.Windows.Markup;
using System.ComponentModel;

namespace BootSentry.UI.Resources;

/// <summary>
/// Markup extension for localized strings in XAML.
/// Usage: Text="{res:Localize KeyName}"
/// </summary>
[MarkupExtensionReturnType(typeof(string))]
public class LocalizeExtension : MarkupExtension
{
    public LocalizeExtension() { }

    public LocalizeExtension(string key)
    {
        Key = key;
    }

    [ConstructorArgument("key")]
    public string Key { get; set; } = string.Empty;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Key))
            return string.Empty;

        var binding = new Binding
        {
            Source = LocalizationManager.Instance,
            Path = new System.Windows.PropertyPath($"[{Key}]"),
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
}

/// <summary>
/// Singleton manager for localization that supports dynamic language switching.
/// </summary>
public class LocalizationManager : INotifyPropertyChanged
{
    private static LocalizationManager? _instance;
    public static LocalizationManager Instance => _instance ??= new LocalizationManager();

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationManager()
    {
        Strings.LanguageChanged += (_, _) => OnLanguageChanged();
    }

    public string this[string key] => Strings.Get(key);

    private void OnLanguageChanged()
    {
        // Notify all bindings that the indexer has changed
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }

    /// <summary>
    /// Forces refresh of all localized strings.
    /// </summary>
    public void Refresh()
    {
        OnLanguageChanged();
    }
}
