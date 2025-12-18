using System.Runtime.InteropServices;

namespace BootSentry.Security.Interop;

/// <summary>
/// P/Invoke declarations for Windows AMSI (Antimalware Scan Interface) APIs.
/// </summary>
internal static partial class AmsiNativeMethods
{
    /// <summary>
    /// AMSI result indicating the content is clean.
    /// </summary>
    public const int AMSI_RESULT_CLEAN = 0;

    /// <summary>
    /// AMSI result indicating content was not detected as malware.
    /// </summary>
    public const int AMSI_RESULT_NOT_DETECTED = 1;

    /// <summary>
    /// AMSI result indicating content was blocked by administrator policy.
    /// </summary>
    public const int AMSI_RESULT_BLOCKED_BY_ADMIN_START = 0x4000;

    /// <summary>
    /// AMSI result indicating content was blocked by administrator policy (end range).
    /// </summary>
    public const int AMSI_RESULT_BLOCKED_BY_ADMIN_END = 0x4FFF;

    /// <summary>
    /// Threshold above which content is considered malware.
    /// Values >= 32768 (0x8000) indicate malware detected.
    /// </summary>
    public const int AMSI_RESULT_DETECTED_THRESHOLD = 32768;

    /// <summary>
    /// AMSI result indicating malware was detected.
    /// </summary>
    public const int AMSI_RESULT_DETECTED = 32768;

    /// <summary>
    /// S_OK - Operation succeeded.
    /// </summary>
    public const int S_OK = 0;

    /// <summary>
    /// Initializes the AMSI API.
    /// </summary>
    /// <param name="appName">The name of the application calling AMSI.</param>
    /// <param name="amsiContext">Receives the AMSI context handle.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [LibraryImport("amsi.dll", EntryPoint = "AmsiInitialize", StringMarshalling = StringMarshalling.Utf16)]
    public static partial int AmsiInitialize(
        string appName,
        out IntPtr amsiContext);

    /// <summary>
    /// Opens an AMSI scan session.
    /// </summary>
    /// <param name="amsiContext">The AMSI context from AmsiInitialize.</param>
    /// <param name="amsiSession">Receives the session handle.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [LibraryImport("amsi.dll", EntryPoint = "AmsiOpenSession")]
    public static partial int AmsiOpenSession(
        IntPtr amsiContext,
        out IntPtr amsiSession);

    /// <summary>
    /// Scans a buffer for malware.
    /// </summary>
    /// <param name="amsiContext">The AMSI context.</param>
    /// <param name="buffer">Pointer to the buffer to scan.</param>
    /// <param name="length">Length of the buffer in bytes.</param>
    /// <param name="contentName">Name/identifier for the content being scanned.</param>
    /// <param name="amsiSession">The session handle (can be IntPtr.Zero for single scans).</param>
    /// <param name="result">Receives the scan result.</param>
    /// <returns>HRESULT indicating success or failure.</returns>
    [LibraryImport("amsi.dll", EntryPoint = "AmsiScanBuffer", StringMarshalling = StringMarshalling.Utf16)]
    public static partial int AmsiScanBuffer(
        IntPtr amsiContext,
        IntPtr buffer,
        uint length,
        string contentName,
        IntPtr amsiSession,
        out int result);

    /// <summary>
    /// Closes an AMSI session.
    /// </summary>
    /// <param name="amsiContext">The AMSI context.</param>
    /// <param name="amsiSession">The session handle to close.</param>
    [LibraryImport("amsi.dll", EntryPoint = "AmsiCloseSession")]
    public static partial void AmsiCloseSession(
        IntPtr amsiContext,
        IntPtr amsiSession);

    /// <summary>
    /// Uninitializes the AMSI API and releases resources.
    /// </summary>
    /// <param name="amsiContext">The AMSI context to release.</param>
    [LibraryImport("amsi.dll", EntryPoint = "AmsiUninitialize")]
    public static partial void AmsiUninitialize(IntPtr amsiContext);

    /// <summary>
    /// Determines if an AMSI result indicates malware.
    /// </summary>
    /// <param name="result">The AMSI scan result.</param>
    /// <returns>True if the result indicates malware or blocked content.</returns>
    public static bool IsMalware(int result) => result >= AMSI_RESULT_DETECTED_THRESHOLD;

    /// <summary>
    /// Determines if an AMSI result indicates content was blocked by admin policy.
    /// </summary>
    /// <param name="result">The AMSI scan result.</param>
    /// <returns>True if the result indicates admin-blocked content.</returns>
    public static bool IsBlockedByAdmin(int result) =>
        result >= AMSI_RESULT_BLOCKED_BY_ADMIN_START && result <= AMSI_RESULT_BLOCKED_BY_ADMIN_END;
}
