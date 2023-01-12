
namespace KidsLearning
{
    public struct Arithmetic: IQuestion
    {
        public int num1;
        public int num2;
        public char operatorUsed;
        public float result;

        public Arithmetic(int num1, char operatorUsed, int num2, float result)
        {
            this.num1 = num1;
            this.num2 = num2;
            this.operatorUsed = operatorUsed;
            this.result = result;
        }

        public override string ToString()
        {
            return $"{num1} {operatorUsed} {num2} = {result}";
        }

        public string ToQuestionText(string answer)
        {
            return $"{num1} {operatorUsed} {num2} = {answer}";
        }
    }
}