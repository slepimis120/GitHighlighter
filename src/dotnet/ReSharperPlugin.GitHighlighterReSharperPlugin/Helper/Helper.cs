using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    public class Helper
    {
        private readonly string _repositoryPath;
        private List<string> _cachedCommits;
        private int _cachedNumberOfCommits;

        public Helper(string repositoryPath)
        {
            _repositoryPath = repositoryPath;
        }

        public bool IsGitRepository()
        {
            return RunGitCommand("rev-parse --is-inside-work-tree") == "true";
        }

        public List<string> GetRecentCommits(int numberOfCommits)
        {
            if (_cachedCommits == null || _cachedNumberOfCommits != numberOfCommits)
            {
                var result = RunGitCommand($"log -n {numberOfCommits} --pretty=format:%H");
                _cachedCommits = result.Split('\n').ToList();
                _cachedNumberOfCommits = numberOfCommits;
            }
            return _cachedCommits;
        }

        private string RunGitCommand(string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                WorkingDirectory = _repositoryPath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd().Trim();
            }
        }
        
        public List<string> GetModifiedFiles(string commitHash)
        {
            var result = RunGitCommand($"diff-tree --no-commit-id --name-only -r {commitHash}");
            var files = result.Split('\n').ToList();
            var fullPaths = files.Select(file => Path.GetFullPath(Path.Combine(_repositoryPath, file))).ToList();
            
            return fullPaths;
        }
    }
}