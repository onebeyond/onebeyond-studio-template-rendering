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
public class HandlebarsTemplateRenderer : IHandlebarsTemplateRenderer
{
    private static readonly ReaderWriterLockSlim _lock = new();

    /// <inheritdoc/>
    /// <remarks>Registration is performed in a thread-safe manner.</remarks>
    public void RegisterTemplate(string name, string template)
    {
        _lock.EnterWriteLock();
        try
        {
            HandlebarsDotNet.Handlebars.RegisterTemplate(name, template);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc/>
    public string Render(string template, object? parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(template);

        _lock.EnterReadLock();
        try
        {
            var compiledTemplate = HandlebarsDotNet.Handlebars.Compile(template);
            return compiledTemplate(parameters);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}
