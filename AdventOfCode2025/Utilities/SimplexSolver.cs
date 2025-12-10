namespace AdventOfCode2025.Utilities;

public static class IntegerSimplex
{
    public static (int Minimum, int[] Assignments) Minimise(
        double[] objectiveFunction,
        double[][] constraintsLT,
        double[][] constraintsEQ,
        double[][] constraintsGT)
    {
        // Avoid mutating the original array provided by the caller
        var negatedObjective = objectiveFunction.Select(x => -x).ToArray();

        (int maximum, int[] assignments) = Maximise(negatedObjective, constraintsLT, constraintsEQ, constraintsGT);

        return (-maximum, assignments);
    }

    public static (int Maximum, int[] Assignments) Maximise(
        double[] objectiveFunction,
        double[][] constraintsLT,
        double[][] constraintsEQ,
        double[][] constraintsGT)
    {
        // Branch and Bound
        throw new NotImplementedException();
    }
}

public static class Simplex
{
    // floating-point tolerance
    public const double EPS = 1e-12;

    private static bool Equals(double x, double y)
        => Math.Abs(x - y) <= EPS; 

    private static bool TwoPhaseNeeded(double[][] constraintsLT, double[][] constraintsGT)
    {
        // if we have any >= constraints, we need two-phase
        if (constraintsGT.Length > 0)
            return true;

        // if we have any <= constraints with negative RHS, we need two-phase
        foreach (var constraint in constraintsLT)
        {
            if (constraint[^1] < -EPS)
                return true;
        }

        return false;
    }

    private static double[] NegateRow(double[] row)
        => [..row.Select(x => -x)];

    public static (double Minimum, double[] Assignments) Minimise(
        double[] objectiveFunction,
        double[][] constraintsLT,
        double[][] constraintsEQ,
        double[][] constraintsGT)
    {
        // we have Min. P = ax + by + cz + ...
        // Minimal P == -(Maximal -P)
        // Let Q = -P = -ax - by - cz - ...
        // Max. Q

        for (int i = 0; i < objectiveFunction.Length; i++)
            objectiveFunction[i] = -objectiveFunction[i];

        (double maximum, double[] assignments) = Maximise(objectiveFunction, constraintsLT, constraintsEQ, constraintsGT);

        return (-maximum, assignments);
    }

    // objective function is of form Max. P = [0]x + [1]y + [2]z + ...
    // each constraintLT is of the form [0]x + [1]y + [2]z + ... <= [^1]
    // constraintEQ is = and GT is >=
    // so given `n` variables, length of `objectiveFunction` = n, and length of each constraint = n + 1
    public static (double Maximum, double[] Assignments) Maximise(
        double[] objectiveFunction,
        double[][] constraintsLT,
        double[][] constraintsEQ,
        double[][] constraintsGT)
    {
        List<double[]> finalLT = [];
        List<double[]> finalGT = [];

        foreach (var c in constraintsLT)
        {
            if (c[^1] < -EPS) finalGT.Add(NegateRow(c));
            else finalLT.Add(c);
        }
        foreach (var c in constraintsGT)
        {
            if (c[^1] < -EPS) finalLT.Add(NegateRow(c));
            else finalGT.Add(c);
        }
        foreach (var c in constraintsEQ)
        {
            if (c[^1] < -EPS)
            {
                finalGT.Add(NegateRow(c));
                finalLT.Add(NegateRow(c));
            }
            else
            {
                finalLT.Add(c);
                finalGT.Add(c);
            }
        }

        return Maximise(objectiveFunction, [..finalLT], [..finalGT]);
    }

