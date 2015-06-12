using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CountClasses
{
    public class Document
    {
        private readonly Microsoft.CodeAnalysis.Document document;
        private Statistics stat;
        private string name;
        private List<ClassDeclarationSyntax> classes = new List<ClassDeclarationSyntax>();
        private SemanticModel semanticModel;
        private SyntaxTree syntaxTree;

        public Document(Microsoft.CodeAnalysis.Document document)
        {
            this.document = document;
            Analyze();
        }

        private void Analyze()
        {
            name = document.Name;
            semanticModel = document.GetSemanticModelAsync().Result;
            syntaxTree = document.GetSyntaxTreeAsync().Result;

            var root = document.GetSyntaxRootAsync().Result;
            var classesInDocument = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
            classes.AddRange(classesInDocument);

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

        public void Inspect(Microsoft.CodeAnalysis.Solution solution, Compilation compilation)
        {
            foreach (var classDeclarationSyntax in classes)
            {
                Console.WriteLine("inspecting class " + classDeclarationSyntax.Identifier);
                var symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
                IEnumerable<ReferencedSymbol> references = SymbolFinder.FindReferencesAsync(symbol, solution).Result.ToList();
                Console.WriteLine("found {0} references", references.Count());
                foreach (ReferencedSymbol referencedSymbol in references)
                {
                    Console.WriteLine("referenced by {0} ({1})", referencedSymbol.Definition.Name, referencedSymbol.Definition.Kind);
                    var documentGroups = referencedSymbol.Locations.GroupBy(l => l.Document).ToList();
                    var projectGroups = documentGroups.GroupBy(d => d.Key.Project);
                    foreach (var projectGroup in projectGroups)
                    {
                        var project = projectGroup.Key;
                        var c = project.GetCompilationAsync().Result;
                        foreach (var documentGroup in documentGroups)
                        {
                            var doc = documentGroup.Key;
                            Console.WriteLine("in document " + doc.Name);
                            var sem = doc.GetSemanticModelAsync().Result;

                            foreach (var referenceLocation in referencedSymbol.Locations)
                            {
                                if (referenceLocation.IsImplicit)
                                    continue;
                                var referencingSymbol = sem.GetEnclosingSymbol(referenceLocation.Location.SourceSpan.Start);
                                Console.WriteLine("referencing symbol: {0} ({1})", referencingSymbol.Name, referencingSymbol.Kind);
                                //Console.WriteLine(referencingSymbol.Name);
                            }
                        }
                    }
                }

                /*
                var documentGroups = referenceLocations.GroupBy(loc => loc.Document);
                var projectGroups = documentGroups.GroupBy(g => g.Key.Project);
                var result = new Dictionary<ISymbol, List<Location>>();

                foreach (var projectGroup in projectGroups)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var project = projectGroup.Key;
                    var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);

                    foreach (var documentGroup in projectGroup)
                    {
                        var document = documentGroup.Key;
                        await AddSymbolsAsync(document, documentGroup, result, cancellationToken).ConfigureAwait(false);
                    }

                    GC.KeepAlive(compilation);
                }

                return result;
                */

                //foreach (var referencedSymbol in references)
                //{
                //    Console.WriteLine(classDeclarationSyntax.Identifier + "===>" + referencedSymbol.Definition.Name);
                //    foreach (var referenceLocation in referencedSymbol.Locations)
                //    {
                //        referenceLocation.
                //    }
                //}
            }
        }
    }
}