# HomographySharp
HomographySharp is a (C#/.NET Standard2.0) class library for finding and using homography matrics.

# Install
NuGet: [HomographySharp](https://www.nuget.org/packages/HomographySharp/)

Package Manager
```
PM > Install-Package HomographySharp -Version 1.1.2
```
.NET CLI
```
dotnet add package HomographySharp --version 1.1.2
```

# How to use
```c#
var srcList = new List<PointF>(4);
var dstList = new List<PointF>(4);

srcList.Add(new PointF { X = -152, Y = 394 });
srcList.Add(new PointF { X = 218, Y = 521 });
srcList.Add(new PointF { X = 223, Y = -331 });
srcList.Add(new PointF { X = -163, Y = -219 });

dstList.Add(new PointF { X = -666, Y = 431 });
dstList.Add(new PointF { X = 500, Y = 300 });
dstList.Add(new PointF { X = 480, Y = -308 });
dstList.Add(new PointF { X = -580, Y = -280 });

var homo = HomographyHelper.FindHomography(srcList, dstList);

(double x, double y) = HomographyHelper.Translate(homo, -152, 394);
Assert.IsTrue(Math.Abs(x - -666) < 0.001);
Assert.IsTrue(Math.Abs(y - 431) < 0.001);
            
```
or
```c#
var srcList = new List<DenseVector>(4);
var dstList = new List<DenseVector>(4);

srcList.Add(DenseVector.OfArray(new double[] { 10, 10 }));
srcList.Add(DenseVector.OfArray(new double[] { 100, 10 }));
srcList.Add(DenseVector.OfArray(new double[] { 100, 150 }));
srcList.Add(DenseVector.OfArray(new double[] { 10, 150 }));

dstList.Add(HomographyHelper.CreateVector2(11, 11));
dstList.Add(HomographyHelper.CreateVector2(500, 11));
dstList.Add(HomographyHelper.CreateVector2(500, 200));
dstList.Add(HomographyHelper.CreateVector2(11, 200));

var homo = HomographyHelper.FindHomography(srcList, dstList);// <-

(double dstX, double dstY) = HomographyHelper.Translate(homo, 100, 10);// <-

Assert.IsTrue(Math.Abs(dstX - 500) < 0.001);//true
Assert.IsTrue(Math.Abs(dstY - 11) < 0.001); //true
```

# Visualize App
If you want to see how points are transformed by homography matrix, use this app.  
https://github.com/nenoNaninu/HomographySharp/tree/master/HomographyVisualizer
[![youtube](http://img.youtube.com/vi/BNACz1SPbj8/0.jpg)](https://youtu.be/BNACz1SPbj8)

# External Library 
- [Math.NET Numerics](https://github.com/mathnet/mathnet-numerics)  
  - Copyright (c) 2002-2018 Math.NET  
  - Released under the [Math.NET Numerics License(MIT/X11)](https://github.com/mathnet/mathnet-numerics/blob/master/LICENSE.md)
