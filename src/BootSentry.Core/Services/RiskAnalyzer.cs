using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;
using BootSentry.Core.Models;

namespace BootSentry.Core.Services;

/// <summary>
/// Analyzes startup entries and assigns risk levels based on heuristics.
/// Implements IRiskService to centralize all risk assessment logic.
/// </summary>
public class RiskAnalyzer : IRiskService
{
    /// <summary>
    /// List of known trusted publishers.
    /// </summary>
    private static readonly HashSet<string> TrustedPublishers = new(StringComparer.OrdinalIgnoreCase)
    {
        "Microsoft Corporation",
        "Microsoft Windows",
        "Google LLC",
        "Google Inc.",
        "Mozilla Corporation",
        "Adobe Inc.",
        "Adobe Systems Incorporated",
        "Intel Corporation",
        "Intel(R) Corporation",
        "NVIDIA Corporation",
        "Advanced Micro Devices, Inc.",
        "AMD",
        "Oracle Corporation",
        "Apple Inc.",
        "Valve Corporation",
        "Realtek Semiconductor Corp.",
        "Logitech",
        "Synaptics Incorporated",
        "Dell Inc.",
        "HP Inc.",
        "Lenovo",
        "ASUS",
        "Zoom Video Communications, Inc.",
        "Slack Technologies, Inc.",
        "Dropbox, Inc.",
        "Spotify AB",
        "Discord Inc.",
        "GitHub, Inc.",
        "JetBrains s.r.o.",
        "Docker Inc",
        "VMware, Inc.",
        "Citrix Systems, Inc."
    };

    /// <summary>
    /// Suspicious file locations.
    /// </summary>
    private static readonly string[] SuspiciousLocations =
    {
        @"\temp\",
        @"\tmp\",
        @"\appdata\local\temp",
        @"\downloads\",
        @"\desktop\",
        @"\public\",
        @"$recycle.bin"
    };

    /// <summary>
    /// System-mimicking filenames often used by malware.
    /// </summary>
    private static readonly string[] MimickedSystemFiles =
    {
        "svchost.exe",
        "csrss.exe",
        "smss.exe",
        "lsass.exe",
        "services.exe",
        "winlogon.exe",
        "explorer.exe",
        "taskmgr.exe",
        "spoolsv.exe"
    };

    /// <summary>
    /// Valid system paths for mimicked files.
    /// </summary>
    private static readonly string[] ValidSystemPaths =
    {
        @"\windows\system32\",
        @"\windows\syswow64\"
    };

    /// <summary>
    /// Analyzes a startup entry and returns a risk assessment.
    /// </summary>
    public RiskAssessment Analyze(StartupEntry entry)
    {
        var assessment = new RiskAssessment
        {
            EntryId = entry.Id,
            OriginalRiskLevel = entry.RiskLevel
        };

        var factors = new List<RiskFactor>();
        var score = 0;

        // Factor 1: Signature status
        var sigFactor = AnalyzeSignature(entry);
        if (sigFactor != null)
        {
            factors.Add(sigFactor);
            score += sigFactor.Score;
        }

        // Factor 2: Publisher
        var pubFactor = AnalyzePublisher(entry);
        if (pubFactor != null)
        {
            factors.Add(pubFactor);
            score += pubFactor.Score;
        }

        // Factor 3: File location
        var locFactor = AnalyzeLocation(entry);
        if (locFactor != null)
        {
            factors.Add(locFactor);
            score += locFactor.Score;
        }

        // Factor 4: Command line analysis
        var cmdFactor = AnalyzeCommandLine(entry);
        if (cmdFactor != null)
        {
            factors.Add(cmdFactor);
            score += cmdFactor.Score;
        }

        // Factor 5: File existence
        var existFactor = AnalyzeFileExistence(entry);
        if (existFactor != null)
        {
            factors.Add(existFactor);
            score += existFactor.Score;
        }

        // Factor 6: Entry type specific checks
        var typeFactor = AnalyzeEntryType(entry);
        if (typeFactor != null)
        {
            factors.Add(typeFactor);
            score += typeFactor.Score;
        }

        // Factor 7: Name mimicking
        var mimicFactor = AnalyzeNameMimicking(entry);
        if (mimicFactor != null)
        {
            factors.Add(mimicFactor);
            score += mimicFactor.Score;
        }

        assessment.Factors = factors;
        assessment.TotalScore = score;
        assessment.RecommendedRiskLevel = ScoreToRiskLevel(score);

        return assessment;
    }

