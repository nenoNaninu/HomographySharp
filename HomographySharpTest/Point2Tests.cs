using HomographySharp;
using NUnit.Framework;

namespace HomographySharpTest;

public class Point2Tests
{
    [Test]
    [TestCase(0, 0, 0, 0, true)]
    [TestCase(0, 1.5f, 0, 1.5f, true)]
    [TestCase(2.5f, 1.5f, 2.5f, 1.5f, true)]
    [TestCase(0, 1.5f, 2.5f, 1.5f, false)]
    [TestCase(2.5f, 0, 2.5f, 1.5f, false)]
    [TestCase(2.5f, 1.5f, 0, 1.5f, false)]
    [TestCase(2.5f, 1.5f, 2.5f, 0, false)]
    public void Point2FloatEquality(float aX, float aY, float bX, float bY, bool areEqual)
    {
        //Given
        var a = new Point2<float>(aX, aY);
        var b = new Point2<float>(bX, bY);

        //Then
        if (areEqual)
        {
            Assert.AreEqual(a, b);
            Assert.True(a == b);
            Assert.False(a != b);
        }
        else
        {
            Assert.AreNotEqual(a, b);
            Assert.False(a == b);
            Assert.True(a != b);
        }
    }

    [Test]
    [TestCase(0, 0, 0, 0, true)]
    [TestCase(0, 1.5f, 0, 1.5f, true)]
    [TestCase(2.5f, 1.5f, 2.5f, 1.5f, true)]
    [TestCase(0, 1.5f, 2.5f, 1.5f, false)]
    [TestCase(2.5f, 0, 2.5f, 1.5f, false)]
    [TestCase(2.5f, 1.5f, 0, 1.5f, false)]
    [TestCase(2.5f, 1.5f, 2.5f, 0, false)]
    public void Point2DoubleEquality(double aX, double aY, double bX, double bY, bool areEqual)
    {
        //Given
        var a = new Point2<double>(aX, aY);
        var b = new Point2<double>(bX, bY);

        //Then
        if (areEqual)
        {
            Assert.AreEqual(a, b);
            Assert.True(a == b);
            Assert.False(a != b);
        }
        else
        {
            Assert.AreNotEqual(a, b);
            Assert.False(a == b);
            Assert.True(a != b);
        }
    }
}
