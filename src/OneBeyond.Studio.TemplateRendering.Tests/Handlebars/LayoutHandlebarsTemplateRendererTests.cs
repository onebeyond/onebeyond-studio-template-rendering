using System.Threading.Tasks;
using AwesomeAssertions;
using OneBeyond.Studio.TemplateRendering.Handlebars;
using VerifyXunit;
using Xunit;

namespace OneBeyond.Studio.TemplateRendering.Tests.Handlebars;

[Collection(HandlebarsTemplateRendererCollection.Name)]
public class LayoutHandlebarsTemplateRendererTests : IClassFixture<LayoutTemplateFixture>
{
    private readonly HandlebarsTemplateRenderer _renderer;

    public LayoutHandlebarsTemplateRendererTests(LayoutTemplateFixture fixture)
    {
        _renderer = fixture.Renderer;
    }

    [Fact]
    public void RegisterTemplate_Always_OverridePrevious()
    {
        // Arrange
        var templateName = "greeting";

        var firstLayout = "Hello, {{name}}! {{{> @partial-block }}}";
        _renderer.RegisterTemplate(templateName, firstLayout);

        var second = "Hi, {{name}}! {{{> @partial-block }}}";
        _renderer.RegisterTemplate(templateName, second);

        var renderTemplate = $"{{{{#> {templateName}}}}}How are you?{{{{/{templateName}}}}}";
        var parameters = new { Name = "Alexis" };

        // Act
        var result = _renderer.Render(renderTemplate, parameters);

        // Assert
        result.Should().Be($"Hi, {parameters.Name}! How are you?");
    }

    public static TheoryData<string, string, object> GetRenderData()
        => new TheoryData<string, string, object>
        {
            {
                "Basic",
                $$$"""
                {{#> {{{LayoutTemplateFixture.LayoutName}}}}}
                Basic template with one {{variable}}
                {{/{{{LayoutTemplateFixture.LayoutName}}}}}
                """,
                new
                {
                    variable = "replacement!",
                    systemName = "Test System"
                }
            },
            {
                "Complex",
                $$$$"""
                {{#> {{{{LayoutTemplateFixture.LayoutName}}}}}}
                <p>Hello {{userName}},</p>
                <p>You have been sent this email as an invitation to access Alexis' Test. In order to access the system you will first need to set a password. Please click <a href="{{callbackUrl}}">here</a> to set your password.</p>
                <p>If you're having trouble clicking the link, copy and paste the URL below into your web browser: {{callbackUrl}}.</p>
                <p>To log in to your account use the following user name: {{userName}}.</p>
                {{/{{{{LayoutTemplateFixture.LayoutName}}}}}}
                """,
                new
                {
                    userName = "Alexis",
                    callbackUrl = "https://testserver/resetpassword=xxasjkalsjdalkjasdlkj",
                    systemName = "Test System"
                }
            }
        };

    [Theory]
    [MemberData(nameof(GetRenderData))]
    public Task Render_WithLayout_Verify(string testCase, string template, object parameters)
    {
        // Act
        var result = _renderer.Render(template, parameters);

        // Assert
        return Verifier.Verify(result).UseParameters(testCase);
    }
}
