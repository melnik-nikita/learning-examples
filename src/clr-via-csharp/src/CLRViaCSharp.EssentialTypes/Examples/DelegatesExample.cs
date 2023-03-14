namespace CLRViaCSharp.EssentialTypes.Examples;

internal class DelegatesExample
{
    public static void Run()
    {
        Console.WriteLine("Delegates example start");
        StaticDelegateDemo();
        InstanceDelegateDemo();
        Chain1DelegateDemo(new DelegatesExample());
        Chain2DelegateDemo(new DelegatesExample());
        Console.WriteLine("------------------------------------");

        UsingLocalVariablesInTheCallbackCode(5);
        Console.WriteLine("------------------------------------");

        Console.WriteLine("Delegates example end");
    }

    public static void CallbackWithoutNewingADelegateObject()
    {
        // Compiler defines new private method in the class
        ThreadPool.QueueUserWorkItem(obj => Console.WriteLine(obj), 5);
    }

    public static void UsingLocalVariablesInTheCallbackCode(Int32 numToDo)
    {
        // Helper class is generated behind the scenes to store values used by delegate
        /*
            [CompilerGenerated]
            private sealed class <>c__DisplayClass2_0
            {
                [Nullable(0)]
                public int[] squares;
                public int numToDo;
                [Nullable(0)]
                public AutoResetEvent done;
                [Nullable(0)]
                public WaitCallback <>9__0;

                public <>c__DisplayClass2_0()
                {
                    base..ctor();
                }

                [NullableContext(2)]
                internal void <UsingLocalVariablesInTheCallbackCode>b__0(object obj)
                {
                    int index = (int) obj;
                    this.squares[index] = index * index;
                    if (Interlocked.Decrement(ref this.numToDo) != 0)
                        return;
                    this.done.Set();
                }
            }
         */
        var squares = new Int32[numToDo];
        var done = new AutoResetEvent(false);

        // Do tasks on other threads
        for (var n = 0; n < squares.Length; n++)
        {
            ThreadPool.QueueUserWorkItem(
                obj =>
                {
                    var num = (Int32)obj;
                    squares[num] = num * num;
                    // If last task, let main thread continue running
                    if (Interlocked.Decrement(ref numToDo) == 0)
                    {
                        done.Set();
                    }
                },
                n
            );
        }

        // Wait for all the other threads to finish
        done.WaitOne();
        // Show results
        for (var n = 0; n < squares.Length; n++)
        {
            Console.WriteLine($"Index {n}, Square={squares[n]}");
        }
    }

    #region FirstLook

    private static void StaticDelegateDemo()
    {
        Console.WriteLine("----- Static Delegate Demo ------");
        Counter(1, 3, null);
        Counter(1, 3, new Feedback(FeedbackToConsole));
        Counter(1, 3, new Feedback(FeedbackToMsgBox));
        Console.WriteLine();
    }

    private static void InstanceDelegateDemo()
    {
        Console.WriteLine("----- Instance Delegate Demo ------");
        var example = new DelegatesExample();
        Counter(1, 3, new Feedback(example.FeedbackToFile));
        Console.WriteLine();
    }

    private static void Chain1DelegateDemo(DelegatesExample example)
    {
        Console.WriteLine("----- Chain Delegate Demo 1 ------");
        var fb1 = new Feedback(FeedbackToConsole);
        var fb2 = new Feedback(FeedbackToMsgBox);
        var fb3 = new Feedback(example.FeedbackToFile);

        Feedback fbChain = null;

        fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
        fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
        fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
        Counter(1, 2, fbChain);

        Console.WriteLine();
        fbChain = (Feedback)Delegate.Remove(fbChain, new Feedback(FeedbackToMsgBox))!;
        Counter(1, 2, fbChain);
    }

    private static void Chain2DelegateDemo(DelegatesExample example)
    {
        Console.WriteLine("----- Chain Delegate Demo 2 ------");
        var fb1 = new Feedback(FeedbackToConsole);
        var fb2 = new Feedback(FeedbackToMsgBox);
        var fb3 = new Feedback(example.FeedbackToFile);

        Feedback fbChain = null;

        fbChain += fb1;
        fbChain += fb2;
        fbChain += fb3;
        Counter(1, 2, fbChain);

        Console.WriteLine();
        fbChain -= new Feedback(FeedbackToMsgBox);
        Counter(1, 2, fbChain);
    }

    private static void Counter(Int32 from, Int32 to, Feedback fb)
    {
        for (var val = from; val <= to; val++)
        {
            // if (fb is not null)
            // {
            //     fb(val);
            // }
            fb?.Invoke(val);
        }
    }

    private static void FeedbackToConsole(int value)
    {
        Console.WriteLine($"Item={value}");
    }

    private static void FeedbackToMsgBox(int value)
    {
        // MessageBox.Show($"Item={value}");
        Console.WriteLine($"To message box:");
        Console.WriteLine($"Item={value}");
    }

    private void FeedbackToFile(int value)
    {
        using var sw = new StreamWriter("Status", true);
        sw.WriteLine($"Item={value}");
    }

    #endregion FirstLook
}

internal delegate Object MyCallback(FileStream fs);

// Declare a delegate type
internal delegate void Feedback(Int32 value);
// Class generated behind the scenes
// internal class Feedback : System.MulticastDelegate
// {
//     public Feedback(Object @object, IntPtr method);
//
//     public virtual void Invoke(Int32 value);
//
//     public virtual IAsyncResult BeginInvoke(Int32 value, AsyncCallback cb, Object @object);
//     public virtual void EndInvoke(IAsyncResult result);
// }
