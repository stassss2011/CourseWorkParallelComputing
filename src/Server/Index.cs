using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server
{
    public class Index
    {
        public ConcurrentDictionary<string, ConcurrentQueue<WordInstance>> WordsIndex { get; }

        /// <summary>
        /// Reversed index of text documents
        /// </summary>
        /// <param name="threadsNumber">Number of threads for setting Dictionary concurrency level</param>
        public Index(int threadsNumber)
        {
            WordsIndex = new ConcurrentDictionary<string, ConcurrentQueue<WordInstance>>(
                threadsNumber, 10000);
        }

        /// <summary>
        /// Adds word in the Index
        /// </summary>
        /// <param name="word">Word to be added</param>
        /// <param name="fileName">Path to file</param>
        /// <param name="position">Sequential position of word in the file</param>
        public void Add(string word, string fileName, int position)
        {
            WordsIndex.GetOrAdd(word, _ => new ConcurrentQueue<WordInstance>())
                .Enqueue(new WordInstance(fileName, position));
        }

        /// <summary>
        /// Gets all occurrences of given word 
        /// </summary>
        /// <param name="word">Word to find</param>
        /// <returns>IEnumerable of WordInstance (can be empty, if there is no such word in the Index)</returns>
        public IEnumerable<WordInstance> Get(string word)
        {
            return WordsIndex.GetValueOrDefault(word) ?? new ConcurrentQueue<WordInstance>();
        }
    }
}