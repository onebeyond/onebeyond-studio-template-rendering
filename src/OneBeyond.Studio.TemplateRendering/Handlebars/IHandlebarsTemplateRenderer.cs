namespace OneBeyond.Studio.TemplateRendering.Handlebars;

/// <summary>
/// Extends <see cref="ITemplateRenderer"/> with Handlebars-specific template registration.
/// </summary>
public interface IHandlebarsTemplateRenderer : ITemplateRenderer
{
    /// <summary>
    /// Registers a template with the specified name and content.
    /// </summary>
    /// <param name="name">The unique name of the template.</param>
    /// <param name="template">The content of the template.</param>
    public void RegisterTemplate(string name, string template);
}
