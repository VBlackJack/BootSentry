using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using BootSentry.Core.Enums;
using BootSentry.Providers;

namespace BootSentry.Providers.Tests;

/// <summary>
/// Tests for IFEO Provider.
/// </summary>
public class IFEOProviderTests
{
    private readonly IFEOProvider _provider;

    public IFEOProviderTests()
    {
        _provider = new IFEOProvider(NullLogger<IFEOProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsIFEO()
    {
        _provider.EntryType.Should().Be(EntryType.IFEO);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.IFEO);
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for Winlogon Provider.
/// </summary>
public class WinlogonProviderTests
{
    private readonly WinlogonProvider _provider;

    public WinlogonProviderTests()
    {
        _provider = new WinlogonProvider(NullLogger<WinlogonProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsWinlogon()
    {
        _provider.EntryType.Should().Be(EntryType.Winlogon);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
        // Winlogon should always have Shell and Userinit entries
        entries.Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.Winlogon);
        }
    }

    [Fact]
    public async Task ScanAsync_ContainsExplorerShell()
    {
        var entries = await _provider.ScanAsync();

        entries.Should().Contain(e =>
            e.SourceName == "Shell" &&
            e.CommandLineRaw != null &&
            e.CommandLineRaw.Contains("explorer", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for BHO Provider.
/// </summary>
public class BHOProviderTests
{
    private readonly BHOProvider _provider;

    public BHOProviderTests()
    {
        _provider = new BHOProvider(NullLogger<BHOProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsBHO()
    {
        _provider.EntryType.Should().Be(EntryType.BHO);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.BHO);
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for ShellExtension Provider.
/// </summary>
public class ShellExtensionProviderTests
{
    private readonly ShellExtensionProvider _provider;

    public ShellExtensionProviderTests()
    {
        _provider = new ShellExtensionProvider(NullLogger<ShellExtensionProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsShellExtension()
    {
        _provider.EntryType.Should().Be(EntryType.ShellExtension);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.ShellExtension);
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for PrintMonitor Provider.
/// </summary>
public class PrintMonitorProviderTests
{
    private readonly PrintMonitorProvider _provider;

    public PrintMonitorProviderTests()
    {
        _provider = new PrintMonitorProvider(NullLogger<PrintMonitorProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsPrintMonitor()
    {
        _provider.EntryType.Should().Be(EntryType.PrintMonitor);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
        // Standard Windows installation has at least the Local Port monitor
        entries.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.PrintMonitor);
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for SessionManager Provider.
/// </summary>
public class SessionManagerProviderTests
{
    private readonly SessionManagerProvider _provider;

    public SessionManagerProviderTests()
    {
        _provider = new SessionManagerProvider(NullLogger<SessionManagerProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsSessionManager()
    {
        _provider.EntryType.Should().Be(EntryType.SessionManager);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
        // Standard Windows has autocheck autochk *
        entries.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.SessionManager);
        }
    }

    [Fact]
    public async Task ScanAsync_ContainsAutocheck()
    {
        var entries = await _provider.ScanAsync();

        entries.Should().Contain(e =>
            e.CommandLineRaw != null &&
            e.CommandLineRaw.Contains("autocheck", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for AppInitDlls Provider.
/// </summary>
public class AppInitDllsProviderTests
{
    private readonly AppInitDllsProvider _provider;

    public AppInitDllsProviderTests()
    {
        _provider = new AppInitDllsProvider(NullLogger<AppInitDllsProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsAppInitDlls()
    {
        _provider.EntryType.Should().Be(EntryType.AppInitDlls);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
        // AppInit_DLLs is usually empty on clean systems
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.AppInitDlls);
            // AppInit_DLLs are always critical
            entry.RiskLevel.Should().BeOneOf(RiskLevel.Suspicious, RiskLevel.Critical);
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}

/// <summary>
/// Tests for WinsockLSP Provider.
/// </summary>
public class WinsockLSPProviderTests
{
    private readonly WinsockLSPProvider _provider;

    public WinsockLSPProviderTests()
    {
        _provider = new WinsockLSPProvider(NullLogger<WinsockLSPProvider>.Instance);
    }

    [Fact]
    public void EntryType_IsWinsockLSP()
    {
        _provider.EntryType.Should().Be(EntryType.WinsockLSP);
    }

    [Fact]
    public void DisplayName_IsNotEmpty()
    {
        _provider.DisplayName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void RequiresAdminToRead_IsFalse()
    {
        _provider.RequiresAdminToRead.Should().BeFalse();
    }

    [Fact]
    public void RequiresAdminToModify_IsTrue()
    {
        _provider.RequiresAdminToModify.Should().BeTrue();
    }

    [Fact]
    public void IsAvailable_ReturnsTrue()
    {
        _provider.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public async Task ScanAsync_ReturnsEntries()
    {
        var entries = await _provider.ScanAsync();
        entries.Should().NotBeNull();
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveRequiredProperties()
    {
        var entries = await _provider.ScanAsync();

        foreach (var entry in entries)
        {
            entry.Id.Should().NotBeNullOrEmpty();
            entry.DisplayName.Should().NotBeNullOrEmpty();
            entry.SourcePath.Should().NotBeNullOrEmpty();
            entry.Type.Should().Be(EntryType.WinsockLSP);
        }
    }

    [Fact]
    public async Task ScanAsync_WithCancellation_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(() => _provider.ScanAsync(cts.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}
