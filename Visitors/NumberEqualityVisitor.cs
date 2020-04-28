using ProgramTree;

namespace SimpleLang.Visitors
{
    class NumberEqualityVisitor : ChangeVisitor
    {
        public override void VisitBinExprNode(BinExprNode binExpr)
        {
            base.VisitBinExprNode(binExpr);
            if (binExpr.Left is IntNumNode left && binExpr.Right is IntNumNode right)
            {
                if (binExpr.OpType == BinOpType.Equal)
                    ReplaceExpr(binExpr, new BoolValNode(left.Num == right.Num));
                if (binExpr.OpType == BinOpType.NotEqual)
                    ReplaceExpr(binExpr, new BoolValNode(left.Num != right.Num));
            } 
        }
    }
}
