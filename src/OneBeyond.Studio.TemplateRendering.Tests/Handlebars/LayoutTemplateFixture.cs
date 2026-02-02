using System;
using System.IO;
using HandlebarsDotNet;
using OneBeyond.Studio.TemplateRendering.Handlebars;
using Xunit;

namespace OneBeyond.Studio.TemplateRendering.Tests.Handlebars;

[CollectionDefinition(Name)]
public static class HandlebarsTemplateRendererCollection
{
    public const string Name = "Handlebars Exclusive";
}

public sealed class LayoutTemplateFixture : IDisposable
{
    public const string LayoutName = "Layout";

    private const string LayoutTemplate = """
        <html>
        <body>
        {{{> @partial-block }}}
        <footer>
        <p>Best Regards,<br />
        {{systemName}}</p>
        </footer>
        </body>
        </html>
        """;

    public HandlebarsTemplateRenderer Renderer { get; } = new HandlebarsTemplateRenderer();

    public LayoutTemplateFixture()
    {
        Renderer.RegisterTemplate(LayoutName, LayoutTemplate);
    }

    public void Dispose()
    {
        HandlebarsDotNet.Handlebars.RegisterTemplate(
            LayoutName,
            (HandlebarsTemplate<TextWriter, object, object>)null!);
    }
}
