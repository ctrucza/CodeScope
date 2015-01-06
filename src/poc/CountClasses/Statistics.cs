namespace CountClasses
{
    public struct Statistics
    {
        public int documentCount;
        public int classCount;
        public int loc;

        public void Add(Statistics stat)
        {
            documentCount += stat.documentCount;
            classCount += stat.classCount;
            loc += stat.loc;
        }

        public override string ToString()
        {
            return string.Format(
                "documents: {0}\nclasses: {1}\nloc: {2}",
                documentCount,
                classCount,
                loc);
        }
    }
}