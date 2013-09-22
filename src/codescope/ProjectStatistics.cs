using System.Collections.Generic;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class ProjectStatistics
    {
        private readonly IProject project;
        private List<SyntaxWalker> walkers = new List<SyntaxWalker>();

        public ProjectStatistics(IProject project)
        {
            this.project = project;
        }

        public void AddWalker(SyntaxWalker walker)
        {
            walkers.Add(walker);
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

                foreach (SyntaxWalker walker in walkers)
                {
                    walker.Visit(root);
                }
            }
        }

        public void AddWalkers(IEnumerable<CommonCollector> walkerList)
        {
            walkers.AddRange(walkerList);
        }
    }
}