using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;

namespace Eldritch.Core.Tests
{
    public class CliE2eTests
    {
        [Fact]
        public void Cli_NonInteractive_RenderOnce_ExitsWithMapOutput()
        {
            var repoDir = Directory.GetCurrentDirectory();
            while (repoDir != null && !File.Exists(Path.Combine(repoDir, "EldritchDungeon.sln")))
            {
                var parent = Directory.GetParent(repoDir);
                if (parent == null) break;
                repoDir = parent.FullName;
            }

            var psi = new ProcessStartInfo("dotnet")
            {
                Arguments = "run --project src/Eldritch.Cli -- --race=Human --class=Warrior --non-interactive --play --render-once --seed=42 --map-width=20 --map-height=10 --viewport-width=10 --viewport-height=5",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = repoDir ?? Directory.GetCurrentDirectory()
            };

            using var proc = Process.Start(psi)!;
            Assert.NotNull(proc);

            var output = new StringBuilder();
            var error = new StringBuilder();
            proc.OutputDataReceived += (s, e) => { if (e.Data != null) output.AppendLine(e.Data); };
            proc.ErrorDataReceived += (s, e) => { if (e.Data != null) error.AppendLine(e.Data); };
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            bool exited = proc.WaitForExit(5000);
            if (!exited)
            {
                try { proc.Kill(); } catch {}
                Assert.True(false, "CLI did not exit within timeout");
            }

            var outStr = output.ToString();
            var errStr = error.ToString();
            Assert.True(proc.HasExited, "CLI process should have exited");
            Assert.Contains("HP:", outStr);
            Assert.True(outStr.Contains('.') || outStr.Contains('#'), "Output should contain map glyphs. StdErr: " + errStr);
        }
    }
}
