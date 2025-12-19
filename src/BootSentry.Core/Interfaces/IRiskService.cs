using BootSentry.Core.Enums;
using BootSentry.Core.Models;
using BootSentry.Core.Services;

namespace BootSentry.Core.Interfaces;

/// <summary>
/// Service interface for risk analysis of startup entries.
/// Centralizes all risk logic to ensure consistent risk assessment.
/// </summary>
public interface IRiskService
{
    /// <summary>
    /// Analyzes a startup entry and returns a detailed risk assessment.
    /// </summary>
    /// <param name="entry">The startup entry to analyze.</param>
    /// <returns>A risk assessment containing factors and recommended risk level.</returns>
    RiskAssessment Analyze(StartupEntry entry);

    /// <summary>
    /// Updates the risk level of an entry based on analysis.
    /// </summary>
    /// <param name="entry">The entry to update.</param>
    void UpdateRiskLevel(StartupEntry entry);

    /// <summary>
    /// Checks if a publisher is in the trusted publishers list.
    /// </summary>
    /// <param name="publisher">The publisher name to check.</param>
    /// <returns>True if the publisher is trusted.</returns>
    bool IsTrustedPublisher(string? publisher);

    /// <summary>
    /// Checks if a file path is in a suspicious location.
    /// </summary>
    /// <param name="path">The file path to check.</param>
    /// <returns>True if the location is suspicious.</returns>
    bool IsSuspiciousLocation(string? path);

    /// <summary>
    /// Determines risk level from signature status and publisher.
    /// </summary>
    /// <param name="signatureStatus">The signature status.</param>
    /// <param name="publisher">The publisher name.</param>
    /// <returns>The recommended risk level.</returns>
    RiskLevel DetermineBaseRiskLevel(SignatureStatus signatureStatus, string? publisher);
}
