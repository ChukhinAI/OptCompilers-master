using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class MaxOpExprVisitor : AutoVisitor
    {
        public int MaxCount = 0;

        public string Result
        {
            get { return $"Максимальное количество операций: = {MaxCount}"; }
        }

        private int GetOpCountBinExprNode(BinExprNode binexpr)
        {
            int res = 1;

            if (binexpr.Left is BinExprNode)
                res += GetOpCountBinExprNode(binexpr.Left as BinExprNode);

            if (binexpr.Right is BinExprNode)
                res += GetOpCountBinExprNode(binexpr.Right as BinExprNode);

            return res;
        }

        public override void VisitBinExprNode(BinExprNode binexpr)
        {
            int count = GetOpCountBinExprNode(binexpr);

            if (count > MaxCount)
                MaxCount = count;
        }
    }
}
