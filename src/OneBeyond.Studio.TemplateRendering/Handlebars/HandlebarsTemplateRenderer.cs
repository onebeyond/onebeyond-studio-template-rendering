using System;
using System.Threading;

namespace OneBeyond.Studio.TemplateRendering.Handlebars;

/// <summary>
/// Renders a template written in mustache {{}} syntax using the <see langword="static"/> instance of <see cref="HandlebarsDotNet"/>.
/// Templates registered prior via <see cref="RegisterTemplate"/> will be applied during rendering.
/// </summary>
/// <remarks>
/// Template registration is thread-safe and mutually exclusive with rendering operations,
/// while multiple renderings can occur concurrently when no registration is in progress.
/// </remarks>
internal sealed class HandlebarsTemplateRenderer : IHandlebarsTemplateRenderer
{
    private static readonly ReaderWriterLockSlim Lock = new();

    /// <inheritdoc/>
    /// <remarks>Registration is performed in a thread-safe manner.</remarks>
    public void RegisterTemplate(string name, string template)
    {
        Lock.EnterWriteLock();
        try
        {
            HandlebarsDotNet.Handlebars.RegisterTemplate(name, template);
        }
        finally
        {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc/>
    public string Render(string template, object? parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(template);

        Lock.EnterReadLock();
        try
        {
            var compiledTemplate = HandlebarsDotNet.Handlebars.Compile(template);
            return compiledTemplate(parameters);
        }
        finally
        {
            Lock.ExitReadLock();
        }
    }
}
