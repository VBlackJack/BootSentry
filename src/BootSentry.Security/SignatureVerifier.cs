using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using BootSentry.Core.Enums;
using BootSentry.Core.Interfaces;

namespace BootSentry.Security;

/// <summary>
/// Verifies Authenticode signatures using WinVerifyTrust API.
/// </summary>
public sealed class SignatureVerifier : ISignatureVerifier
{
    /// <inheritdoc/>
    public async Task<SignatureInfo> VerifyAsync(string filePath, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => VerifySignature(filePath), cancellationToken);
    }

    /// <inheritdoc/>
    public bool IsSigned(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        try
        {
            var result = VerifyWithWinTrust(filePath);
            return result != NativeMethods.TRUST_E_NOSIGNATURE;
        }
        catch
        {
            return false;
        }
    }

    private SignatureInfo VerifySignature(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new SignatureInfo
            {
                Status = SignatureStatus.Unknown,
                ErrorMessage = "File not found"
            };
        }

        try
        {
            var trustResult = VerifyWithWinTrust(filePath);
            var status = MapTrustResult(trustResult);

            // Get certificate details if signed
            if (status != SignatureStatus.Unsigned)
            {
                var certInfo = GetCertificateInfo(filePath);
                return certInfo with { Status = status };
            }

            return new SignatureInfo
            {
                Status = status,
                ErrorMessage = status == SignatureStatus.Unsigned ? null : $"Trust error: 0x{trustResult:X8}"
            };
        }
        catch (Exception ex)
        {
            return new SignatureInfo
            {
                Status = SignatureStatus.Unknown,
                ErrorMessage = ex.Message
            };
        }
    }

    private static int VerifyWithWinTrust(string filePath)
    {
        var filePathPtr = Marshal.StringToHGlobalUni(filePath);
        try
        {
            var fileInfo = new NativeMethods.WINTRUST_FILE_INFO
            {
                cbStruct = (uint)Marshal.SizeOf<NativeMethods.WINTRUST_FILE_INFO>(),
                pcwszFilePath = filePathPtr,
                hFile = IntPtr.Zero,
                pgKnownSubject = IntPtr.Zero
            };

            var fileInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.WINTRUST_FILE_INFO>());
            try
            {
                Marshal.StructureToPtr(fileInfo, fileInfoPtr, false);

                var trustData = new NativeMethods.WINTRUST_DATA
                {
                    cbStruct = (uint)Marshal.SizeOf<NativeMethods.WINTRUST_DATA>(),
                    pPolicyCallbackData = IntPtr.Zero,
                    pSIPClientData = IntPtr.Zero,
                    dwUIChoice = NativeMethods.WTD_UI_NONE,
                    fdwRevocationChecks = NativeMethods.WTD_REVOKE_NONE,
                    dwUnionChoice = NativeMethods.WTD_CHOICE_FILE,
                    pFile = fileInfoPtr,
                    dwStateAction = NativeMethods.WTD_STATEACTION_VERIFY,
                    hWVTStateData = IntPtr.Zero,
                    pwszURLReference = IntPtr.Zero,
                    dwProvFlags = 0,
                    dwUIContext = 0,
                    pSignatureSettings = IntPtr.Zero
                };

                var actionId = NativeMethods.WINTRUST_ACTION_GENERIC_VERIFY_V2;
                var result = NativeMethods.WinVerifyTrust(IntPtr.Zero, ref actionId, ref trustData);

                // Close the trust state
                trustData.dwStateAction = NativeMethods.WTD_STATEACTION_CLOSE;
                NativeMethods.WinVerifyTrust(IntPtr.Zero, ref actionId, ref trustData);

                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(fileInfoPtr);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(filePathPtr);
        }
    }

    private static SignatureStatus MapTrustResult(int result)
    {
        return result switch
        {
            0 => SignatureStatus.SignedTrusted,
            NativeMethods.TRUST_E_NOSIGNATURE => SignatureStatus.Unsigned,
            NativeMethods.TRUST_E_EXPLICIT_DISTRUST => SignatureStatus.SignedUntrusted,
            NativeMethods.TRUST_E_SUBJECT_NOT_TRUSTED => SignatureStatus.SignedUntrusted,
            NativeMethods.CRYPT_E_SECURITY_SETTINGS => SignatureStatus.SignedUntrusted,
            _ => SignatureStatus.SignedUntrusted
        };
    }

    private static SignatureInfo GetCertificateInfo(string filePath)
    {
        try
        {
            var baseCert = X509Certificate.CreateFromSignedFile(filePath);
            using var cert = new X509Certificate2(baseCert);

            return new SignatureInfo
            {
                Status = SignatureStatus.Unknown, // Will be overwritten
                SignerName = cert.GetNameInfo(X509NameType.SimpleName, false),
                Issuer = cert.GetNameInfo(X509NameType.SimpleName, true),
                SerialNumber = cert.SerialNumber,
                Thumbprint = cert.Thumbprint,
                ValidFrom = cert.NotBefore,
                ValidTo = cert.NotAfter,
                ChainIsValid = ValidateCertificateChain(cert)
            };
        }
        catch
        {
            return new SignatureInfo
            {
                Status = SignatureStatus.Unknown,
                ErrorMessage = "Could not read certificate"
            };
        }
    }

    private static bool ValidateCertificateChain(X509Certificate2 cert)
    {
        using var chain = new X509Chain();
        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

        try
        {
            return chain.Build(cert);
        }
        catch
        {
            return false;
        }
    }
}
