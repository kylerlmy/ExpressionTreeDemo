using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");

            CreateExpressionTreeFromApi();
            ExpressionParse();

            Aggregate();

        }

        private static void CreateExpressionTreeFromApi()
        {
            // Creating a parameter expression.  
            ParameterExpression value = Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable.   
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            // Creating a label to jump to from a loop.  
            LabelTarget label = Expression.Label(typeof(int));

            // Creating a method body.  
            BlockExpression block = Expression.Block(
                // Adding a local variable.  
                new[] { result },
                // Assigning a constant to a local variable: result = 1  
                Expression.Assign(result, Expression.Constant(1)),
                    // Adding a loop.  
                    Expression.Loop(
                       // Adding a conditional block into the loop.  
                       Expression.IfThenElse(
                           // Condition: value > 1  
                           Expression.GreaterThan(value, Expression.Constant(1)),
                           // If true: result *= value --  
                           Expression.MultiplyAssign(result,
                               Expression.PostDecrementAssign(value)),
                           // If false, exit the loop and go to the label.  
                           Expression.Break(label, result)
                       ),
                   // Label to jump to.  
                   label
                )
            );

            // Compile and execute an expression tree.  
            var func = Expression.Lambda<Func<int, int>>(block, value).Compile();
            int factorial = func(5);

            Console.WriteLine(factorial);
            // Prints 120.  
        }

        private static void ExpressionParse()
        {
            // Add the following using directive to your code file:
            // using System.Linq.Expressions;  

            // Create an expression tree from a lambda Expression.  
            Expression<Func<int, bool>> exprTree = num => num < 5;

            // Decompose the expression tree.  
            ParameterExpression param = (ParameterExpression)exprTree.Parameters[0];
            BinaryExpression operation = (BinaryExpression)exprTree.Body;
            ParameterExpression left = (ParameterExpression)operation.Left;
            ConstantExpression right = (ConstantExpression)operation.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
                              param.Name, left.Name, operation.NodeType, right.Value);

            // This code produces the following output:  

            // Decomposed expression: num => num LessThan 5  
        }

        /// <summary>
        /// 将一个表达式编译成一个委托
        /// </summary>
        private static void Complile()
        {
            // Creating an expression tree.  
            Expression<Func<int, bool>> expr = num => num < 5;

            // Compiling the expression tree into a delegate.  
            Func<int, bool> result = expr.Compile();

            // Invoking the delegate and writing the result to the console.  
            Console.WriteLine(result(4));

            // Prints True.  

            // You can also use simplified syntax  
            // to compile and run an expression tree.  
            // The following line can replace two previous statements.  
            Console.WriteLine(expr.Compile()(4));

            // Also prints True. 
        }

        private static void Aggregate()
        {
            string sentence = "the quick brown fox jumps over the lazy dog";

            // Split the string into individual words.
            string[] words = sentence.Split(' ');

            // Prepend each word to the beginning of the 
            // new sentence to reverse the word order.
            string reversed = words.Aggregate((workingSentence, next) =>
                                                  next + " " + workingSentence);

            Console.WriteLine(reversed);
        }


        private static void Includes()
        {

        }

    }



}
