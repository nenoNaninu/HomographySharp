using System.Collections.Generic;
using System.Text.Json;
using HomographySharp;
using NUnit.Framework;

namespace HomographySharpTest;

public class SerializeTests
{
    [Test]
    public void DoublePointsSerialization()
    {
        //Given
        var srcList = new List<Point2<double>>(4);
        var dstList = new List<Point2<double>>(4);

        srcList.Add(new Point2<double>(-152, 394));
        srcList.Add(new Point2<double>(218, 521));
        srcList.Add(new Point2<double>(223, -331));
        srcList.Add(new Point2<double>(-163, -219));

        dstList.Add(new Point2<double>(-666, 431));
        dstList.Add(new Point2<double>(500, 300));
        dstList.Add(new Point2<double>(480, -308));
        dstList.Add(new Point2<double>(-580, -280));

        var actual = Homography.Find(srcList, dstList);

        //When
        string json = JsonSerializer.Serialize(actual);
        var expected = JsonSerializer.Deserialize<HomographyMatrix<double>>(json);

        //Then
        TestContext.WriteLine(json);
        Assert.AreEqual(actual.Elements, expected.Elements);
    }

    [Test]
    public void FloatPointsSerialization()
    {
        //Given
        var srcList = new List<Point2<float>>(4);
        var dstList = new List<Point2<float>>(4);

        srcList.Add(new Point2<float>(-152, 394));
        srcList.Add(new Point2<float>(218, 521));
        srcList.Add(new Point2<float>(223, -331));
        srcList.Add(new Point2<float>(-163, -219));

        dstList.Add(new Point2<float>(-666, 431));
        dstList.Add(new Point2<float>(500, 300));
        dstList.Add(new Point2<float>(480, -308));
        dstList.Add(new Point2<float>(-580, -280));

        var actual = Homography.Find(srcList, dstList);

        //When
        string json = JsonSerializer.Serialize(actual);
        var expected = JsonSerializer.Deserialize<HomographyMatrix<float>>(json);

        //Then
        TestContext.WriteLine(json);
        Assert.AreEqual(actual.Elements, expected.Elements);
    }
}
