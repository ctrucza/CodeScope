using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace SemanticAnalysis
{
    class Program
    {
        private const string solutionFilePath = @"..\..\..\sample\sample.sln";
        static void Main(string[] args)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionFilePath).Result;
            foreach (var project in solution.Projects)
            {
                ProcessProject2(project);
            }
        }

        private static void ProcessProject2(Project project)
        {
            foreach (var document in project.Documents)
            {
                var root = document.GetSyntaxRootAsync().Result;

                foreach (var method in root.DescendantNodes().OfType<BaseMethodDeclarationSyntax>())
                {
                    dynamic x = method;
                    Console.WriteLine(x.ToString());
                    foreach (var parameter in method.ParameterList.Parameters)
                    {
                        Console.WriteLine(parameter.Type);
                    }
                    ProcessMethod(method.Body);
                }

                foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
                {
                    Console.WriteLine(method.ReturnType);
                }


                foreach (var property in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                {
                    Console.WriteLine(property.Identifier);
                    Console.WriteLine(property.Type);
                }

                foreach (var field in root.DescendantNodes().OfType<FieldDeclarationSyntax>())
                {
                    foreach (var variable in field.Declaration.Variables)
                    {
                        Console.WriteLine(variable.Identifier);
                        Console.WriteLine(field.Declaration.Type);
                    }
                }
            }
        }

        private static void ProcessMethod(BlockSyntax body)
        {
            foreach (var statements in body.Statements)
            {
                //BlockSyntax;
                //LocalDeclarationStatementSyntax;
                //VariableDeclarationSyntax;
                //ExpressionStatementSyntax;
                //ReturnStatementSyntax;
                //ThrowStatementSyntax;
                //YieldStatementSyntax;
                //WhileStatementSyntax;
                //ForStatementSyntax;
                //ForEachStatementSyntax;
                //DoStatementSyntax;
                //UsingStatementSyntax;
                //IfStatementSyntax;
                //ElseClauseSyntax;
                //SwitchStatementSyntax;
                //CatchClauseSyntax;

                //{
                    
                //}
            }
        }

        private static void ProcessProject(Project project)
        {
            Compilation compilation = project.GetCompilationAsync().Result;
            foreach (var document in project.Documents)
            {
                var syntaxTree = document.GetSyntaxTreeAsync().Result;
                var semanticModel = compilation.GetSemanticModel(syntaxTree);

                var root = syntaxTree.GetRoot();
                foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    ProcessClass(semanticModel, classDeclaration);
                }
            }
        }

        private static void ProcessClass(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration)
        {
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

            // inherited from
            Console.WriteLine(classSymbol.Name + ":" + classSymbol.BaseType.Name + "("+classSymbol.BaseType.ContainingNamespace+")");

            //// constructors
            //foreach (var constructor in classSymbol.Constructors)
            //{
            //    Console.WriteLine("ctor: " + constructor.Name);
            //    ProcesParameters(constructor);
            //}

            // methods
            foreach (var method in classSymbol.GetMembers().OfType<IMethodSymbol>())
            {
                Console.WriteLine(method.Name);
                ProcesParameters(method);
                Console.WriteLine(method.ReturnType.Name);
                
            }

            foreach (var member in classSymbol.GetTypeMembers())
            {
                Console.WriteLine(member.Name);
            }
        }

        private static void ProcesParameters(IMethodSymbol method)
        {
            foreach (var parameter in method.Parameters)
            {
                Console.WriteLine("param: " + parameter.Type.Name + "(" + parameter.Type.ContainingNamespace + ")");
            }
        }
    }
}
