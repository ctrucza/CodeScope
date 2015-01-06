using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CountClasses
{
    public class Document
    {
        private readonly Microsoft.CodeAnalysis.Document document;
        private Statistics stat;
        private string name;

        public Document(Microsoft.CodeAnalysis.Document document)
        {
            this.document = document;
            Analyze();
        }

        private void Analyze()
        {
            name = document.Name;

            var root = document.GetSyntaxRootAsync().Result;
            var classesInDocument = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            stat.documentCount = 1;
            stat.classCount += classesInDocument.Count();
            stat.loc += document.GetTextAsync().Result.Lines.Count;
        }

        public Statistics Accumulate(Statistics statistics)
        {
            statistics.Add(stat);
            return statistics;
        }

        public override string ToString()
        {
            return string.Format(
                "document: {0}\n{1}",
                name,
                stat
                );
        }
    }
}