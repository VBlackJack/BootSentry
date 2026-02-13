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

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Core-level interface for the knowledge base service.
/// Exposes lifecycle and metadata methods that do not require Knowledge-project types,
/// avoiding a circular dependency between Core and Knowledge.
/// For strongly-typed access to KnowledgeEntry (search, find), consumers should resolve
/// the concrete KnowledgeService directly. Cross-project lookup is available via the
/// <see cref="Services.KnowledgeFinder"/> delegate.
/// </summary>
public interface IKnowledgeService : IDisposable
{
    /// <summary>
    /// Search for a knowledge entry by name, executable, or publisher.
    /// Returns an untyped reference to avoid a circular project dependency.
    /// The returned object is a KnowledgeEntry from BootSentry.Knowledge.Models.
    /// </summary>
    /// <param name="name">Display name of the startup entry.</param>
    /// <param name="executable">Executable path or name.</param>
    /// <param name="publisher">Publisher name.</param>
    /// <returns>A KnowledgeEntry object, or null if not found.</returns>
    object? FindEntry(string? name, string? executable, string? publisher);

    /// <summary>
    /// Get total entry count in the knowledge base.
    /// </summary>
    /// <returns>The number of entries in the knowledge base.</returns>
    int GetCount();

    /// <summary>
    /// Seed the database with initial entries if empty or needs update.
    /// </summary>
    void SeedIfEmpty();
}
