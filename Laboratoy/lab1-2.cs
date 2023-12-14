using System;
using System.Linq.Expressions;


// Лабораторная 1.
//                  Пользователь вводит строку с математическим выражением. Гарантируется, что выражение является корректным.
//                  В выражении могут быть только операции: +, -, *, /, - и целочисленные числа. Также в строке может быть неизвестное кол-во пробелов.
//                  Нужно создать два списка: в один список сохранит все числа из выражения, в другой - операции.
//                  Вывести на экран список чисел и список операций.
//
// Лабораторная 2.
//                  Вычислить выражение из предыдущей лабораторной. Не забыть про приоритет операций - умножение и деление вычисляются в первую очередь.
//


namespace Laboratory 
{
    public class Laboratory
    {
        public static void Run()
        {
            Console.WriteLine("                  =#=    Лабораторные 1+2    =#= ");
            Console.WriteLine("Ввод: ");

            string expression = Console.ReadLine();

            // Нужно создать два списка: в один список сохранит все числа из выражения, в другой - операции.
            List<double> doubles = ParseNumbers(expression);
            List<char> operations = ParseOperations(expression);

            Console.WriteLine("Лабораторная №1 :");

            foreach (double number in doubles)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine("");

            foreach (char operation in operations)
            {
                Console.Write(operation + " ");
            }
            Console.WriteLine("");

            Console.WriteLine("Лабораторная №2 :");
            Console.WriteLine(Calculate(doubles, operations));
        }

        // Функция разбирает ввод на числа
        static List<double> ParseNumbers(string expression)
        {
            List<double> result = new List<double>();
            string number = ""; // Для многоразрядных чисел
            foreach (char symbol in expression)
            {
                if (symbol != ' ')
                {
                    if (char.IsDigit(symbol))
                    {
                        number += symbol;
                    }
                    else
                    {
                        if (number != "")
                        {
                            result.Add(Convert.ToDouble(number));
                            number = "";
                        }
                    }
                }
            }
            if (number != "")
            {
                result.Add(Convert.ToDouble(number));
            }
            return result;
        }

        // Функция разбирает ввод на символы
        static List<char> ParseOperations(string expression)
        {
            List<char> result = new List<char>();
            foreach (char symbol in expression)
            {
                if (symbol != ' ' && !char.IsDigit(symbol))
                {
                    result.Add(symbol);
                }
            }
            return result;
        }

        // Лабораторная 2!
        // Эта страшная функция считает значение выражения
        public static double Calculate(List<double> numbers, List<char> operations)
        {
            while (operations.Count > 0)
            {
                if (operations.Contains('*') || operations.Contains('/'))
                {
                    int multiplicationIndex = operations.IndexOf('*');
                    int divisionIndex = operations.IndexOf('/');
                    int index; 

                    if (multiplicationIndex == -1)
                    {
                        index = divisionIndex; // Если в выражении нет умножения, то начинаем с индекса деления
                    }
                        
                    else if (divisionIndex == -1)
                    {
                        index = multiplicationIndex; // Если в выражении нет деления, то начинаем с индекса умножения
                    }
                        
                    else
                    {
                        index = Math.Min(divisionIndex, multiplicationIndex); // Если есть и то, и другое, то начинаем с минимального индекса
                    }
                        
                    // Выполнение операции
                    double newValue = PerformOperation(numbers[index], numbers[index + 1], operations[index]);
                    
                    // Заменяем использованные числа на полученное значение
                    numbers.RemoveAt(index + 1);  
                    numbers.RemoveAt(index);
                    numbers.Insert(index, newValue);

                    operations.RemoveAt(index); // Операция выполнена, вычеркиваем из списка
                }
                else // Если нет ни умножения, ни деления
                {
                    double next = PerformOperation(numbers[0], numbers[1], operations[0]); // Выполнение операции

                    numbers.RemoveAt(1);
                    numbers.RemoveAt(0);
                    operations.RemoveAt(0);

                    numbers.Insert(0, next);
                }
            }
            // Подготовим всё это к ретёрну
            return Convert.ToDouble(string.Join(" ", numbers));
        }

        // Эта функция определяет операцию и выполняет её 
        public static double PerformOperation(double a, double b, char operation) // Это математическая операция, так что считаю допустимым исползовать имена a и b
        {
            if (operation.Equals('+'))
            {
                return a + b;
            }
            else if (operation.Equals('-'))
            {
                return a - b;
            }
            else if (operation.Equals('*'))
            {
                return a * b;
            }
            else if (operation.Equals('/'))
            {
                return a / b;
            }
            else 
            {
                Console.WriteLine("Ошибка вычислений.");
                return 0;
            }
            // Нужно пользоваться свич-кейсом, переписать 
        }
    }
}
