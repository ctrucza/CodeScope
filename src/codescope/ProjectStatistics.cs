using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class ProjectStatistics
    {
        private readonly IProject project;
        private readonly List<CommonCollector> collectors = new List<CommonCollector>();

        public ProjectStatistics(IProject project)
        {
            this.project = project;
        }

        public void CollectStatistics()
        {
            foreach (IDocument document in project.Documents)
            {
                // Here we could use 
                // SyntaxTree tree= document.GetSyntaxTree() as SyntaxTree;
                // but GetSyntaxTree creates CommonSyntaxTree (which is in the CSharp namespace)

                SyntaxTree tree = SyntaxTree.ParseFile(document.FilePath);
                SyntaxNode root = tree.GetRoot();

                foreach (CommonCollector collector in collectors)
                {
                    collector.Visit(root);
                }
            }
        }

        public void AddCollectors(IEnumerable<CommonCollector> collectorList)
        {
            collectors.AddRange(collectorList);
        }
    }
}