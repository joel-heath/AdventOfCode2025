# Advent of Code 2025
My C# solutions to [Advent of Code 2025](https://adventofcode.com/2025).
View my (lesser) solutions in Haskell [here](https://github.com/joel-heath/AoC25Haskell).

## Set-up
If you'd like to run my solutions on your input, you can clone this repo, and either manually create the file Inputs/Day{n}.txt, or alternatively you can run `dotnet user-secrets set SessionToken your-aoc-session-token`, and your input will be fetched automatically.

This project is using `.NET 10.0`.

## Notes
Here you can easily navigate each days code and read about how well I think I did.

In previous years I turned my solutions into one-liners for the lols (many were atrocious)
Lazily doing so often resulted in impure methods, and solutions that looked functional but were really just imperative code in disguise.

But not to fear! This year, I need to learn Haskell for uni, so after solving each problem in C#, I will turn it functional then convert the code into Haskell.
Hopefully, for the most part, this will make my C# code more succinct and clear, however it may end up worse in some cases, so this year, we shall have a NEW AND IMPROVED LEGEND:

### Legend
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

#### Purely functional solutions
🟣 The quintessential one-liner. \
🟢 Functions are allowed: 1. base cases 2. variable initialisations 3. return line. \
🟡 Fully functional solution that is a bit all-over-the-place.

#### Multi-paradigm solutions
🟪 The quintessential multi-paradigm solution. Clear reduced code that is functional where possible with simplified iterative parts. \
🟩 Short, succinct code \
🟨 Unreduced code, but not largely inefficient. \
🟥 A poorer solution than most out there. (Brute forces when there's no need, hard-coded to my input etc.)

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2025/Day01.cs) | 🟢 | For part two, the simplest of code would just create a range and iterate, this would also work well in Haskell but is unnecessarily inefficient. Instead, I took a better approach: divide by 100 before wrapping, but beware off-by-one errors!! 😱⚠️🐞🚨🤯🔢➕1️⃣👽 |
| [2](AdventOfCode2025/Day02.cs) | 🟣 | Today's problem prompted me to create new extension methods, `.CountLessThan(k)` and `.CountGreaterThanOrEqual(k)` etc because `!list.Skip(k).Any()` is hard to read in my opinion. |
| [3](AdventOfCode2025/Day03.cs) | 🟢 | Not much to say today except "a recursive solution on day three!?!?". It'll lend itself nicely to a Haskell conversion (except for the `.SkipLast(n)` calls, not the most idiomatic but oh well). |
| [4](AdventOfCode2025/Day04.cs) | 🟢 | Today's part two was a monster to turn functional, I had to create new constructors for my `Grid<T>` class, and a new extension method `.SelectAggregate()`, which produces a new list and returns an accumulator threaded through. |
| [5](AdventOfCode2025/Day05.cs) | 🟪 | [Déjà vu on part 2…](https://github.com/joel-heath/AdventOfCode2022/blob/master/AdventOfCode2022/Day15.cs) I initially converted today's into functional code, but admittedly it looked pretty terrible in C# [(it's cleaner in Haskell though!)](https://github.com/joel-heath/AoC25Haskell/blob/main/Day5.hs), so I've stuck with my original imperative solution. It merges ranges in average time `O(nlogn)` by sorting then merging in one `O(n)` passthrough. |
| [6](AdventOfCode2025/Day06.cs) | 🟣 | Today's part two brought to my attention the lack of a generic extension of `string.Split()`. So I created created `.SplitBy()`, where `charArray.SplitBy(' ')` is the same as `string.Split(' ')`, but you can also do `lines.Split("")` to split on empty lines. With that and a touch of `.Trim()`, part two was pretty smooth sailing. |
| [7](AdventOfCode2025/Day07.cs) | 🟩 | I solved today's by keeping a collection of beams (rather than drawing the beams on the grid) which made extending to part two much easier. In my opinion today's begs to be imperative with mutable sets / dictionaries, since my Haskell solution mimics my C# one by creating new collections each time this solution would modify the collection. |
| [8](AdventOfCode2025/Day08.cs) | 🟨 | The only 'trick' to today's was generating all connections and ordering them (rather than generating only those that were needed, requiring you to check each time whether you'd already evaluated it). To address the not-green-or-higher rating for todays: idk how to reduce this. The Haskell solution takes multiple millennia so don't look at that. |
| [9](AdventOfCode2025/Day09.cs) | 🟢 | We've had these kinds of polygons (simple or otherwise!) before and I've never really been great at them, reaching for online resources to explain flood fill and collision calculations. This year I've had enough so I've written my own `Line` and `Rectangle` classes composing my `Point` class, and a collection of utilities (like Point-In-Polygon, AABB collision, and todays, testing if a line segment intersects with a rectangle) into a new static `Polygons` class. Anywho regarding the solution itself, my original working approach was: 1. check the implicit corners are inside the polygon (PIP), 2. check there are no polygon vertices inside the rectangle, 3. check no edge intersects with the rectangle, and finally, given that all the other tests passed, the rectangle is wholly empty, and is internally either fully inside or outside the polygon, so I just test PIP the midpoint of the rectangle. But after looking online I saw many solving it using just step 3., and low and behold check #3 solves it entirely. I cannot rationalise this at all in my head so I assume it's a quirk of inputs and isn't fully general but I literally have no idea. (holy yap) |
| [10](AdventOfCode2025/Day10.cs) | 🟨 | So, I learnt the two-phase Simplex method in A-Level Further Maths, and we covered the branch-and-bound method last year of uni, thus I decided I'd just code it all myself rather than relying on a library, because, what could go wrong? Naturally, I gave up after many hours of work and ending up at an algorithm that seems to work most the time but clearly not all the time, so I resorted to using a library. After much searching, I found HiGHS to be one of the few with a simple C# API and no binaries I have to compile myself. The documentation is atrocious (it has ONE example that doesn't even do ILP and entering the constraints is awful) so I had to vibecode the conversion from my (NORMAL) representation to HiGHS' weird one (thanks Gemini). |
| [11](AdventOfCode2025/Day11.cs) | ⚫ |  |
| [12](AdventOfCode2025/Day12.cs) | ⚫ |  |