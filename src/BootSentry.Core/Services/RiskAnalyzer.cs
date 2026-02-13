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
    private readonly HeuristicAnalyzer _heuristicAnalyzer;

    public RiskAnalyzer()
    {
        _heuristicAnalyzer = new HeuristicAnalyzer();
    }

    /// <summary>
    /// List of known trusted publishers.
    /// </summary>
    private static readonly HashSet<string> TrustedPublishers = Constants.Security.TrustedPublishers;

    /// <summary>
    /// Suspicious file locations.
    /// </summary>
    private static readonly string[] SuspiciousLocations = Constants.Paths.SuspiciousLocations;

    /// <summary>
    /// System-mimicking filenames often used by malware.
    /// </summary>
    private static readonly string[] MimickedSystemFiles = Constants.Security.MimickedSystemFiles;

    /// <summary>
    /// Valid system paths for mimicked files.
    /// </summary>
    private static readonly string[] ValidSystemPaths = Constants.Paths.ValidSystemPaths;

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
        entry.RiskFactors = assessment.Factors;
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
                Score = Constants.Security.RiskScores.SignedTrusted,
                Type = RiskFactorType.Positive
            },
            SignatureStatus.SignedUntrusted => new RiskFactor
            {
                Name = "Signature",
                Description = "Signed but certificate not trusted",
                Score = Constants.Security.RiskScores.SignedUntrusted,
                Type = RiskFactorType.Warning
            },
            SignatureStatus.Unsigned => new RiskFactor
            {
                Name = "Signature",
                Description = "Unsigned file",
                Score = Constants.Security.RiskScores.Unsigned,
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
                Score = Constants.Security.RiskScores.UnknownPublisher,
                Type = RiskFactorType.Warning
            };
        }

        if (TrustedPublishers.Contains(entry.Publisher))
        {
            return new RiskFactor
            {
                Name = "Publisher",
                Description = $"Trusted publisher: {entry.Publisher}",
                Score = Constants.Security.RiskScores.TrustedPublisher,
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
                    Score = Constants.Security.RiskScores.SuspiciousLocation,
                    Type = RiskFactorType.Negative
                };
            }
        }

        // Standard locations are good
        if (Constants.Paths.TrustedLocations.Any(loc => pathLower.Contains(loc)))
        {
            return new RiskFactor
            {
                Name = "Location",
                Description = "File in standard location",
                Score = Constants.Security.RiskScores.StandardLocation,
                Type = RiskFactorType.Positive
            };
        }

        return null;
    }

    private RiskFactor? AnalyzeCommandLine(StartupEntry entry)
    {
        if (string.IsNullOrEmpty(entry.CommandLineRaw))
            return null;

        var matches = _heuristicAnalyzer.Analyze(entry.CommandLineRaw);
        if (matches.Count > 0)
        {
            // Take the highest score match or sum them? Let's take the highest impact.
            var worstMatch = matches.OrderByDescending(m => m.Score).First();
            
            return new RiskFactor
            {
                Name = "Heuristics",
                Description = $"{worstMatch.Name}: {worstMatch.Description}",
                Score = worstMatch.Score, // Heuristic scores are positive (bad)
                Type = RiskFactorType.Negative
            };
        }

        if (Parsing.CommandLineParser.IsSuspiciousCommandLine(entry.CommandLineRaw))
        {
            return new RiskFactor
            {
                Name = "Command Line",
                Description = "Command contains suspicious patterns (encoding, download, etc.)",
                Score = Constants.Security.RiskScores.SuspiciousCommandLine,
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
                Score = Constants.Security.RiskScores.FileNotFound,
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
                Score = Constants.Security.RiskScores.IFEOEntryType,
                Type = RiskFactorType.Warning
            },
            EntryType.Winlogon => new RiskFactor
            {
                Name = "Type",
                Description = "Winlogon entry - sensitive system area",
                Score = Constants.Security.RiskScores.WinlogonEntryType,
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
                    Score = Constants.Security.RiskScores.NameMimicking,
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
            < Constants.Security.RiskThresholds.Safe => RiskLevel.Safe,
            < Constants.Security.RiskThresholds.Unknown => RiskLevel.Unknown,
            < Constants.Security.RiskThresholds.Suspicious => RiskLevel.Suspicious,
            _ => RiskLevel.Critical
        };
    }
}
