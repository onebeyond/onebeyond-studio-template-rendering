using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using OneBeyond.Studio.TemplateRendering.Handlebars;
using VerifyXunit;
using Xunit;

namespace OneBeyond.Studio.TemplateRendering.Tests.Handlebars;

[Collection(HandlebarsTemplateRendererCollection.NAME)]
public sealed class HandlebarsTemplateRendererTests
{
    private readonly HandlebarsTemplateRenderer _renderer = new HandlebarsTemplateRenderer();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Render_NullOrWhitespaceTemplate_ArgumentException(string? input)
    {
        // Arrange
        var parameters = new { };

        // Act
        var action = () => _renderer.Render(input!, parameters);

        // Assert
        action.Should().Throw<ArgumentException>();
    }


    public static TheoryData<string, string, object> GetRenderData()
        => new TheoryData<string, string, object>
        {
            { "Basic", @"Basic template with one {{variable}}", new { variable = "replacement variable!"} },
            { "No", "No variables here.", new { } },
            {
                "Complex",
                """
                <html>
                <body>
                <p>Hello {{userName}},</p>
                <p>You have been sent this email as an invitation to access Alexis' Test. In order to access the system you will first need to set a password. Please click <a href="{{callbackUrl}}">here</a> to set your password.</p>
                <p>If you're having trouble clicking the link, copy and paste the URL below into your web browser: {{callbackUrl}}.</p>
                <p>To log in to your account use the following user name: {{userName}}.</p>
                <p>Best Regards,<br />
                {{systemName}}</p></body></html>
                """,
                new
                {
                    userName = "Alexis",
                    callbackUrl = "https://testserver/resetpassword=xxasjkalsjdalkjasdlkj",
                    systemName = "Test System"
                }
            },
            {
                "Missing",
                """
                <html>
                <body>
                <p>Hello {{userName}},</p>
                <p>You have been sent this email as an invitation to access Alexis' Test. In order to access the system you will first need to set a password. Please click <a href="{{callbackUrl}}">here</a> to set your password.</p>
                <p>If you're having trouble clicking the link, copy and paste the URL below into your web browser: {{callbackUrl}}.</p>
                <p>To log in to your account use the following user name: {{userName}}.</p>
                <p>Best Regards,<br />
                {{systemName}}</p></body></html>
                """,
                new
                {
                    userName = "Alexis",
                    callbackUrl = "https://testserver/resetpassword=xxasjkalsjdalkjasdlkj"
                }
            }
        };

    [Theory]
    [MemberData(nameof(GetRenderData))]
    public Task Render_WithoutLayout_Verify(string testCase, string template, object parameters)
    {
        // Act
        var result = _renderer.Render(template, parameters);

        // Assert
        return Verifier.Verify(result).UseParameters(testCase);
    }
}
