``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.6 (18G95) [Darwin 18.7.0]
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.401
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT


```
| Method |     Mean |    Error |   StdDev |      Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |---------:|---------:|---------:|-----------:|------:|------:|----------:|
|  Bench | 247.0 ms | 4.855 ms | 4.769 ms | 55000.0000 |     - |     - | 249.48 MB |
