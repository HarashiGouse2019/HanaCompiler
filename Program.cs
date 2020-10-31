﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hana.CodeAnalysis;
using Hana.CodeAnalysis.Binding;
using Hana.CodeAnalysis.Syntax;

namespace Hana
{
    internal static class Program
    {
        private static void Main()
        {
            var showTree = false;

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if(line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                    continue;
                } else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var binder = new Binder();
                var boundExpression = binder.BindExpression(syntaxTree.Root);
                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

                if (showTree)
                {

                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    ShowTreeGraph(syntaxTree.Root);
                    Console.ResetColor();
                }


                if (!diagnostics.Any())
                {
                    var evaluator = new Evaluator(boundExpression);
                    var result = evaluator.Evaluate();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ResetColor();
                }
            }
        }

        static void ShowTreeGraph(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                ShowTreeGraph(child, indent, child == lastChild);
            }
        }
    }
}