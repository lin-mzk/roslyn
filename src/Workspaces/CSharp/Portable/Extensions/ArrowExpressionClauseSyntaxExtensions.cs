﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace Microsoft.CodeAnalysis.CSharp.Extensions
{
    internal static class ArrowExpressionClauseSyntaxExtensions
    {
        public static bool TryConvertToBlock(
            this ArrowExpressionClauseSyntax arrowExpression,
            SyntaxToken semicolonToken,
            bool createReturnStatementForExpression,
            out BlockSyntax block)
        {
            if (arrowExpression.Expression.GetLeadingTrivia().Any(t => t.IsDirective))
            {
                block = null;
                return false;
            }

            var statement = ConvertToStatement(arrowExpression.Expression, semicolonToken, createReturnStatementForExpression);
            statement = statement.WithPrependedLeadingTrivia(arrowExpression.ArrowToken.TrailingTrivia);
            block = SyntaxFactory.Block(statement);
            return true;
        }

        private static StatementSyntax ConvertToStatement(
            ExpressionSyntax expression, 
            SyntaxToken semicolonToken, 
            bool createReturnStatementForExpression)
        {
            if (expression.IsKind(SyntaxKind.ThrowExpression))
            {
                var throwExpression = (ThrowExpressionSyntax)expression;
                return SyntaxFactory.ThrowStatement(throwExpression.ThrowKeyword, throwExpression.Expression, semicolonToken);
            }
            else if (createReturnStatementForExpression)
            {
                return SyntaxFactory.ReturnStatement(expression)
                                    .WithSemicolonToken(semicolonToken);
            }
            else
            {
                return SyntaxFactory.ExpressionStatement(expression)
                                    .WithSemicolonToken(semicolonToken);
            }
        }
    }
}
