namespace DesignPatterns.Reflection;

// An ordinary class with normal methods. No dispatcher logic, no switch statements,
// no registry of operations. Reflection will discover and invoke these at runtime.
internal class Calculator
{
    public double Add(double a, double b) => a + b;
    public double Subtract(double a, double b) => a - b;
    public double Multiply(double a, double b) => a * b;
    public double Divide(double a, double b)
        => b == 0 ? throw new DivideByZeroException() : a / b;
    public double Power(double a, double b) => Math.Pow(a, b);
    //public double Sqrt(double a) => Math.Sqrt(a);
}
