using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace RawBot.Runtime.Scripts
{
    public static class Compiler
    {
        private static readonly List<MetadataReference> MetadataReferences = new();

        static Compiler()
        {
            MetadataReferences.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(a => MetadataReference.CreateFromFile(a.Location)).ToList());
        }

        public static (Assembly, AssemblyLoadContext) Compile(string source)
        {
            var code = SourceText.From(source);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);
            var tree = SyntaxFactory.ParseSyntaxTree(code, options);
            var compilation = CSharpCompilation.Create($"script-{source.GetHashCode()}.dll",
                new[] { tree },
                MetadataReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release));

            using var stream = new MemoryStream();
            var result = compilation.Emit(stream);
            if (result.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var context = new CollectibleAssemblyLoadContext();
                var assembly = context.LoadFromStream(stream);
                return (assembly, context);
            }

            throw new CompilationException(result.Diagnostics.Select(d => d.GetMessage()), result.Diagnostics.Length);
        }
    }
}
