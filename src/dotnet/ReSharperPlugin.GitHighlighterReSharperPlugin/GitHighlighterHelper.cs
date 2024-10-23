using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

public class GitHighlighterHelper
{
    private readonly string _solutionRoot;

    public GitHighlighterHelper(string solutionRoot)
    {
        _solutionRoot = solutionRoot;
    }

    public bool IsGitRepository()
    {
        return Repository.IsValid(_solutionRoot);
    }

    public List<Commit> GetRecentCommits(int numberOfCommits)
    {
        using (var repo = new Repository(_solutionRoot))
        {
            return repo.Commits.Take(numberOfCommits).ToList();
        }
    }
}