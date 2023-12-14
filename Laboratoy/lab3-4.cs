using System;
using System.Linq.Expressions;


// Лабораторная 3. - Полный провал,  мне не удалось придумать приемлемый алгоритм, поэтому просто воспользуюсь ОПЗ.
//                   Но, по сути, выполнение четвертой выполняет задачи, поставленные в третьей?
//        В выражении теперь могут быть числа с плавающей запятой и скобки.
//        Переписать программу так, чтобы выражение вычислялось.
// 
// Лабораторная 4.
//        Изучить, что такое обратная польская запись (ОПЗ).
//        Релизовать алгоритм вычисления результата математического выражения с ее использованием.
//        Алгоритм должен выглядеть следующим образом:
//         - Преобразуем строку с привычным математическим выражением (например, 1+20*3) в список токенов - логически значимых элементов в выражении
//             (например, [1, +, 20, *, 3]). Порядок токенов тот же, что был в изначальном выражении
//         - Список токенов преобразовать в токены ОПЗ (например, [1, 20, 3, *+])
//         - На основе списка токенов, записанных в ОПЗ, вычислить результат выражения
//    
//


namespace Laboratory 
{
    internal class Laboratory_3_4
    {
        public static void Run()
        {
            Console.WriteLine("               =#==#=    Лабораторные 3 и 4    =#==#= ");
            Console.WriteLine("Ввод: ");
            string expression = Console.ReadLine();
            // Преобразуем строку с привычным математическим выражением в список токенов
            List<object> expressionList = ParseToObject(expression);

            // Список токенов преобразовать в токены ОПЗ (например, [1, 20, 3, *+])
            List<object> reversedPolishNotation = ToRPN(expressionList);
            Console.WriteLine("ОПЗ: " + string.Join(" ", reversedPolishNotation));

            // На основе списка токенов, записанных в ОПЗ, вычислить результат выражения
            Console.WriteLine("Результат: " + CalclateRPN(reversedPolishNotation));
            Run();
        }


        // Функция разбирает ввод пользователя на числа и символы, заиписывая в объект
        // Преобразуем строку с привычным математическим выражением (например, 1+20*3) в список токенов - логически значимых элементов в выражении
        //             (например, [1, +, 20, *, 3]). Порядок токенов тот же, что был в изначальном выражении
        static List<object> ParseToObject(string expression)
        {
            List<object> expressionObject = new List<object>();
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
                            expressionObject.Add(number);
                        }
                        expressionObject.Add((symbol));
                        number = "";
                    }
                }
            }
            if (number != "")
            {
                expressionObject.Add(number);
            }
            return expressionObject;
        }

        // Превращаем в ОПЗ:
        static List<object> ToRPN(List<object> expression)
        {
            Stack<object> operations = new Stack<object>(); // этот стек для операций
            List<object> result = new List<object>();      // а список - для записи ОПЗ

            foreach (object token in expression)
            {
                if (token is string)
                {
                    result.Add(token);
                }
                if (token.Equals('+') || token.Equals('-') || token.Equals('*') || token.Equals('/'))
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
        static double CalclateRPN(List<object> rpn)
        { 
            for (int i = 0; i < rpn.Count; i++)
            {
                if (rpn[i] is char) 
                {
                    double result = 0;
                    double a = Convert.ToDouble(rpn[i - 2]);  // Это математическая операция,  
                    double b = Convert.ToDouble(rpn[i - 1]); // так что считаю допустимым исползовать имена a и b

                    // Обратимся к функции из предыдущей лабораторной, которая выполняет мат. операции, соответстыующие полученному чару 
                    result = Laboratory.PerformOperation(a, b, (char)rpn[i]);

                    // Заменяем использованные числа на полученное значение
                    rpn.RemoveRange(i - 2, 3);
                    rpn.Insert(i - 2, result);
                    i -= 2;
                }
            }
             
            return Convert.ToDouble(rpn[0]); // rpn[0] - после цикла остаётся единственное число, которое и является ответом
        }


    }
}
