﻿using System;

namespace Hana.CodeAnalysis
{
    public sealed class Evaluator
    {
        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public ExpressionSyntax _root { get; }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            //Binary Expression
            //Number Expression

            if (node is LiteralExpressionSyntax n)
                return (int)n.LiteralToken.Value;

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.MultiplyToken)
                    return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.DivideToken)
                    return left / right;
                else
                    throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
            }

            if (node is ParenthesizedExpressionSyntax p)
                return EvaluateExpression(p.Expression);

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}