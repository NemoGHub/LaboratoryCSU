using Laboratoy;
using Laboratoy.classes;
using System;

///                                   
/// Лабораторная 5
///
/// Изучите что такое структура, класс, объект, конструктор, наследование. Создайте классы Token, Parenthesis, Number, Operation. 
/// Переделайте список токенов с List<object> на List<Token>.
/// После изменений, чтобы не писать длинные operation1.Symbol == operation2.Symbol и operation1.Priority > operation2.Priority
/// можно прочитать про перегрузку операторов и писать более кратко operation1 == operation2 и operation1 > operation2. 
/// 
///

namespace Laboratory
{
    public class Laboratory5
    {
        public static void Run()
        {
            Console.WriteLine("            =#==#==#=    Лабораторная 5    =#==#==#= ");
            Console.WriteLine("Ввод: ");
            string expression = Console.ReadLine();
            // Преобразуем строку с привычным математическим выражением в список токенов
            List<Token> expressionList = ParseToTokenList(expression);

            List<Token> reversedPolishNotation = ToRPN(expressionList);

            Console.WriteLine("Результат: " + CalclateRPN(reversedPolishNotation).number);
            Run();
        }

        public static List<Token> ParseToTokenList(string expression)
        {
            List<Token> expressionObject = new();
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
                            Laboratoy.classes.Number numericToken = new();
                            numericToken.SetToken(Convert.ToDouble(number));
                            expressionObject.Add(numericToken);
                        }
                        Operation operationToken = new();
                        operationToken.SetToken(Convert.ToChar(symbol));
                        expressionObject.Add((operationToken));

                        number = "";
                    }
                }
            }
            if (number != "")
            {
                Laboratoy.classes.Number numericToken = new();
                numericToken.SetToken(Convert.ToDouble(number));
                expressionObject.Add((numericToken));
            }
            return expressionObject;
        }

        // Превращаем в ОПЗ:
        static List<Token> ToRPN(List<Token> expression)
        {
            Stack<Token> operations = new(); // этот стек для операций
            List<Token> result = new();     // а список - для записи ОПЗ

            foreach (Token token in expression)
            {
                if (token is Laboratoy.classes.Number)
                {
                    result.Add(token);
                }
                if (token is Operation)
                {
                    while (operations.Count > 0 && FindOrderOfActions(operations.Peek()) >= FindOrderOfActions(token))
                    {
                        result.Add(operations.Pop());
                    }
                    operations.Push(token);
                }
                if (token.Equals('('))
                {
                    operations.Push(token);
                }
                if (token.Equals(')'))
                {
                    while (operations.Count > 0 && !operations.Peek().Equals('('))
                    {
                        result.Add(operations.Pop());
                    }
                    operations.Pop();
                }
            }

            while (operations.Count > 0)
            {
                result.Add(operations.Pop());
            }

            return result;
        }

        // Функция определяет порядок действий
        static int FindOrderOfActions(Token operation)
        {
            Operation unknownOperation = (Operation)operation;
            switch (unknownOperation.operation)
            {
                case '+':
                    return 1;
                case '-':
                    return 1;
                case '*':
                    return 2;
                case '/':
                    return 2;
                default:
                    return 0;
            }
        }


        // Эта функция считает значение выражения, записанного в виде ОПЗ
        static Laboratoy.classes.Number CalclateRPN(List<Token> rpn)
        {
            for (int i = 0; i < rpn.Count; i++)
            {
                if (rpn[i] is Operation)
                {
                    var num1 = (Laboratoy.classes.Number)rpn[i - 2]; // Происходит конфликт с системным классом, надёжнее указать полный путь
                    var num2 = (Laboratoy.classes.Number)rpn[i - 1];
                    var operation = (Operation)rpn[i];

                    double a = Convert.ToDouble(num1.number);  // Это математическая операция,  
                    double b = Convert.ToDouble(num2.number); // так что считаю допустимым исползовать имена a и b

                    // Обратимся к функции из предыдущей лабораторной, которая выполняет мат. операции, соответстыующие полученному чару 
                    // её тоже юыло бы неплохо перевести в объектно-ориентированный стиль, но тогда могут сломаться старые лабораторные
                    Console.WriteLine(a + " " + operation.operation + " " + num2.number);
                    double result = Laboratory.PerformOperation(a, b, operation.operation);

                    Laboratoy.classes.Number number = new(); 
                    number.SetToken(result);
                    
                    // Заменяем использованные числа на полученное значение
                    rpn.RemoveRange(i - 2, 3);
                    rpn.Insert(i - 2, number);
                    i -= 2;
                }
            }

            return (Laboratoy.classes.Number)rpn[0]; // rpn[0] - после цикла остаётся единственное число, которое и является ответом
        }

    }
}
