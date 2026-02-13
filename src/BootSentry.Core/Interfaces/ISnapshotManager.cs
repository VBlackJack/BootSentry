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

using BootSentry.Core.Models;
using BootSentry.Core.Snapshots;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Interface for managing startup snapshots (create, save, load, delete, compare).
/// </summary>
public interface ISnapshotManager
{
    /// <summary>
    /// Creates a snapshot from the current list of entries.
    /// </summary>
    /// <param name="name">Name for the snapshot.</param>
    /// <param name="description">Description for the snapshot.</param>
    /// <param name="entries">The startup entries to include.</param>
    /// <returns>A new <see cref="StartupSnapshot"/> instance.</returns>
    StartupSnapshot CreateSnapshot(string name, string description, IEnumerable<StartupEntry> entries);

    /// <summary>
    /// Saves a snapshot to disk.
    /// </summary>
    /// <param name="snapshot">The snapshot to persist.</param>
    Task SaveSnapshotAsync(StartupSnapshot snapshot);

    /// <summary>
    /// Loads all snapshots from the storage directory.
    /// </summary>
    /// <returns>A list of snapshots ordered by creation date descending.</returns>
    Task<List<StartupSnapshot>> LoadSnapshotsAsync();

    /// <summary>
    /// Deletes a snapshot file.
    /// </summary>
    /// <param name="snapshot">The snapshot to delete.</param>
    void DeleteSnapshot(StartupSnapshot snapshot);

    /// <summary>
    /// Compares two snapshots and returns the differences.
    /// </summary>
    /// <param name="baseSnapshot">The baseline snapshot.</param>
    /// <param name="targetSnapshot">The target snapshot to compare against.</param>
    /// <returns>A comparison result detailing added, removed, and modified entries.</returns>
    SnapshotComparisonResult Compare(StartupSnapshot baseSnapshot, StartupSnapshot targetSnapshot);
}
