using System.Threading;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ReSharperPlugin.GitHighlighterReSharperPlugin.Tests
{
    [ZoneDefinition]
    public class GitHighlighterReSharperPluginTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<IGitHighlighterReSharperPluginZone> { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<GitHighlighterReSharperPluginTestEnvironmentZone> { }

    [SetUpFixture]
    public class GitHighlighterReSharperPluginTestsAssembly : ExtensionTestEnvironmentAssembly<GitHighlighterReSharperPluginTestEnvironmentZone> { }
}
