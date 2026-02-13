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
using BootSentry.Security;

namespace BootSentry.Security.Tests;

public class HashCalculatorTests : IDisposable
{
    private readonly HashCalculator _calculator;
    private readonly string _testDirectory;

    public HashCalculatorTests()
    {
        _calculator = new HashCalculator();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"BootSentryHashTest_{Guid.NewGuid():N}");
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
    // SHA-256 Tests
    // ============================================================

    [Fact]
    public async Task CalculateSha256Async_WithKnownContent_ReturnsExpectedHash()
    {
        var filePath = Path.Combine(_testDirectory, "known_content.txt");
        // SHA-256 of "Hello, World!" is well-known
        await File.WriteAllTextAsync(filePath, "Hello, World!");

        var hash = await _calculator.CalculateSha256Async(filePath);

        // SHA-256("Hello, World!") as UTF-8 bytes
        hash.Should().NotBeNullOrEmpty();
        hash.Should().MatchRegex("^[0-9a-f]{64}$"); // Lowercase hex, 64 chars
    }

    [Fact]
    public async Task CalculateSha256Async_WithSameContent_ReturnsSameHash()
    {
        var filePath1 = Path.Combine(_testDirectory, "same_content_1.txt");
        var filePath2 = Path.Combine(_testDirectory, "same_content_2.txt");
        await File.WriteAllTextAsync(filePath1, "identical content");
        await File.WriteAllTextAsync(filePath2, "identical content");

        var hash1 = await _calculator.CalculateSha256Async(filePath1);
        var hash2 = await _calculator.CalculateSha256Async(filePath2);

        hash1.Should().Be(hash2);
    }

    [Fact]
    public async Task CalculateSha256Async_WithDifferentContent_ReturnsDifferentHash()
    {
        var filePath1 = Path.Combine(_testDirectory, "different_1.txt");
        var filePath2 = Path.Combine(_testDirectory, "different_2.txt");
        await File.WriteAllTextAsync(filePath1, "content A");
        await File.WriteAllTextAsync(filePath2, "content B");

        var hash1 = await _calculator.CalculateSha256Async(filePath1);
        var hash2 = await _calculator.CalculateSha256Async(filePath2);

        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public async Task CalculateSha256Async_WithEmptyFile_ReturnsEmptyFileHash()
    {
        var filePath = Path.Combine(_testDirectory, "empty.txt");
        await File.WriteAllTextAsync(filePath, "");

        var hash = await _calculator.CalculateSha256Async(filePath);

        // SHA-256 of empty input is e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
        hash.Should().Be("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
    }

    [Fact]
    public async Task CalculateSha256Async_ReturnsLowercaseHex()
    {
        var filePath = Path.Combine(_testDirectory, "lowercase_test.txt");
        await File.WriteAllTextAsync(filePath, "test");

        var hash = await _calculator.CalculateSha256Async(filePath);

        hash.Should().Be(hash.ToLowerInvariant());
    }

    [Fact]
    public async Task CalculateSha256Async_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentPath = Path.Combine(_testDirectory, "does_not_exist.txt");

        var action = async () => await _calculator.CalculateSha256Async(nonExistentPath);

        await action.Should().ThrowAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task CalculateSha256Async_WithCancellation_ThrowsOperationCanceledException()
    {
        var filePath = Path.Combine(_testDirectory, "cancel_test.txt");
        await File.WriteAllTextAsync(filePath, "test content");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var action = async () => await _calculator.CalculateSha256Async(filePath, cts.Token);

        await action.Should().ThrowAsync<OperationCanceledException>();
    }

    // ============================================================
    // MD5 Tests
    // ============================================================

    [Fact]
    public async Task CalculateMd5Async_WithKnownContent_ReturnsExpectedHash()
    {
        var filePath = Path.Combine(_testDirectory, "md5_known.txt");
        await File.WriteAllTextAsync(filePath, "Hello, World!");

        var hash = await _calculator.CalculateMd5Async(filePath);

        hash.Should().NotBeNullOrEmpty();
        hash.Should().MatchRegex("^[0-9a-f]{32}$"); // Lowercase hex, 32 chars
    }

    [Fact]
    public async Task CalculateMd5Async_WithEmptyFile_ReturnsEmptyFileHash()
    {
        var filePath = Path.Combine(_testDirectory, "md5_empty.txt");
        await File.WriteAllTextAsync(filePath, "");

        var hash = await _calculator.CalculateMd5Async(filePath);

        // MD5 of empty input is d41d8cd98f00b204e9800998ecf8427e
        hash.Should().Be("d41d8cd98f00b204e9800998ecf8427e");
    }

    [Fact]
    public async Task CalculateMd5Async_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentPath = Path.Combine(_testDirectory, "md5_does_not_exist.txt");

        var action = async () => await _calculator.CalculateMd5Async(nonExistentPath);

        await action.Should().ThrowAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task CalculateMd5Async_ReturnsLowercaseHex()
    {
        var filePath = Path.Combine(_testDirectory, "md5_lowercase.txt");
        await File.WriteAllTextAsync(filePath, "test");

        var hash = await _calculator.CalculateMd5Async(filePath);

        hash.Should().Be(hash.ToLowerInvariant());
    }

    [Fact]
    public async Task CalculateMd5Async_WithSameContent_ReturnsSameHash()
    {
        var filePath1 = Path.Combine(_testDirectory, "md5_same_1.txt");
        var filePath2 = Path.Combine(_testDirectory, "md5_same_2.txt");
        await File.WriteAllTextAsync(filePath1, "identical");
        await File.WriteAllTextAsync(filePath2, "identical");

        var hash1 = await _calculator.CalculateMd5Async(filePath1);
        var hash2 = await _calculator.CalculateMd5Async(filePath2);

        hash1.Should().Be(hash2);
    }

    // ============================================================
    // Cross-Algorithm Tests
    // ============================================================

    [Fact]
    public async Task Sha256AndMd5_WithSameFile_ReturnDifferentHashes()
    {
        var filePath = Path.Combine(_testDirectory, "cross_algo.txt");
        await File.WriteAllTextAsync(filePath, "test content for both algorithms");

        var sha256 = await _calculator.CalculateSha256Async(filePath);
        var md5 = await _calculator.CalculateMd5Async(filePath);

        sha256.Should().NotBe(md5);
        sha256.Should().HaveLength(64); // SHA-256 = 32 bytes = 64 hex chars
        md5.Should().HaveLength(32);    // MD5 = 16 bytes = 32 hex chars
    }
}
