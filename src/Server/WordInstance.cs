namespace Server
{
    public struct WordInstance
    {
        public string FileName { get; }
        public int Position { get; }

        /// <summary>
        /// Representation of word position
        /// </summary>
        /// <param name="fileName">Path to file</param>
        /// <param name="position">Sequential position of word in the file</param>
        public WordInstance(string fileName, int position)
        {
            FileName = fileName;
            Position = position;
        }

        public override string ToString()
        {
            return $"File name: {FileName}, position: {Position}";
        }
    }
}