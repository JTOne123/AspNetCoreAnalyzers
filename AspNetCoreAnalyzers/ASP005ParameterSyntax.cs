namespace AspNetCoreAnalyzers
{
    using Microsoft.CodeAnalysis;

    public static class ASP005ParameterSyntax
    {
        public const string DiagnosticId = "ASP005";

        public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: "Syntax error in parameter.",
            messageFormat: "Syntax error in parameter.",
            category: AnalyzerCategory.Routing,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Syntax error in parameter.",
            helpLinkUri: HelpLink.ForId(DiagnosticId));
    }
}
