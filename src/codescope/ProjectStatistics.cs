using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace codescope
{
    class ProjectStatistics
    {
        private readonly IProject project;
        private readonly ClassCollector classCollector;

        public ProjectStatistics(IProject project)
        {
            this.project = project;
            classCollector = new ClassCollector();
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

                classCollector.Visit(root);
            }
        }

        public int ClassCount()
        {
            return classCollector.Classes.Count;
        }
    }
}