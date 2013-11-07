using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class ProjectStatistics
    {
        private readonly IProject project;
        private readonly List<SyntaxWalker> collectors = new List<SyntaxWalker>();

        public ProjectStatistics(IProject project, IEnumerable<SyntaxWalker> collectors )
        {
            this.project = project;
            this.collectors = collectors.ToList();
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

                foreach (SyntaxWalker collector in collectors)
                {
                    collector.Visit(root);
                }
            }
        }
    }
}