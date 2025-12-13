using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using BootSentry.Core.Enums;
using BootSentry.Providers;

namespace BootSentry.Providers.Tests;

public class RegistryRunProviderTests
{
    private readonly RegistryRunProvider _provider;

    public RegistryRunProviderTests()
    {
        _provider = new RegistryRunProvider(
            NullLogger<RegistryRunProvider>.Instance,
            signatureVerifier: null);
    }

    [Fact]
    public void EntryType_IsRegistryRun()
    {
        _provider.EntryType.Should().Be(EntryType.RegistryRun);
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
        // This is an integration test that reads the actual registry
        var entries = await _provider.ScanAsync();

        // We can't guarantee specific entries, but the scan should succeed
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
            entry.Type.Should().BeOneOf(EntryType.RegistryRun, EntryType.RegistryRunOnce);
            entry.Scope.Should().BeOneOf(EntryScope.User, EntryScope.Machine);
        }
    }

    [Fact]
    public async Task ScanAsync_EntriesHaveUniqueIds()
    {
        var entries = await _provider.ScanAsync();
        var ids = entries.Select(e => e.Id).ToList();

        ids.Should().OnlyHaveUniqueItems();
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
