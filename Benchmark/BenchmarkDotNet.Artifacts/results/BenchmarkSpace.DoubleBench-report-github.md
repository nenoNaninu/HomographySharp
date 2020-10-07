``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.1082 (1903/May2019Update/19H1)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET Core SDK=3.1.402
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT


```
| Method |     Mean |   Error |  StdDev |      Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |---------:|--------:|--------:|-----------:|------:|------:|----------:|
|  Bench | 204.0 ms | 2.77 ms | 2.46 ms | 44000.0000 |     - |     - | 352.48 MB |
