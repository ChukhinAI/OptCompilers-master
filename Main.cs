using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.TACode.TacNodes;
using SimpleLang.Optimizers;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        #region tests
        static void SeveralOldTests(Parser parser) //31.03 andreya
        {
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
        static void CurrentTests(Parser parser) // 07.04 pervie 10 ballov
        {
            Console.WriteLine("Syntax tree before: \n");
            foreach (var st in parser.root.StList)
                Console.WriteLine(st);
            var pf = new FillParentVisitor();
            parser.root.Visit(pf);
            var ne = new NumberEqualityVisitor();
            parser.root.Visit(ne);
            var ifelse = new AlwaysIfOrElseVisitor();
            parser.root.Visit(ifelse);
            Console.WriteLine("After visitors: \n");
            foreach (var st in parser.root.StList)
                Console.WriteLine(st);
        }
        static void TacTests(Parser parser) // 14.04
        {
            var pf = new FillParentVisitor();
            parser.root.Visit(pf);
            //var ne = new NumberEqualityVisitor();
            //parser.root.Visit(ne);
            //var ifelse = new AlwaysIfOrElseVisitor();
            //parser.root.Visit(ifelse);
            var tac = new ThreeAddressCodeVisitor();
            parser.root.Visit(tac);
            
            Console.WriteLine("Tac code: \n");
            Console.WriteLine(tac.TACodeContainer.ToString());
        }
        static void TacConstantsAndCopies(Parser parser) // 14.04
        {
            var pf = new FillParentVisitor();
            parser.root.Visit(pf);
            var taco = new ThreeAddressCodeVisitor();
            parser.root.Visit(taco);

            var tacCode = taco.TACodeContainer;           
            Console.WriteLine("Tac code before: \n");
            Console.WriteLine(tacCode.ToString());
            var cc = new CopyAndConstantsOptimizer(tacCode);
            Console.WriteLine("Tac code after: \n");
            Console.WriteLine(tacCode.ToString());

        }
        #endregion
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
                    Console.WriteLine("Syntax tree was built: \n");
                    TacConstantsAndCopies(parser);
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

            Console.WriteLine("Done. Press any key to continue."); 
            Console.ReadKey();
        }

    }
}
