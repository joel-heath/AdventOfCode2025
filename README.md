# Advent of Code 2025
My C# solutions to [Advent of Code 2025](https://adventofcode.com/2025).
View my (lesser) solutions in Haskell [here](https://github.com/joel-heath/AoC25Haskell).

## Set-up
If you'd like to run my solutions on your input, you can clone this repo, and either manually create the file Inputs/Day{n}.txt, or alternatively you can run `dotnet user-secrets set SessionToken your-aoc-session-token`, and your input will be fetched automatically.

This project is using `.NET 10.0`.

## Notes
Here you can easily navigate each days code and read about how well I think I did.

In previous years I turned my solutions into one-liners for the lols (many were atrocious).
Lazily doing so often resulted in impure methods, and solutions that looked functional but were really just imperative code in disguise. \
This year, I'm doing so again, in order to convert them into Haskell. (I have to learn Haskell for uni. I would not be doing this according to my own will.) \
So there's now actually a requirement for the code to be purely functional.

### Legend
🟣 The quintessential one-liner. \
🟢 One-liner but with some functions that involve only variable declarations, then a one-line return. In a functional language these would be `let ... in ...` but C# doesn't have such syntax. \
🟡 Short, succinct code. \
🟠 Average solution that is unreduced. \
🔴 A poorer solution than most out there. \
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2025/Day01.cs) | 🟢 | For part two, the simplest of code would just create a range and iterate, this would also work well in Haskell but is unnecessarily inefficient. Instead, I took a better approach: divide by 100 before wrapping, but beware off-by-one errors!! 😱⚠️🐞🚨🤯🔢➕1️⃣👽 |
| [2](AdventOfCode2025/Day02.cs) | ⚫ |  |
| [3](AdventOfCode2025/Day03.cs) | ⚫ |  |
| [4](AdventOfCode2025/Day04.cs) | ⚫ |  |
| [5](AdventOfCode2025/Day05.cs) | ⚫ |  |
| [6](AdventOfCode2025/Day06.cs) | ⚫ |  |
| [7](AdventOfCode2025/Day07.cs) | ⚫ |  |
| [8](AdventOfCode2025/Day08.cs) | ⚫ |  |
| [9](AdventOfCode2025/Day09.cs) | ⚫ |  |
| [10](AdventOfCode2025/Day10.cs) | ⚫ |  |
| [11](AdventOfCode2025/Day11.cs) | ⚫ |  |
| [12](AdventOfCode2025/Day12.cs) | ⚫ |  |
| [13](AdventOfCode2025/Day13.cs) | ⚫ |  |
| [14](AdventOfCode2025/Day14.cs) | ⚫ |  |
| [15](AdventOfCode2025/Day15.cs) | ⚫ |  |
| [16](AdventOfCode2025/Day16.cs) | ⚫ |  |
| [17](AdventOfCode2025/Day17.cs) | ⚫ |  |
| [18](AdventOfCode2025/Day18.cs) | ⚫ |  |
| [19](AdventOfCode2025/Day19.cs) | ⚫ |  |
| [20](AdventOfCode2025/Day20.cs) | ⚫ |  |
| [21](AdventOfCode2025/Day21.cs) | ⚫ |  |
| [22](AdventOfCode2025/Day22.cs) | ⚫ |  |
| [23](AdventOfCode2025/Day23.cs) | ⚫ |  |
| [24](AdventOfCode2025/Day24.cs) | ⚫ |  |
| [25](AdventOfCode2025/Day25.cs) | ⚫ |  |
