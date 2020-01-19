using System.Diagnostics.Eventing.Reader;

namespace CShapQuestions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using CSharpQuestions.Lib;

    enum TaskStatus
    {
        Started,
        StillProcessing,
        Finished
    }

    delegate void CallbackDelegate(LongRunningTask t, TaskStatus status);

    public class Questions
    {
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6 };              

        // What is the difference between static and const and readonly
        public static void ConstVsStaticExplanation()
        {
            /*
            1. If you know the value will never, ever, ever change for any reason, use const.
            2. If you're unsure of whether or not the value will change, but you don't want other classes or code to be able to change it, use readonly.
            3. If you need a field to be a property of a type, and not a property of an instance of that type, use static.
            4. A const value is also implicitly static.
            5. If a const variable exists in Assembly A and is used in Assembly B, when Assembly A gets recompiled with a new value for the const variable Assembly B will still have the previous value until it is also recompiled.
            6. In the case of the const value, it is like a find-replace, the value 2 is 'baked into' the AssemblyB's IL. 
                This means that if tomorrow I'll update I_CONST_VALUE to 20 in the future. 
                AssemblyB would still have 2 till I recompile it.
                In the case of the readonly value, it is like a ref to a memory location. 
                The value is not baked into AssemblyB's IL. 
                This means that if the memory location is updated, AssemblyB gets the new value without recompilation. 
                So if I_RO_VALUE is updated to 30, you only need to build AssemblyA. All clients do not need to be recompiled.

                So if you are confident that the value of the constant won't change use a const.
                But if you have a constant that may change (e.g. w.r.t. precision).. or when in doubt, use a readonly.
            7. ReadOnly for reference types only makes the reference readonly not the values.
            8. const value is copied into every assembly that uses them (every assembly gets a local copy of values)
            */
            Console.WriteLine("How to access const type declared in a class is ConstVsStatic.SIZE. Its value is :{0}", CSharpQuestions.Lib.ConstVsStatic.SIZE);

            ConstVsStatic constVsStatic = new ConstVsStatic(100,201);
            Console.WriteLine("The ReadOnlySize variable value in first object of ConstVsStatic class is {0}", constVsStatic.ReadOnlySize);
            Console.WriteLine("The StaticSize variable value in first object of ConstVsStatic class is {0}", ConstVsStatic.StaticSize);
            Console.WriteLine("The GlobalStaticSize variable value in first object of ConstVsStatic class is {0}", ConstVsStatic.StaticGlobalSize);

            ConstVsStatic constVsStatic1 = new ConstVsStatic(200,202);
            Console.WriteLine("The ReadOnlySize variable value in second object of ConstVsStatic class is {0}", constVsStatic1.ReadOnlySize);
            Console.WriteLine("The StaticSize variable value in first object of ConstVsStatic class is {0}", ConstVsStatic.StaticSize);
            Console.WriteLine("The GlobalStaticSize variable value in first object of ConstVsStatic class is {0}", ConstVsStatic.StaticGlobalSize);
        }

        // Difference between var and dynamic
        public static void VarVsDynamicKeyword()
        {
            /* 
             1. Var is statically typed (early bounded) and Dynamic is late bounded or checked on runtime
             2. When we declare a type using var keyword the data type is evaluated at compile type.
             3. We can declare a method with return type dynamic but cannot declare a method that return var type.
             4. We can declare a variable using dynamic keyword without assigning a value during declartion but we can't do this using var keyword.
                We need to assign value to variable if we declare it using var keyword.
             5. We can use the var keyword to declare an anonymous object that helps with intellisense.
             6. We can use dynamic keyword to declare an anonymous object but the it doesn't provide intellisense and also not compile time error. So var it better option in this case.
             7. we can use dynamic keyword when we are working with components developed in a different language, though we can use var also here.
             8. We can use var or dynamic keyword when we are workign with linq expression which returns diffrent subsets of results based on some cond.

            */

            //This compiles and gives correct answer
            var x = "string1";
            int lenX = x.Length; //Here the type is evaluated at compile time.

            //This also compiles and gives correct answer
            dynamic y = "string1";
            int lenY = y.Length; //Here the type is evaluated at runtime using reflection.

            Questions questions = new Questions();
            Console.WriteLine("We can declare a method of return type dynamic and invoke it. See the value returned from GetCount method is number: {0}", questions.GetCount("NUM"));
            Console.WriteLine();
            Console.WriteLine("We can declare a method of return type dynamic and invoke it. See the value returned from GetCount method is string: {0}", questions.GetCount("STRING"));
            Console.WriteLine();
            Console.WriteLine("We can declare a method of return type dynamic and invoke it. See the value returned is TestClass object and Name property value: {0}", questions.GetCount("TestClassWithIdGuid").Name);
            Console.WriteLine();
            Console.WriteLine("We can declare a property of return type dynamic and invoke it. See the value returned is Id property value of TestClass object and Id value is Guid: {0}", questions.GetCount("TestClassWithIdGuid").Id);
            Console.WriteLine();
            Console.WriteLine("We can declare a property of return type dynamic and invoke it. See the value returned is Id property value of TestClass object and Id value is Number: {0}", questions.GetCount("TestClassWithIdNum").Id);
        }

        // Difference between object and dynamic
        public static void ObjectVsDynamic()
        {
            /*
             1. Object: When using an object, you  need to cast the object variable to the original type to use it and do the desired operations.
             2. Dynamic: Casting is not required but you need to know the properties and methods related to the stored type.
             3. Object: When we use the object keyword with a variable it can create a problem at run time if the stored value is not converted to the appropriate data type. 
                It cannot show an error at compile time but it will show an error on run time.
                For e.g., 
                string a = "Jack Ryan";
                object a1 = a;
                int b = (int)a1; // Give runtime error "Specified cast is not valid.

             4. Dynamic: When we use the dynamic keyword, it doesn't cause a problem because the compiler has all the info about the stored value.
             5. Object is useful when we don't have more information about the data type. 
             6. Dynamic is useful when we need to code using reflection or dynamic languages or with the COM objects and when getting result out of the LinQ queries.
             7. The dynamic keyword uses reflection internally and the code is excecuted by DLR(dynmaic language runtime). 
                It uses caching techniq internally so the first operation is very slow indeed as it does a huge amount of analysis, but the anslysis is cached and reused.
             8. dynamic is faster that reflection. 
             9. Reflection uses CLR whereas dynamic uses DLR.

             */

            //object s = "Foo";
            //string u = s.ToUpper(); //It gives compile error

            //dynamic s = "Foo";
            //string u = s.ToUpper(); // It works well because it resolves the type at runtime
        }

        //What is the use of yeild keyword
        public static void YieldKeywordExplanation()
        {
            // 1. yield keyword helps to do custom/statefull iteration over a collection
            // 2. When we use yield keyword, the control moves between source and caller.

            //Custom Iteration example without using temp collection
            Questions questions = new Questions();
            Console.Write("Number greater than 3 are:");
            foreach (int number in questions.Filter())
                Console.Write(" {0} ",number);

            //Statefull iteration example
            //For e.g, we want to display the running total of a list
            Console.WriteLine();
            Console.Write("Running total are:");
            foreach (int runningTotal in questions.RunningTotal())
                Console.Write(" {0} ", runningTotal);

            // 3. yield keyword effectively creates a lazy enumeration over collection items that can be much more efficient.
            // For example, yield return is a .NET sugar to return an IEnumerable with the only needed items.

            // Code without yield:
            //class MyItem
            //{
            //public MyItem() { }
            //static public IEnumerable CreateListOfItems()
            //{
            //    return new List {
            //    new MyItem(),
            //    new MyItem(),
            //    new MyItem() };
            //}
            //}

            //Same code using yield:
            //class MyItem
            //{
            //public MyItem() { }
            //static public IEnumerable CreateListOfItems()
            //{
            //yield return new MyItem();
            //yield return new MyItem();
            //yield return new MyItem();
            //}
            //}

            // The advantage of using yield is that if the function consuming your data simply needs the first item of the collection, 
            // the rest of the items won’t be created so it’s more efficient. 

            // 4. The yield operator allows the creation of items as it is demanded.That’s a good reason to use it.

            // 5. The declaration of an iterator must meet the following requirements:
            // 5.A. The return type must be IEnumerable, IEnumerable<T>, IEnumerator, or IEnumerator<T>.
            // 5.B. The declaration can't have any in ref or out parameters.

            // 6. The yield type of an iterator that returns IEnumerable or IEnumerator is object.
            // 7. If the iterator returns IEnumerable<T> or IEnumerator<T>, there must be an implicit conversion from the type of the expression in the yield return statement to the generic type parameter.

            // 8. You can't include a yield return or yield break statement in:
            // 8.A. Lambda expressions and anonymous methods.
            // 8.B. Methods that contain unsafe blocks.

            // 9. A yield return statement can't be located in a try-catch block. A yield return statement can be located in the try block of a try-finally statement.
            // 10. A yield break statement can be located in a try block or a catch block but not a finally block.            
            // 11. If the foreach body(outside of the iterator method) throws an exception, a finally block in the iterator method is executed.
            // 12. yield keyword can be used in a get accessor that is an iterator.

            var theGalaxies = new Galaxies();
            foreach (Galaxy theGalaxy in theGalaxies.NextGalaxy)
                Console.WriteLine("Galaxy name: {0} and Mega Light Year is: {1}", theGalaxy.Name, theGalaxy.MegaLightYears);

            // 13. You can use a yield break statement to end the iteration.
        }
                
        // What is the advantage of Delegates
        public static void DelegatesDemo()
        {
            // A very good example https://buildplease.com/pages/why-delegates/
            //1. A delegate is an object which refers to a method or you can say it is a reference type variable that can hold a reference to the methods. 
            //2. Delegates are mainly used in implementing the call-back methods and events.
            //3. Delegates can also be used in “anonymous methods” invocation.
            //4. A delegate will call only a method which agrees with its signature and return type. 
            //   A method can be a static method associated with a class or can be an instance method associated with an object, it doesn’t matter.
            //5. Multicasting of delegate is an extension of the normal delegate(sometimes termed as Single Cast Delegate). 
            //   It helps the user to point more than one method in a single call.
            //   Delegates are combined and when you call a delegate then a complete list of methods is called.
            //   All methods are called in First in First Out(FIFO) order.
            //   ‘+’ or ‘+=’ Operator is used to add the methods to delegates.
            //   ‘–’ or ‘-=’ Operator is used to remove the methods from the delegates list.
            //   multicasting of delegate should have a return type of Void otherwise it will throw a runtime exception. 
            //   Also, the multicasting of delegate will return the value only from the last method added in the multicast. Although, the other methods will be executed successfully.

            //Good example
            //You're an O/S, and I'm an application.I want to tell you to call one of my methods when you detect something happening. 
            //To do that, I pass you a delegate to the method of mine which I want you to call.
            //I don't call that method of mine myself, because I want you to call it when you detect the something. 
            //You don't call my method directly because you don't know (at your compile-time) that the method exists (I wasn't even written when you were built); 
            //instead, you call whichever method is specified by the delegate which you receive at run - time.

            LongRunningTask longRunningTask = new LongRunningTask();
            longRunningTask.Start(new CallbackDelegate(MyCallbackMthod));
        }

        private static void MyCallbackMthod(LongRunningTask t, TaskStatus status)
        {
            Console.WriteLine("The task status is {0}", status);
        }

        // what is the difference between IComparable and IComparer
        // What is extension methods

        private dynamic GetCount(string type)
        {
            if (type == "NUM")
                return 100;
            else if (type == "STRING")
                return "Hello World";
            else if (type == "TestClassWithIdGuid")
                return (new TestClass() { Name = "Ranjan", Id = Guid.NewGuid() });
            else
                return (new TestClass() { Name = "Ranjan", Id = 123 });
        }

        private IEnumerable<int> RunningTotal()
        {
            int runningTotal = 0;
            foreach (int number in this.numbers)
            {
                runningTotal += number;
                yield return runningTotal;
            }
        }

        private IEnumerable<int> Filter()
        {
            foreach (int number in this.numbers)
            {
                if (number > 3)
                    yield return number;
            }
        }

    }

    class LongRunningTask
    {
        public void Start(CallbackDelegate callback)
        {
            callback(this, TaskStatus.Started);

            // calculate PI to 1 billion digits
            for (int i=0;i<1000;i++)
            {
                callback(this, TaskStatus.StillProcessing);
            }

            callback(this, TaskStatus.Finished);
        }
    }


    public class TestClass
    {
        public string Name { get; set; }
        public dynamic Id { get; set; }
    }

    public class Galaxies
    {
        public IEnumerable<Galaxy> NextGalaxy
        {
            get
            {
                yield return new Galaxy { Name = "Tadpole", MegaLightYears = 400 };
                yield return new Galaxy { Name = "Pinwheel", MegaLightYears = 25 };
                yield return new Galaxy { Name = "Milky Way", MegaLightYears = 0 };
                yield return new Galaxy { Name = "Andromeda", MegaLightYears = 3 };
            }
        }
    }

    public class Galaxy
    {
        public String Name { get; set; }
        public int MegaLightYears { get; set; }
    }
}
