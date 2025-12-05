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
🟢 Functions are allowed 1. base cases, 2. variable initialisations, and 3. return line. \
🟡 Fully functional solution that is a bit all-over-the-place. \
🟠 Not fully functional. Short, succinct, imperitive code. \
🔴 A poorer solution than most out there. \
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2025/Day01.cs) | 🟢 | For part two, the simplest of code would just create a range and iterate, this would also work well in Haskell but is unnecessarily inefficient. Instead, I took a better approach: divide by 100 before wrapping, but beware off-by-one errors!! 😱⚠️🐞🚨🤯🔢➕1️⃣👽 |
| [2](AdventOfCode2025/Day02.cs) | 🟢 | Today's problem prompted me to create new extension methods, `.CountLessThan(k)` and `.CountGreaterThanOrEqual(k)` etc because `!list.Skip(k).Any()` is hard to read in my opinion. |
| [3](AdventOfCode2025/Day03.cs) | 🟢 | Not much to say today except "a recursive solution on day three!?!?". It'll lend itself nicely to a Haskell conversion (except for the `.SkipLast(n)` calls, not the most idiomatic but oh well). |
| [4](AdventOfCode2025/Day04.cs) | 🟢 | Today's part two was a monster to turn functional, I had to create new constructors for my `Grid<T>` class, and a new extension method `.SelectAggregate()`, which produces a new list and reutrns an accumulator threaded through. |
| [5](AdventOfCode2025/Day05.cs) | 🟡 | Today's part 2 gave [déjà vu](https://github.com/joel-heath/AdventOfCode2022/blob/master/AdventOfCode2022/Day15.cs), rather than copying my old code I came up with a new solution which was thankfully much faster to write and is O(nlogn) rather than O(n^2). With regards to the "verbosity score", this one could be seen as green, I gave it yellow because it has variable declarations and return cases interwoven. |
| [6](AdventOfCode2025/Day06.cs) | ⚫ |  |
| [7](AdventOfCode2025/Day07.cs) | ⚫ |  |
| [8](AdventOfCode2025/Day08.cs) | ⚫ |  |
| [9](AdventOfCode2025/Day09.cs) | ⚫ |  |
| [10](AdventOfCode2025/Day10.cs) | ⚫ |  |
| [11](AdventOfCode2025/Day11.cs) | ⚫ |  |
| [12](AdventOfCode2025/Day12.cs) | ⚫ |  |