    /// <summary>
    /// Updates the risk level of an entry based on analysis.
    /// </summary>
    public void UpdateRiskLevel(StartupEntry entry)
    {
        var assessment = Analyze(entry);
        entry.RiskLevel = assessment.RecommendedRiskLevel;
    }

    /// <inheritdoc/>
    public bool IsTrustedPublisher(string? publisher)
    {
        if (string.IsNullOrEmpty(publisher))
            return false;
        return TrustedPublishers.Contains(publisher);
    }

    /// <inheritdoc/>
    public bool IsSuspiciousLocation(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        var pathLower = path.ToLowerInvariant();
        return SuspiciousLocations.Any(loc => pathLower.Contains(loc));
    }

    /// <inheritdoc/>
    public RiskLevel DetermineBaseRiskLevel(SignatureStatus signatureStatus, string? publisher)
    {
        // Signed by trusted publisher = Safe
        if (signatureStatus == SignatureStatus.SignedTrusted && IsTrustedPublisher(publisher))
            return RiskLevel.Safe;

        // Signed by trusted publisher without full trust chain = still relatively safe
        if (IsTrustedPublisher(publisher))
            return RiskLevel.Safe;

        // Signed with trusted chain but unknown publisher = Unknown
        if (signatureStatus == SignatureStatus.SignedTrusted)
            return RiskLevel.Unknown;

        // Unsigned or untrusted signature = Unknown
        return RiskLevel.Unknown;
    }

    private static RiskFactor? AnalyzeSignature(StartupEntry entry)
    {
        return entry.SignatureStatus switch
        {
            SignatureStatus.SignedTrusted => new RiskFactor
            {
                Name = "Signature",
                Description = "Signed with a valid trust chain",
                Score = -30,
                Type = RiskFactorType.Positive
            },
            SignatureStatus.SignedUntrusted => new RiskFactor
            {
                Name = "Signature",
                Description = "Signed but certificate not trusted",
                Score = 20,
                Type = RiskFactorType.Warning
            },
            SignatureStatus.Unsigned => new RiskFactor
            {
                Name = "Signature",
                Description = "Unsigned file",
                Score = 10,
                Type = RiskFactorType.Warning
            },
            _ => null
        };
    }

    private static RiskFactor? AnalyzePublisher(StartupEntry entry)
    {
        if (string.IsNullOrEmpty(entry.Publisher))
        {
            return new RiskFactor
            {
                Name = "Publisher",
                Description = "Unknown publisher",
                Score = 5,
                Type = RiskFactorType.Warning
            };
        }

        if (TrustedPublishers.Contains(entry.Publisher))
        {
            return new RiskFactor
            {
                Name = "Publisher",
                Description = $"Trusted publisher: {entry.Publisher}",
                Score = -20,
                Type = RiskFactorType.Positive
            };
        }

        return null;
    }