    private static (double Maximum, double[] Assignments) Maximise(
        double[] objectiveFunction,
        double[][] constraintsLT,
        double[][] constraintsGT)
    {
        /// first arrange normal simplex tableau with slack and surplus variables

        int variables = objectiveFunction.Length,
            slackVariables = constraintsLT.Length,
            surplusVariables = constraintsGT.Length; // == artificial variables

        int rows = 1 + constraintsLT.Length + constraintsGT.Length;
        int cols = 1 + variables + slackVariables + surplusVariables + 1;

        double[][] tableau = new double[rows][];

        // objective row
        tableau[0] = [1d, .. objectiveFunction.Select(x => -x), .. Enumerable.Repeat(0d, slackVariables), .. Enumerable.Repeat(0d, surplusVariables), 0d];

        // constraint rows
        for (int i = 0; i < constraintsLT.Length; i++)
        {
            var constraint = constraintsLT[i];
            var slackPadding = Enumerable.Range(0, slackVariables).Select(j => j == i ? 1d : 0d);
            var surplusPadding = Enumerable.Repeat(0d, surplusVariables);
            tableau[1 + i] = [0, .. constraint[..^1], .. slackPadding, .. surplusPadding, constraint[^1]];
        }
        for (int i = 0; i < constraintsGT.Length; i++)
        {
            var constraint = constraintsGT[i];
            var slackPadding = Enumerable.Repeat(0d, slackVariables);
            var surplusPadding = Enumerable.Range(0, surplusVariables).Select(j => j == i ? -1d : 0d);
            tableau[1 + constraintsLT.Length + i] = [0d, .. constraint[..^1], .. slackPadding, .. surplusPadding, constraint[^1]];
        }


        if (TwoPhaseNeeded(constraintsLT, constraintsGT))
        {
            /// Phase 1: Minimize Artificial Variables
            /// expand to add artificial variables

            int aRows = 1 + 1 + constraintsLT.Length + constraintsGT.Length;
            int aCols = 1 + 1 + variables + slackVariables + surplusVariables + surplusVariables + 1;

            double[][] aTableau = new double[aRows][];

            // objective row for artificial minimization

            // we need to sum all equations with artificial variables
            // so every >= constraint
            var artificialObjectiveFunction =
                tableau[(1 + constraintsLT.Length)..]
                .Select(variable => variable.Sum())
                .ToArray();

            //             A      P, x, y, z, s1, s2, ...               a1, a2, ...m                             RHS
            aTableau[0] = [1d, .. artificialObjectiveFunction[..^1], .. Enumerable.Repeat(0d, surplusVariables), artificialObjectiveFunction[^1]];

            // actual objective function
            //             A      P,x,s1,...           a1,a2,...                                RHS (also makes sense to do tableau[0][^1])
            aTableau[1] = [0d, .. tableau[0][..^1], .. Enumerable.Repeat(0d, surplusVariables), 0d];

            // constraint rows
            for (int i = 0; i < constraintsLT.Length; i++)
            {
                var constraint = tableau[1 + i];
                var artificialPadding = Enumerable.Repeat(0d, surplusVariables);
                aTableau[1 + 1 + i] = [0d, .. constraint[..^1], .. artificialPadding, constraint[^1]];
            }
            for (int i = 0; i < constraintsGT.Length; i++)
            {
                var constraint = tableau[1 + constraintsLT.Length + i];
                var artificialPadding = Enumerable.Range(0, surplusVariables).Select(j => j == i ? 1d : 0d);
                aTableau[1 + 1 + constraintsLT.Length + i] = [0d, .. constraint[..^1], .. artificialPadding, constraint[^1]];
            }

            // we want to minimise so let's maximise the negative
            aTableau[0] = [.. aTableau[0].Select((v, i) => i == 0 ? v : -v)];

            OneStageSimplex(aTableau, doNotPivotOnRow1: true);

            double minimum = -aTableau[0][^1];

            // check if feasible (did we reduce artificial cost to 0?)
            if (minimum < -EPS)
                throw new Exception("Infeasible");

            /// Phase 2: Maximize Original Objective
            // tableau is appropriate size, we just need to fill with the values from phase 1

            for (int i = 0; i < rows; i++)
            {
                double[] artificialRow = aTableau[i + 1]; // skip artificial objective row
                double[] reducedRow =
                    [.. artificialRow[1..(1 + 1 + variables + slackVariables + surplusVariables)], // skip artificial variables
                 artificialRow[^1]]; // RHS

                tableau[i] = reducedRow;
            }
        }

        OneStageSimplex(tableau);

        double maximum = tableau[0][^1];
        double[] assignments = new double[variables];

        for (int c = 0; c < variables; c++)
        {
            double optimum = double.NaN;

            for (int r = 0; r < rows; r++)
            {
                double value = tableau[r][c + 1]; // skip the objective column

                if (Equals(value, 1))
                {
                    if (!double.IsNaN(optimum))
                    {
                        optimum = 0;
                        break;
                    }
                    optimum = tableau[r][^1];
                }
                else if (!Equals(value, 0))
                {
                    optimum = 0;
                    break;
                }
            }

            assignments[c] = optimum;
        }

        return (maximum, assignments);
    }

    // `doNotPivotOnRow1` is used for two-phase simplex to avoid pivoting on the actual objective row when minimising the artificial objective function
    private static double[][] OneStageSimplex(double[][] tableau, bool doNotPivotOnRow1 = false)
    {
        bool maximised = false;
        while (!maximised)
            maximised = Iteration(tableau, doNotPivotOnRow1);

        return tableau;
    }

    // returns -1 if top row has no negatives (maximised)
    private static int SelectEnteringVariable(double[][] tableau)
    {
        double[] objectiveRow = tableau[0][1..^1]; // skip objective variable [0] and RHS [^1]

        (double min, int minIndex) = objectiveRow.Select((v, i) => (v, i)).MinBy(t => t.v);

        return min < 0 ? (1 + minIndex) : -1; // add 1 because we skipped objective variable
    }

    // use the minimum ratio test
    private static int SelectLeavingVariable(double[][] tableau, int entering, bool doNotPivotOnRow1)
    {
        double minVal = double.PositiveInfinity;
        int minRow = -1;

        // skip objective row(s)
        for (int i = doNotPivotOnRow1 ? 2 : 1; i < tableau.Length; i++)
        {
            // pivot must be positive, ratio must be non-negative hence RHS must be nonnegative
            if (tableau[i][entering] <= EPS || tableau[i][^1] < 0)
                continue;

            double val = tableau[i][^1] / tableau[i][entering];

            if (val < minVal) (minRow, minVal) = (i, val);
        }

        return minRow;
    }

    // returns true if maximum value has been found
    static bool Iteration(double[][] tableau, bool doNotPivotOnRow1)
    {
        int chosenCol = SelectEnteringVariable(tableau);
        if (chosenCol == -1)
            return true;

        int chosenRowIndex = SelectLeavingVariable(tableau, chosenCol, doNotPivotOnRow1);
        if (chosenRowIndex == -1)
            throw new Exception("Linear program is unbounded");

        double[] chosenRow = tableau[chosenRowIndex];
        double dividend = chosenRow[chosenCol];

        // divide chosen row first
        for (int i = 0; i < chosenRow.Length; i++)
            chosenRow[i] /= dividend;

        // reduce all other values of variable to 0
        for (int r = 0; r < tableau.Length; r++)
        {
            if (r == chosenRowIndex)
                continue;

            double[] row = tableau[r];

            double scalar = -row[chosenCol];

            for (int c = 0; c < row.Length; c++)
                row[c] += scalar * chosenRow[c];
        }

        return false;
    }
}