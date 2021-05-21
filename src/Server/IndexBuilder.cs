using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server
{
    public class IndexBuilder
    {
        private readonly Index _indexInstance;
        private readonly int _threadsNumber;
        private readonly Regex _regex = new Regex("[^a-zA-Z0-9 -]");

        /// <summary>
        /// Index builder class
        /// </summary>
        /// <param name="threadsNumber">Number of threads which would be used</param>
        public IndexBuilder(int threadsNumber)
        {
            _indexInstance = new Index(threadsNumber);
            _threadsNumber = threadsNumber;
        }

        /// <summary>
        /// Updates current index with files from given directory
        /// </summary>
        /// <param name="directory">Path to the directory</param>
        /// <returns>Built index</returns>
        public Index UpdateFromPath(string directory)
        {
            if (Directory.Exists(directory))
            {
                var files = Directory.EnumerateFiles(directory).ToArray();

                Parallel.ForEach(files,
                    new ParallelOptions {MaxDegreeOfParallelism = _threadsNumber},
                    ProcessFile);
            }

            return _indexInstance;
        }

        /// <summary>
        /// Adds a file to index
        /// </summary>
        /// <param name="fileName">Path to file</param>
        private void ProcessFile(string fileName)
        {
            var position = 0;
            var fileText = File.ReadAllText(fileName);
            var words = _regex.Replace(fileText, "")
                .ToLower()
                .Split(" ");

            foreach (var word in words)
            {
                _indexInstance.Add(word, fileName, position++);
            }
        }
    }
}