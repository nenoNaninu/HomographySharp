# HomographySharp
HomographySharp is a (C#/.NET Standard2.0) class library for finding and using homography matrics.

# Install
Package Manager
```
PM > Install-Package HomographySharp -Version 1.0.2
```
.NET CLI
```
dotnet add package HomographySharp --version 1.0.2
```

# How to use
```c#
var srcList = new List<DenseVector>(4);
var dstList = new List<DenseVector>(4);

srcList.Add(DenseVector.OfArray(new double[] { 10, 10 }));
srcList.Add(DenseVector.OfArray(new double[] { 100, 10 }));
srcList.Add(DenseVector.OfArray(new double[] { 100, 150 }));
srcList.Add(DenseVector.OfArray(new double[] { 10, 150 }));

dstList.Add(DenseVector.OfArray(new double[] { 11,11 }));
dstList.Add(DenseVector.OfArray(new double[] { 500, 11 }));
dstList.Add(DenseVector.OfArray(new double[] { 500, 200 }));
dstList.Add(DenseVector.OfArray(new double[] { 11, 200 }));

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
  - https://github.com/mathnet/mathnet-numerics/blob/master/LICENSE.md
