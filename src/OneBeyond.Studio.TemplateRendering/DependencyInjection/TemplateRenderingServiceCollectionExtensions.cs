using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OneBeyond.Studio.TemplateRendering.Handlebars;

namespace OneBeyond.Studio.TemplateRendering.DependencyInjection;

/// <summary>
/// Extension methods for registering Handlebars-based template renderers.
/// </summary>
public static class TemplateRenderingServiceCollectionExtensions
{
    /// <summary>
    /// Adds Handlebars-based services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>Reference to the <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddHandlebarsTemplateRenderer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<ITemplateRenderer, HandlebarsTemplateRenderer>();
        return services;
    }
}
