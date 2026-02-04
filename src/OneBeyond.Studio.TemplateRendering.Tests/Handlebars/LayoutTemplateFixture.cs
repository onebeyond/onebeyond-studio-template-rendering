using System;
using System.IO;
using HandlebarsDotNet;
using OneBeyond.Studio.TemplateRendering.Handlebars;
using Xunit;

namespace OneBeyond.Studio.TemplateRendering.Tests.Handlebars;

[CollectionDefinition(NAME)]
public static class HandlebarsTemplateRendererCollection
{
    public const string NAME = "Handlebars Exclusive";
}

public sealed class LayoutTemplateFixture : IDisposable
{
    public const string LAYOUT_NAME = "Layout";

    private const string LAYOUT_TEMPLATE = """
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

    internal HandlebarsTemplateRenderer Renderer { get; } = new HandlebarsTemplateRenderer();

    public LayoutTemplateFixture()
    {
        Renderer.RegisterTemplate(LAYOUT_NAME, LAYOUT_TEMPLATE);
    }

    public void Dispose()
    {
        HandlebarsDotNet.Handlebars.RegisterTemplate(
            LAYOUT_NAME,
            (HandlebarsTemplate<TextWriter, object, object>)null!);
    }
}
