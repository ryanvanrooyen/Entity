
//using NUnit.Framework;
//using NSubstitute;

//public class ButtonTests
//{
//	[Test]
//	public void EnableTests()
//	{
//        var input = Substitute.For<IButtonInput>();
//        var icon = Substitute.For<IButtonIcon>();

//        input.IsPressed.Returns(true);
//        input.WasPressed.Returns(true);

//        // Buttons should be enabled by default.
//        AssertEnabled(new Button(input, icon));

//        var b1 = Substitute.For<IButton>();
//        b1.IsPressed.Returns(true);
//        b1.WasPressed.Returns(true);
//        var b2 = Substitute.For<IButton>();
//        b2.IsPressed.Returns(true);
//        b2.WasPressed.Returns(true);

//        AssertEnabled(new ButtonCombo(b1, b2));
//    }

//    private void AssertEnabled(IButton button)
//    {
//        Assert.IsTrue(button.IsPressed);
//        Assert.IsTrue(button.WasPressed);

//        button.Enabled = false;
//        Assert.IsFalse(button.IsPressed);
//        Assert.IsFalse(button.WasPressed);
//    }

//    [Test]
//    public void AreEqualTests()
//    {
//        var b1Input = Substitute.For<IButtonInput>();
//        var b2Input = Substitute.For<IButtonInput>();
//        b1Input.Name.Returns("input1");
//        var icon = Substitute.For<IButtonIcon>();

//        IButton b1 = new Button(b1Input, icon);
//        Assert.IsFalse(b1.Equals(null));

//        b2Input.Name.Returns("Input1");
//        IButton b2 = new Button(b2Input, icon);
//        Assert.IsTrue(b1.Equals(b2));

//        b2Input.Name.Returns("input2");
//        Assert.IsFalse(b1.Equals(b2));
//    }
//}
