``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1265/22H2/2022Update/SunValley2)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.201
  [Host]     : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2


```
|                           Method |     Mean |     Error |    StdDev |
|--------------------------------- |---------:|----------:|----------:|
|                         UseLoops | 3.585 μs | 0.0226 μs | 0.0176 μs |
|            UseLoopsPreRangeCheck | 3.312 μs | 0.0191 μs | 0.0179 μs |
|                           Plain0 | 2.086 μs | 0.0173 μs | 0.0162 μs |
|                            Plain | 2.165 μs | 0.0214 μs | 0.0190 μs |
|               PlainPreRangeCheck | 1.945 μs | 0.0163 μs | 0.0153 μs |
|                     PlainReverse | 2.037 μs | 0.0151 μs | 0.0141 μs |
|        PlainPreRangeCheckReverse | 2.042 μs | 0.0232 μs | 0.0217 μs |
|     PlainPreRangeCheckPassAllTks | 1.656 μs | 0.0141 μs | 0.0132 μs |
|                        PlainFlat | 1.800 μs | 0.0209 μs | 0.0195 μs |
| PlainPreRangeCheckPassAllDTsFlat | 1.658 μs | 0.0183 μs | 0.0171 μs |
