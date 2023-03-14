namespace CLRViaCSharp.EssentialTypes.Examples;

internal static class ArraysExample
{
    public static void Run()
    {
        Console.WriteLine("Arrays example start");

        string[] names = { "Aidan", "Grant" };

        NonZeroLowerBoundArrays();

        Console.WriteLine("Arrays example end");
    }

    private static void NonZeroLowerBoundArrays()
    {
        int[] lowerBounds = { 2005, 1 };
        int[] lengths = { 5, 4 };
        var quarterlyRevenue = (Decimal[,])Array.CreateInstance(typeof(Decimal), lengths, lowerBounds);

        Console.WriteLine("{0,4} {1,9} {2,9} {3,9} {4,9}", "Year", "Q1", "Q2", "Q3", "Q4");

        var firstYear = quarterlyRevenue.GetLowerBound(0);
        var lastYear = quarterlyRevenue.GetUpperBound(0);
        var firstQuarter = quarterlyRevenue.GetLowerBound(1);
        var lastQuarter = quarterlyRevenue.GetUpperBound(1);

        for (var year = firstYear; year <= lastYear; year++)
        {
            Console.Write($"{year} ");
            for (var quarter = firstQuarter; quarter <= lastQuarter; quarter++)
            {
                Console.Write("{0,9:C} ", quarterlyRevenue[year, quarter]);
            }

            Console.WriteLine();
        }
    }
}
