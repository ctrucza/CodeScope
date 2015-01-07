using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;

namespace FindingCallersOfMethods
{
    class Program
    {
        static Dictionary<string, SemanticModel> models  = new Dictionary<string, SemanticModel>();
        static Dictionary<string, Compilation> compilations = new Dictionary<string, Compilation>(); 

        static void Main(string[] args)
        {
            var callTree = new Dictionary<string, SortedSet<string>>();
            var inverseCallTree = new Dictionary<string, SortedSet<string>>();
            var referenceMap = new Dictionary<string, SortedSet<string>>();

            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(args[0]).Result;
            Console.Write(solution.Projects.Count());
            foreach (var project in solution.Projects)
            {
                Console.Write(".");
                var compilation = GetCompilation(project);
                foreach (var document in project.Documents)
                {
                    var root = document.GetSyntaxRootAsync().Result;
                    var types = root.DescendantNodes().OfType<TypeDeclarationSyntax>().ToList();
                    foreach (var type in types)
                    {
                        //var semanticModel = compilation.GetSemanticModel(document.GetSyntaxTreeAsync().Result);
                        var semanticModel = GetSemanticModel(project, document);

                        foreach (var method in type.Members.OfType<MethodDeclarationSyntax>())
                        {
                            var methodSymbol = semanticModel.GetDeclaredSymbol(method);
                            var callers = SymbolFinder.FindCallersAsync(methodSymbol, solution).Result;
                            foreach (SymbolCallerInfo caller in callers)
                            {
                                if (!caller.IsDirect)
                                    continue;
                                if (caller.CallingSymbol.ContainingType == caller.CalledSymbol.ContainingType)
                                    continue;

                                var source = GetFullName(caller.CallingSymbol);
                                var target = GetFullName(caller.CalledSymbol);

                                if (!callTree.ContainsKey(source))
                                    callTree.Add(source, new SortedSet<string>());
                                callTree[source].Add(target);

                                if (!inverseCallTree.ContainsKey(target))
                                    inverseCallTree.Add(target, new SortedSet<string>());
                                inverseCallTree[target].Add(source);
                            }
                        }

                        var typeSymbol = semanticModel.GetDeclaredSymbol(type);
                        IEnumerable<ReferencedSymbol> references = SymbolFinder.FindReferencesAsync(typeSymbol, solution).Result;

                        foreach (var referencedSymbol in references)
                        {
                            ISymbol referencedType = GetTypeFor(referencedSymbol.Definition);
                            var documentGroups = referencedSymbol.Locations.GroupBy(l => l.Document).ToList();
                            var projectGroups = documentGroups.GroupBy(d => d.Key.Project);

                            foreach (var p in projectGroups)
                            {
                                Compilation c = GetCompilation(p.Key);
                                foreach (var d in p)
                                {
                                    SemanticModel model = GetSemanticModel(p.Key, d.Key);
                                    foreach (var r in d)
                                    {
                                        ISymbol containingSymbol = GetEnclosingMethodOrPropertyOrField(model, r);
                                        if (containingSymbol == null)
                                            continue;
                                        if (!referenceMap.ContainsKey(referencedType.Name))
                                            referenceMap.Add(referencedType.Name, new SortedSet<string>());
                                        referenceMap[referencedType.Name].Add(containingSymbol.Name);

                                    }
                                }
                            }
                        }


                        //foreach (var reference in references)
                        //{
                        //    ISymbol referredSymbol = reference.Definition;
                        //    foreach (var referenceLocation in reference.Locations)
                        //    {
                        //        Location location = referenceLocation.Location;
                        //          // BAD HERE!The semanticModel is the model of the current file not the file where the reference is
                        //          // We need to do two steps (or semantic model caching).
                        //        ISymbol enclosingSymbol = semanticModel.GetEnclosingSymbol(location.SourceSpan.Start);
                        //        if (enclosingSymbol == typeSymbol)
                        //            continue;
                        //        if (enclosingSymbol.ContainingType == typeSymbol)
                        //            continue;

                        //        Console.WriteLine(enclosingSymbol);
                        //    }
                        //}
                    }
                }
            }

            foreach (var caller in callTree)
            {
                if (caller.Value.Count < 2)
                    continue;
                Console.WriteLine(caller.Key);
                foreach (var callee in caller.Value)
                {
                    Console.WriteLine("\t"+callee);
                }
            }

            Console.WriteLine("===");

            foreach (var caller in inverseCallTree)
            {
                if (caller.Value.Count < 2)
                    continue;
                Console.WriteLine(caller.Key);
                foreach (var callee in caller.Value)
                {
                    Console.WriteLine("\t" + callee);
                }
            }

            Console.WriteLine("***");
            foreach (var referredSymbol in referenceMap)
            {
                Console.WriteLine();
                Console.WriteLine("Class {0} referred by:", referredSymbol.Key);
                foreach (var referringType in referredSymbol.Value)
                {
                    Console.WriteLine("\t" + referringType);
                }

            }

            Console.WriteLine("***");
            foreach (var referredSymbol in referenceMap)
            {
                //Console.WriteLine("Class {0} referred by:", referredSymbol.Key);
                foreach (var referringType in referredSymbol.Value)
                {
                    Console.WriteLine(referredSymbol.Key + "\t" + referringType);
                }
            }
        }

        private static SemanticModel GetSemanticModel(Project project, Document document)
        {
            if (!models.ContainsKey(document.FilePath))
            {
                var compilation = GetCompilation(project);
                models.Add(document.FilePath, compilation.GetSemanticModel(document.GetSyntaxTreeAsync().Result));
            }
            return models[document.FilePath];
        }

        private static Compilation GetCompilation(Project project)
        {
            if (!compilations.ContainsKey(project.Name))
                compilations.Add(project.Name, project.GetCompilationAsync().Result);
            return compilations[project.Name];
        }

        private static string GetFullName(ISymbol symbol)
        {
            return string.Format(
                "{0}.{1}()",
                //symbol.ContainingNamespace,
                symbol.ContainingType,
                symbol.Name);
        }

        private static ISymbol GetEnclosingMethodOrPropertyOrField(
            SemanticModel semanticModel,
            ReferenceLocation reference)
        {
            var enclosingSymbol = semanticModel.GetEnclosingSymbol(reference.Location.SourceSpan.Start);
            return GetTypeFor(enclosingSymbol);


            for (var current = enclosingSymbol; current != null; current = current.ContainingSymbol)
            {
                if (current.Kind == SymbolKind.Field)
                {
                    return current;
                }

                if (current.Kind == SymbolKind.Property)
                {
                    return current;
                }

                if (current.Kind == SymbolKind.Method)
                {
                    var method = (IMethodSymbol)current;
                    if (
                        IsAccessor(method))
                    {
                        return method.AssociatedSymbol;
                    }

                    if (method.MethodKind != MethodKind.AnonymousFunction)
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        private static ISymbol GetTypeFor(ISymbol symbol)
        {
            if (symbol.Kind == SymbolKind.NamedType)
                return symbol;
            else
                return symbol.ContainingType;
        }

        private static bool IsAccessor(IMethodSymbol method)
        {
            return method.MethodKind == MethodKind.PropertyGet || 
                   method.MethodKind == MethodKind.PropertySet || 
                   method.MethodKind == MethodKind.EventAdd ||
                   method.MethodKind == MethodKind.EventRaise ||
                   method.MethodKind == MethodKind.EventRemove;
        }
    }
}
