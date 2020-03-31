using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            string FileName = @"..\..\a.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();

                // new 31.03.2020
                var r = parser.root;
                // Console.WriteLine(r);
                Console.WriteLine("Исходный текст программы");
                var printv = new PrettyPrintVisitor(true);
                r.Visit(printv);
                Console.WriteLine(printv.Text);
                Console.WriteLine("-------------------------------");
                // new 31.03.2020

                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Syntax tree was built");
                    //foreach (var st in parser.root.StList)
                    //Console.WriteLine(st);

                    // new 31.03.2020
                    var avis = new AssignCountVisitor();
                    parser.root.Visit(avis);
                    Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    Console.WriteLine("-------------------------------");
                    var operv = new OperatorCountVisitor();
                    parser.root.Visit(operv);
                    Console.WriteLine(operv.Result);

                    //var maxcv = new MaxOpExprVisitor();
                    //parser.root.Visit(maxcv);
                    //Console.WriteLine(maxcv.Result);


                    var maxdeepv = new MaxDeepCycleVistor();
                    parser.root.Visit(maxdeepv);
                    Console.WriteLine(maxdeepv.Result);
                    // new 31.03.2020
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }

            Console.ReadLine();
        }

    }
}
