Console.WriteLine("CSharpInDepth.CSharpTwoToFive");

internal static class ExploringAsync
{
    private static async Task PrintAndWait(TimeSpan delay)
    {
        Console.WriteLine("Before first delay");
        await Task.Delay(delay);
        Console.WriteLine("Between delays");
        await Task.Delay(delay);
        Console.WriteLine("After second delay");
    }
}

// ThrowExceptionAsync();
// while (true)
// {
//     await Task.Delay(1000);
//     Console.WriteLine("Still running");
// }
// async void ThrowExceptionAsync()
// {
//     await Task.Delay(5000);
//     throw new Exception("test");
// }
