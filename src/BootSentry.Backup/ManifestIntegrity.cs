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

using System.Security.Cryptography;
using System.Text;
using BootSentry.Core;

namespace BootSentry.Backup;

/// <summary>
/// Provides HMAC-SHA256 integrity protection for backup manifest files.
/// Uses a machine-specific key derived from environment identifiers to prevent
/// manifests from being copied between machines.
/// </summary>
public static class ManifestIntegrity
{
    /// <summary>
    /// Computes an HMAC-SHA256 signature for the given manifest JSON content.
    /// </summary>
    /// <param name="manifestJson">The serialized manifest JSON content.</param>
    /// <returns>A base64-encoded HMAC-SHA256 signature.</returns>
    public static string ComputeHmac(string manifestJson)
    {
        ArgumentNullException.ThrowIfNull(manifestJson);

        var key = DeriveKey();
        var contentBytes = Encoding.UTF8.GetBytes(manifestJson);

        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(contentBytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verifies that the given HMAC-SHA256 signature matches the manifest JSON content.
    /// Uses a constant-time comparison to prevent timing attacks.
    /// </summary>
    /// <param name="manifestJson">The serialized manifest JSON content.</param>
    /// <param name="expectedHmac">The base64-encoded HMAC to verify against.</param>
    /// <returns><c>true</c> if the HMAC matches; otherwise <c>false</c>.</returns>
    public static bool VerifyHmac(string manifestJson, string expectedHmac)
    {
        ArgumentNullException.ThrowIfNull(manifestJson);
        ArgumentNullException.ThrowIfNull(expectedHmac);

        var computedHmac = ComputeHmac(manifestJson);

        // Use constant-time comparison to prevent timing attacks
        var computedBytes = Convert.FromBase64String(computedHmac);
        var expectedBytes = Convert.FromBase64String(expectedHmac);

        return CryptographicOperations.FixedTimeEquals(computedBytes, expectedBytes);
    }

    /// <summary>
    /// Derives a machine-specific HMAC key from environment identifiers.
    /// Combines machine name, user name, and a fixed salt, then hashes with SHA-256
    /// to produce a consistent 256-bit key.
    /// </summary>
    private static byte[] DeriveKey()
    {
        var rawKey = string.Concat(
            Environment.MachineName,
            Environment.UserName,
            Constants.Security.ManifestHmacKeySalt);

        var rawKeyBytes = Encoding.UTF8.GetBytes(rawKey);
        return SHA256.HashData(rawKeyBytes);
    }
}
