using System;
using UnityEngine;
using Random = System.Random;

namespace KidsLearning
{
    public class ArithmeticGenerator : MonoBehaviour
    {
        private static readonly Random random = new Random();
        private Arithmetic _arithmetic;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GetRandomArithmeticProblem();
            }
        }

        public Arithmetic GetRandomArithmeticProblem()
        {
            _arithmetic = GenerateProblem();
            while (ShouldReGenerate(_arithmetic))
            {
                _arithmetic = GenerateProblem();
            }
            Debug.Log(_arithmetic.ToString());
            return _arithmetic;
        }

        private bool ShouldReGenerate(Arithmetic arithmetic)
        {
            return arithmetic.result < 0 || arithmetic.result > 20 ||
            !(Math.Abs(arithmetic.result % 1) < double.Epsilon) ||
            (arithmetic.result == 0 && arithmetic.operatorUsed == '/');
        }

        private Arithmetic GenerateProblem(char operatorUsed = ' ')
        {
            // Choose two random numbers between 1 and 9
            int num1 = random.Next(1, 10);
            int num2 = random.Next(1, 10);

            // Choose a random operator
            char op = operatorUsed;
            if (char.IsWhiteSpace(op))
            {
                op = GetRandomOperator();
            }

            // Calculate the result of the problem
            float result = CalculateResult(num1, num2, op);

            // Return the problem as a string in the form "num1 op num2 = result"
            return new Arithmetic(num1, op, num2, result);
        }

        private char GetRandomOperator()
        {
            // Choose a random number between 0 and 3
            int opIndex = random.Next(0, 4);

            // Map the random number to an operator
            switch (opIndex)
            {
                case 0:
                    return '+';
                case 1:
                    return '-';
                case 2:
                    return '*';
                case 3:
                    return '/';
                default:
                    throw new InvalidOperationException("Invalid operator index");
            }
        }

        private float CalculateResult(int num1, int num2, char op)
        {
            switch (op)
            {
                case '+':
                    return num1 + num2;
                case '-':
                    return num1 - num2;
                case '*':
                    return (float)num1 * num2;
                case '/':
                    return (float)num1 / num2;
                default:
                    throw new InvalidOperationException("Invalid operator");
            }
        }
    }
}
