using System;
using System.Diagnostics;
using System.IO;

[SetUpFixture]
public class TestHostFixture
{
    private static Process? _webAppProcess;

    public static readonly string BaseUrl = "https://localhost:51877/";
    public static readonly string MessagePath = "message";

    public static string MessageUrl => BaseUrl + MessagePath;

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        var projectPath = GetUiProjectPath();

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{projectPath}\" --urls {BaseUrl}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetDirectoryName(projectPath) ?? Environment.CurrentDirectory
        };

        _webAppProcess = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start web app process.");

        WaitForAppToBeReady().GetAwaiter().GetResult();
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
        if (_webAppProcess == null)
        {
            return;
        }

        try
        {
            if (!_webAppProcess.HasExited)
            {
                _webAppProcess.Kill(true);
            }
        }
        catch
        {
        }
        finally
        {
            _webAppProcess.Dispose();
            _webAppProcess = null;
        }
    }

    private static string GetUiProjectPath()
    {
        var baseDir = AppContext.BaseDirectory;
        var root = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
        var projectPath = Path.Combine(root, "src", "Morsley.UK.DarkBootstrap.UI", "Morsley.UK.DarkBootstrap.UI.csproj");
        return projectPath;
    }

    private static async Task WaitForAppToBeReady()
    {
        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(2)
        };

        var maxAttempts = 30;

        for (var i = 0; i < maxAttempts; i++)
        {
            try
            {
                var response = await client.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
            }

            await Task.Delay(1000);
        }

        throw new TimeoutException($"The web app at {BaseUrl} did not become ready in time.");
    }
}