    private static RiskFactor? AnalyzeLocation(StartupEntry entry)
    {
        if (string.IsNullOrEmpty(entry.TargetPath))
            return null;

        var pathLower = entry.TargetPath.ToLowerInvariant();

        // Check for suspicious locations
        foreach (var suspiciousLoc in SuspiciousLocations)
        {
            if (pathLower.Contains(suspiciousLoc))
            {
                return new RiskFactor
                {
                    Name = "Location",
                    Description = $"File in suspicious location ({suspiciousLoc.Trim('\\')})",
                    Score = 25,
                    Type = RiskFactorType.Negative
                };
            }
        }

        // Standard locations are good
        if (pathLower.Contains(@"\program files\") ||
            pathLower.Contains(@"\program files (x86)\") ||
            pathLower.Contains(@"\windows\"))
        {
            return new RiskFactor
            {
                Name = "Location",
                Description = "File in standard location",
                Score = -10,
                Type = RiskFactorType.Positive
            };
        }

        return null;
    }

    private static RiskFactor? AnalyzeCommandLine(StartupEntry entry)
    {
        if (string.IsNullOrEmpty(entry.CommandLineRaw))
            return null;

        if (Parsing.CommandLineParser.IsSuspiciousCommandLine(entry.CommandLineRaw))
        {
            return new RiskFactor
            {
                Name = "Command Line",
                Description = "Command contains suspicious patterns (encoding, download, etc.)",
                Score = 40,
                Type = RiskFactorType.Negative
            };
        }

        return null;
    }

    private static RiskFactor? AnalyzeFileExistence(StartupEntry entry)
    {
        if (!entry.FileExists && !string.IsNullOrEmpty(entry.TargetPath))
        {
            return new RiskFactor
            {
                Name = "File",
                Description = "Target file does not exist",
                Score = 15,
                Type = RiskFactorType.Warning
            };
        }

        return null;
    }

    private static RiskFactor? AnalyzeEntryType(StartupEntry entry)
    {
        return entry.Type switch
        {
            EntryType.IFEO => new RiskFactor
            {
                Name = "Type",
                Description = "IFEO entry (Image File Execution Options) - common malware vector",
                Score = 30,
                Type = RiskFactorType.Warning
            },
            EntryType.Winlogon => new RiskFactor
            {
                Name = "Type",
                Description = "Winlogon entry - sensitive system area",
                Score = 20,
                Type = RiskFactorType.Warning
            },
            _ => null
        };
    }

    private static RiskFactor? AnalyzeNameMimicking(StartupEntry entry)
    {
        if (string.IsNullOrEmpty(entry.TargetPath))
            return null;

        var fileName = Path.GetFileName(entry.TargetPath).ToLowerInvariant();
        var pathLower = entry.TargetPath.ToLowerInvariant();

        // Check if file mimics a system file but isn't in a valid system location
        if (MimickedSystemFiles.Contains(fileName))
        {
            var inValidLocation = ValidSystemPaths.Any(p => pathLower.Contains(p));
            if (!inValidLocation)
            {
                return new RiskFactor
                {
                    Name = "Mimicry",
                    Description = $"File '{fileName}' outside its normal system location",
                    Score = 50,
                    Type = RiskFactorType.Negative
                };
            }
        }

        return null;
    }

    private static RiskLevel ScoreToRiskLevel(int score)
    {
        return score switch
        {
            < -20 => RiskLevel.Safe,
            < 10 => RiskLevel.Unknown,
            < 30 => RiskLevel.Suspicious,
            _ => RiskLevel.Critical
        };
    }
}

/// <summary>
/// Risk assessment result for a startup entry.
/// </summary>
public class RiskAssessment
{
    public string EntryId { get; set; } = string.Empty;
    public RiskLevel OriginalRiskLevel { get; set; }
    public RiskLevel RecommendedRiskLevel { get; set; }
    public int TotalScore { get; set; }
    public IReadOnlyList<RiskFactor> Factors { get; set; } = Array.Empty<RiskFactor>();
}

/// <summary>
/// Individual risk factor contributing to the assessment.
/// </summary>
public class RiskFactor
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Score { get; set; }
    public RiskFactorType Type { get; set; }
}

/// <summary>
/// Type of risk factor.
/// </summary>
public enum RiskFactorType
{
    Positive,   // Reduces risk
    Neutral,    // No impact
    Warning,    // Minor concern
    Negative    // Increases risk
}
