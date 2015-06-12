using System;
using System.Collections.Generic;
using System.Linq;

namespace CountClasses
{
    public class Project
    {
        private readonly Microsoft.CodeAnalysis.Project project;
        private string name;
        private int referenceCount;
        private Statistics stat;
        private readonly List<Document> documents = new List<Document>(); 

        public Project(Microsoft.CodeAnalysis.Project project)
        {
            this.project = project;
            project.GetCompilationAsync();
            Analyze();
        }

        private void Analyze()
        {
            name = project.Name;
            referenceCount = project.AllProjectReferences.Count;

            var documentList  = project.Documents.ToList();

            stat.documentCount = documentList.Count;
            foreach (var document in documentList)
            {
                Document d = new Document(document);
                stat = d.Accumulate(stat);
                Console.WriteLine(d);
                documents.Add(d);
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

        public void Inspect(Microsoft.CodeAnalysis.Solution solution)
        {
            var compilation = project.GetCompilationAsync().Result;
            foreach (var document in documents)
            {
                document.Inspect(solution, compilation);
            }
        }
    }
}