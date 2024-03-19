using Bootsharp;
using Bootsharp.Inject;
using FilesToXml.Wasm;
using Microsoft.Extensions.DependencyInjection;

// Group all generated JavaScript APIs under "Converter" namespace.
[assembly: JSPreferences(Space = [".+", "Converter"])]
// Generate C# -> JavaScript interop handlers for specified contracts.
[assembly: JSExport(typeof(IConverter))]
// Perform dependency injection.
new ServiceCollection()
    .AddSingleton<IConverter, Converter>()
    .AddBootsharp() // inject generated interop handlers
    .BuildServiceProvider()
    .RunBootsharp(); // initialize interop services
Console.WriteLine($".NET {Environment.Version} ready");