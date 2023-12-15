using Laboratoy;
using Laboratoy.classes;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

///                                   
/// Лабораторная 5
///
/// Изучите что такое структура, класс, объект, конструктор, наследование. Создайте классы Token, Parenthesis, Number, Operation. 
/// Переделайте список токенов с List<object> на List<Token>.
/// После изменений, чтобы не писать длинные operation1.Symbol == operation2.Symbol и operation1.Priority > operation2.Priority
/// можно прочитать про перегрузку операторов и писать более кратко operation1 == operation2 и operation1 > operation2. 
/// 
///

////// --- --- I --- --- ---- ---- I ####          НЕ ЗАКОНЧЕНА!          #### I --- --- ---- ---- I  --- --- \\\\\\
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

            Console.WriteLine("Результат: " + Convert.ToString(CalclateRPN(reversedPolishNotation)));
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
                Operation operationToken = new();
                operationToken.SetToken(Convert.ToChar(number));
                expressionObject.Add((operationToken));
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
                // if (token.Equals('+') || token.Equals('-') || token.Equals('*') || token.Equals('/'))
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
        static int FindOrderOfActions(object operation)
        {
            switch (operation)
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
        static Token CalclateRPN(List<Token> rpn)
        {
            for (int i = 0; i < rpn.Count; i++)
            {
                if (rpn[i] is Operation)
                {
                    double result = 0;
                    double a = Convert.ToDouble(rpn[i - 2]);  // Это математическая операция,  
                    double b = Convert.ToDouble(rpn[i - 1]); // так что считаю допустимым исползовать имена a и b

                    // Обратимся к функции из предыдущей лабораторной, которая выполняет мат. операции, соответстыующие полученному чару 
                    // её тоже юыло бы неплохо перевести в объектно-ориентированный стиль, но тогда сломаются старые лабораторные
                    result = Laboratory.PerformOperation(a, b, Convert.ToChar(rpn[i]));
                    Laboratoy.classes.Number number = new(); // Происходит конфликт с системным классом, надёжнее указать полный путь
                    number.SetToken(result);
                    
                    // Заменяем использованные числа на полученное значение
                    rpn.RemoveRange(i - 2, 3);
                    rpn.Insert(i - 2, number);
                    i -= 2;
                }
            }

            return rpn[0]; // rpn[0] - после цикла остаётся единственное число, которое и является ответом
        }

    }
}
