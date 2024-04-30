using System.Collections;

namespace ProjOb_24L_01180781.Media
{
    public class NewsGenerator : IEnumerable<string?>
    {
        public NewsGenerator(List<IMedia> media, List<IReportable> reportable)
        {
            _media = media;
            _reportable = reportable;
            _iterator = new NewsIterator(this);
        }
        public string? GenerateNextNews()
        {
            _iterator.MoveNext();
            return _iterator.Current;
        }
        public IEnumerator<string?> GetEnumerator()
        {
            while (_iterator.MoveNext())
            {
                yield return _iterator.Current;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<IMedia> _media;
        private List<IReportable> _reportable;
        private NewsIterator _iterator;

        private class NewsIterator : IEnumerator<string?>
        {
            public NewsIterator(NewsGenerator generator)
            {
                _generator = generator;
            }
            public string? Current
            {
                get
                {
                    if (_currentMediaIndex >= _generator._media.Count ||
                        _generator._reportable.Count == 0 ||
                        _generator._media.Count == 0)
                        return null;

                    return _generator._reportable[_currentReportableIndex]
                        .AcceptMediaReport(_generator._media[_currentMediaIndex]);
                }
            }
            object? IEnumerator.Current => Current;
            public bool MoveNext()
            {
                if (_currentReportableIndex >= _generator._reportable.Count - 1)
                {
                    _currentReportableIndex = 0;
                    if (_currentMediaIndex < _generator._media.Count)
                        _currentMediaIndex++;
                }
                else
                {
                    _currentReportableIndex++;
                }
                return true;
            }
            public void Reset()
            {
                _currentMediaIndex = 0;
                _currentReportableIndex = -1;
            }
            public void Dispose() { }

            private int _currentMediaIndex = 0;
            private int _currentReportableIndex = -1;
            private NewsGenerator _generator;
        }
    }
}
