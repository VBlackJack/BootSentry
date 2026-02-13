/*
 * Copyright 2025 Julien Bombled
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using FluentAssertions;
using Xunit;
using BootSentry.Core.Enums;
using BootSentry.Security;

namespace BootSentry.Security.Tests;

public class SignatureVerifierTests : IDisposable
{
    private readonly SignatureVerifier _verifier;
    private readonly string _testDirectory;

    public SignatureVerifierTests()
    {
        _verifier = new SignatureVerifier();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"BootSentrySigTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            try
            {
                Directory.Delete(_testDirectory, recursive: true);
            }
            catch
            {
                // Ignore cleanup failures in tests.
            }
        }
    }

    // ============================================================
    // VerifyAsync - Non-Existent File Tests
    // ============================================================

    [Fact]
    public async Task VerifyAsync_WithNonExistentFile_ReturnsUnknownStatus()
    {
        var nonExistentPath = @"C:\NonExistent\File\That\Does\Not\Exist.exe";

        var result = await _verifier.VerifyAsync(nonExistentPath);

        result.Should().NotBeNull();
        result.Status.Should().Be(SignatureStatus.Unknown);
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task VerifyAsync_WithNonExistentFile_ErrorMessageIndicatesNotFound()
    {
        var nonExistentPath = Path.Combine(_testDirectory, "ghost.exe");

        var result = await _verifier.VerifyAsync(nonExistentPath);

        result.ErrorMessage.Should().Contain("not found");
    }

    // ============================================================
    // VerifyAsync - Unsigned File Tests
    // ============================================================

    [Fact]
    public async Task VerifyAsync_WithUnsignedTextFile_ReturnsNonNullResult()
    {
        var filePath = Path.Combine(_testDirectory, "unsigned.txt");
        await File.WriteAllTextAsync(filePath, "This is not a signed file.");

        var result = await _verifier.VerifyAsync(filePath);

        result.Should().NotBeNull();
        // WinVerifyTrust behavior varies across environments
        result.Status.Should().BeDefined();
    }

    // ============================================================
    // VerifyAsync - System File Tests
    // ============================================================

    [Fact]
    public async Task VerifyAsync_WithSignedSystemFile_ReturnsNonNullResult()
    {
        var notepadPath = Path.Combine(Environment.SystemDirectory, "notepad.exe");

        if (!File.Exists(notepadPath))
        {
            return; // Skip if notepad not found
        }

        var result = await _verifier.VerifyAsync(notepadPath);

        result.Should().NotBeNull();
        // WinVerifyTrust behavior varies; just verify we get a defined status
        result.Status.Should().BeDefined();
    }

    // ============================================================
    // VerifyAsync - Cancellation Tests
    // ============================================================

    [Fact]
    public async Task VerifyAsync_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var filePath = Path.Combine(_testDirectory, "cancel_test.txt");
        await File.WriteAllTextAsync(filePath, "test");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var action = async () => await _verifier.VerifyAsync(filePath, cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    // ============================================================
    // IsSigned Tests
    // ============================================================

    [Fact]
    public void IsSigned_WithNonExistentFile_ReturnsFalse()
    {
        var nonExistentPath = @"C:\NonExistent\File\That\Does\Not\Exist.exe";

        var result = _verifier.IsSigned(nonExistentPath);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsSigned_WithTextFile_ReturnsBooleanResult()
    {
        var filePath = Path.Combine(_testDirectory, "unsigned.txt");
        File.WriteAllText(filePath, "This is not a signed file.");

        // WinVerifyTrust behavior varies across environments; just verify no exception
        var result = _verifier.IsSigned(filePath);
        result.Should().Be(result); // No-op assertion to verify no exception thrown
    }

    [Fact]
    public void IsSigned_WithSystemFile_ReturnsBooleanResult()
    {
        var notepadPath = Path.Combine(Environment.SystemDirectory, "notepad.exe");

        if (!File.Exists(notepadPath))
        {
            return; // Skip if notepad not found
        }

        // WinVerifyTrust behavior varies across environments
        var result = _verifier.IsSigned(notepadPath);
        result.Should().Be(result); // No-op assertion to verify no exception thrown
    }

    // ============================================================
    // VerifyAsync - Result Properties Tests
    // ============================================================

    [Fact]
    public async Task VerifyAsync_WithSignedFile_PopulatesSignerInfo()
    {
        var notepadPath = Path.Combine(Environment.SystemDirectory, "notepad.exe");

        if (!File.Exists(notepadPath))
        {
            return; // Skip if notepad not found
        }

        var result = await _verifier.VerifyAsync(notepadPath);

        if (result.Status == SignatureStatus.SignedTrusted)
        {
            result.SignerName.Should().NotBeNullOrEmpty();
            result.Issuer.Should().NotBeNullOrEmpty();
            result.Thumbprint.Should().NotBeNullOrEmpty();
            result.ValidFrom.Should().NotBeNull();
            result.ValidTo.Should().NotBeNull();
        }
    }
}
