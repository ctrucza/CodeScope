using System;
using System.Linq;

namespace CountClasses
{
    public class Project
    {
        private readonly Microsoft.CodeAnalysis.Project project;
        private string name;
        private int referenceCount;
        private Statistics stat;

        public Project(Microsoft.CodeAnalysis.Project project)
        {
            this.project = project;
            Analyze();
        }

        private void Analyze()
        {
            name = project.Name;
            referenceCount = project.AllProjectReferences.Count;

            var documents = project.Documents.ToList();
            stat.documentCount = documents.Count;
            foreach (var document in documents)
            {
                Document d = new Document(document);
                stat = d.Accumulate(stat);
                Console.WriteLine(d);

            }
        }

        public override string ToString()
        {
            return string.Format(
                "project {0}\nreferences {1}\n{2}",
                name,
                referenceCount,
                stat);
        }

        public Statistics Accumulate(Statistics accumulator)
        {
            accumulator.Add(stat);
            return accumulator;
        }
    }
}