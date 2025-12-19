using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using BootSentry.Backup;

namespace BootSentry.Backup.Tests;

public class BackupStorageTests : IDisposable
{
    private readonly Mock<ILogger<BackupStorage>> _loggerMock;
    private readonly string _testBasePath;
    private readonly BackupStorage _storage;

    public BackupStorageTests()
    {
        _loggerMock = new Mock<ILogger<BackupStorage>>();
        _testBasePath = Path.Combine(Path.GetTempPath(), $"BootSentryTest_{Guid.NewGuid():N}");
        _storage = new BackupStorage(_loggerMock.Object, _testBasePath);
    }

    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testBasePath))
        {
            try
            {
                Directory.Delete(_testBasePath, recursive: true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    // ============================================================
    // Transaction ID Validation Tests (Path Traversal Prevention)
    // ============================================================

    [Theory]
    [InlineData("..")]
    [InlineData("..\\parent")]
    [InlineData("../parent")]
    [InlineData("test\\..\\..\\etc")]
    [InlineData("test/../../../etc")]
    public void GetTransactionPath_WithPathTraversal_ThrowsArgumentException(string transactionId)
    {
        var action = () => _storage.GetTransactionPath(transactionId);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*invalid characters*");
    }

    [Theory]
    [InlineData("C:\\Windows")]
    [InlineData("C:/Windows")]
    [InlineData("D:")]
    public void GetTransactionPath_WithAbsolutePath_ThrowsArgumentException(string transactionId)
    {
        var action = () => _storage.GetTransactionPath(transactionId);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*invalid characters*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void GetTransactionPath_WithEmptyOrNull_ThrowsArgumentException(string? transactionId)
    {
        var action = () => _storage.GetTransactionPath(transactionId!);

        action.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be null or empty*");
    }

    [Theory]
    [InlineData("20241219-120000-abcd1234")]
    [InlineData("valid-transaction-id")]
    [InlineData("12345678")]
    public void GetTransactionPath_WithValidId_ReturnsCorrectPath(string transactionId)
    {
        var result = _storage.GetTransactionPath(transactionId);

        result.Should().Be(Path.Combine(_testBasePath, transactionId));
    }

    // ============================================================
    // Transaction Directory Tests
    // ============================================================

    [Fact]
    public void CreateTransactionDirectory_CreatesDirectory()
    {
        var transactionId = "test-transaction-" + Guid.NewGuid().ToString("N")[..8];

        var path = _storage.CreateTransactionDirectory(transactionId);

        Directory.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void GetManifestPath_ReturnsCorrectPath()
    {
        var transactionId = "test-transaction";

        var result = _storage.GetManifestPath(transactionId);

        result.Should().EndWith("manifest.json");
        result.Should().Contain(transactionId);
    }

    // ============================================================
    // Manifest Tests
    // ============================================================

    [Fact]
    public async Task SaveAndLoadManifest_RoundTrips()
    {
        var transactionId = "roundtrip-" + Guid.NewGuid().ToString("N")[..8];
        _storage.CreateTransactionDirectory(transactionId);

        var manifest = new Models.TransactionManifest
        {
            Id = transactionId,
            Timestamp = DateTime.UtcNow,
            User = "TestUser",
            ActionType = Core.Enums.ActionType.Disable,
            EntryId = "entry-123",
            EntryDisplayName = "Test Entry",
            EntryType = Core.Enums.EntryType.RegistryRun,
            EntryScope = Core.Enums.EntryScope.User,
            SourcePath = @"HKCU\Software\Test",
            Status = Models.TransactionStatus.Pending
        };

        await _storage.SaveManifestAsync(manifest);
        var loaded = await _storage.LoadManifestAsync(transactionId);

        loaded.Should().NotBeNull();
        loaded!.Id.Should().Be(transactionId);
        loaded.User.Should().Be("TestUser");
        loaded.EntryDisplayName.Should().Be("Test Entry");
    }

    [Fact]
    public async Task LoadManifest_WithNonExistentTransaction_ReturnsNull()
    {
        var result = await _storage.LoadManifestAsync("non-existent-id");

        result.Should().BeNull();
    }

    // ============================================================
    // Delete Transaction Tests
    // ============================================================

    [Fact]
    public void DeleteTransaction_RemovesDirectory()
    {
        var transactionId = "delete-test-" + Guid.NewGuid().ToString("N")[..8];
        var path = _storage.CreateTransactionDirectory(transactionId);
        Directory.Exists(path).Should().BeTrue();

        _storage.DeleteTransaction(transactionId);

        Directory.Exists(path).Should().BeFalse();
    }

    [Fact]
    public void DeleteTransaction_WithNonExistent_DoesNotThrow()
    {
        var action = () => _storage.DeleteTransaction("non-existent");

        action.Should().NotThrow();
    }

    // ============================================================
    // GetAllManifests Tests
    // ============================================================

    [Fact]
    public async Task GetAllManifests_ReturnsAllManifests()
    {
        // Create multiple transactions
        for (int i = 0; i < 3; i++)
        {
            var id = $"manifest-test-{i}-{Guid.NewGuid():N}"[..20];
            _storage.CreateTransactionDirectory(id);

            var manifest = new Models.TransactionManifest
            {
                Id = id,
                Timestamp = DateTime.UtcNow.AddMinutes(-i),
                User = "TestUser",
                ActionType = Core.Enums.ActionType.Disable,
                EntryId = $"entry-{i}",
                EntryDisplayName = $"Test Entry {i}",
                EntryType = Core.Enums.EntryType.RegistryRun,
                EntryScope = Core.Enums.EntryScope.User,
                SourcePath = @"HKCU\Software\Test",
                Status = Models.TransactionStatus.Committed
            };

            await _storage.SaveManifestAsync(manifest);
        }

        var manifests = await _storage.GetAllManifestsAsync();

        manifests.Count.Should().Be(3);
        // Should be sorted by timestamp descending (most recent first)
        manifests[0].Timestamp.Should().BeOnOrAfter(manifests[1].Timestamp);
    }

    [Fact]
    public async Task GetAllManifests_WithEmptyStorage_ReturnsEmptyList()
    {
        var manifests = await _storage.GetAllManifestsAsync();

        manifests.Should().BeEmpty();
    }
}
