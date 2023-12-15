using Microsoft.AspNetCore.Components.Web;
using Storyteller.BlazorApp.KeyHandlers;

namespace Storyteller.BlazorApp.Tests.Unit.KeyHandlers;

public sealed class BackspaceHandlerTests
{
    private static readonly KeyHandlerArgs EmptyArgsFixture =
        new(() => "", _ => { }, () => 0, _ => { });

    private static BackspaceHandler SutFixture(KeyHandlerArgs args) => new(args);

    [Fact]
    public void OnKey_ShouldNotSetText_WhenAtSentenceStart()
    {
        // Arrange.
        var setText = "";
        var sut = SutFixture(new KeyHandlerArgs(() => "Text", x => setText = x, () => 4, _ => { }));

        // Act.
        sut.OnKey(new KeyboardEventArgs { Key = "Backspace" });

        // Assert.
        setText.Should().BeEmpty();
    }

    [Fact]
    public void OnKey_ShouldReturnFalse_WhenKeyIsNotBackspace()
    {
        // Arrange.
        var sut = SutFixture(EmptyArgsFixture);

        // Act.
        var handled = sut.OnKey(new KeyboardEventArgs { Key = "a" });

        // Assert.
        handled.Should().BeFalse();
    }

    [Fact]
    public void OnKey_ShouldReturnTrue_WhenAtSentenceStart()
    {
        // Arrange.
        var sut = SutFixture(new KeyHandlerArgs(() => "Text", _ => { }, () => 4, _ => { }));

        // Act.
        var handled = sut.OnKey(new KeyboardEventArgs { Key = "Backspace" });

        // Assert.
        handled.Should().BeTrue();
    }

    [Fact]
    public void OnKey_ShouldSetSentenceStart_WhenHandled()
    {
        // Arrange.
        var sentenceStart = 0;
        var sut = SutFixture(
            new KeyHandlerArgs(() => "Text", _ => { }, () => 0, x => sentenceStart = x)
        );

        // Act.
        sut.OnKey(new KeyboardEventArgs { Key = "Backspace" });

        // Assert.
        sentenceStart.Should().Be(0);
    }

    [Fact]
    public void OnKey_ShouldSetText_WhenNotAtSentenceStart()
    {
        // Arrange.
        var setText = "";
        var sut = SutFixture(new KeyHandlerArgs(() => "Text", x => setText = x, () => 0, _ => { }));

        // Act.
        sut.OnKey(new KeyboardEventArgs { Key = "Backspace" });

        // Assert.
        setText.Should().Be("Tex");
    }
}
