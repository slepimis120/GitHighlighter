using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace ReSharperPlugin.GitHighlighterReSharperPlugin
{
    public class ElementProcessor : IRecursiveElementProcessor
    {
        private readonly List<HighlightingInfo> _highlightings;
        private readonly string _commitMessage;
        private bool _processingFinished;

        public ElementProcessor(List<HighlightingInfo> highlightings, string commitMessage)
        {
            _highlightings = highlightings;
            _commitMessage = commitMessage;
            _processingFinished = false;
        }

        public bool InteriorShouldBeProcessed(ITreeNode element) => !_processingFinished;

        public void ProcessBeforeInterior(ITreeNode element)
        {
            if (_processingFinished) return;

            var firstFiveNonWhitespaceChars = GetFirstFiveNonWhitespaceChars(element);
            if (firstFiveNonWhitespaceChars != DocumentRange.InvalidRange)
            {
                var highlighting = new GitCommitHighlighting(firstFiveNonWhitespaceChars, _commitMessage);
                _highlightings.Add(new HighlightingInfo(firstFiveNonWhitespaceChars, highlighting));
                _processingFinished = true;
            }
        }


        public void ProcessAfterInterior(ITreeNode element) { }

        public bool ProcessingIsFinished => _processingFinished;

        private DocumentRange GetFirstFiveNonWhitespaceChars(ITreeNode node)
        {
            var text = node.GetText();
            int start = 0;
            int count = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsWhiteSpace(text[i]))
                {
                    if (count == 0)
                    {
                        start = i;
                    }
                    count++;
                    if (count == 5)
                    {
                        var range = new DocumentRange(node.GetDocumentRange().Document, new TextRange(start, i + 1));
                        return range;
                    }
                }
            }

            return DocumentRange.InvalidRange;
        }

    }
}