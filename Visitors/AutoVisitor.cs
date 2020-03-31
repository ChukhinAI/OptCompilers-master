using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    // базовая логика обхода без действий
    // Если нужны действия или другая логика обхода, то соответствующие методы надо переопределять
    // При переопределении методов для задания действий необходимо не забывать обходить подузлы
    public class AutoVisitor : Visitor
    {
        public override void VisitBinExprNode(BinExprNode binExpr)
        {
            binExpr.Left.Visit(this);
            binExpr.Right.Visit(this);
        }
        public override void VisitUnoExprNode(UnoExprNode unop)
        {
            unop.Expr.Visit(this);
        }

        public override void VisitLogicNotNode(LogicNotNode lnot)
        {
            lnot.LogExpr.Visit(this);
        }

        public override void VisitAssignNode(AssignNode a)
        {
            // для каких-то визиторов порядок может быть обратный - вначале обойти выражение, потом - идентификатор
            a.Id.Visit(this);
            a.Expr.Visit(this);
        }

        public override void VisitWhileNode(WhileNode c)
        {
            c.Condition.Visit(this);
            c.Stat.Visit(this);
        }

        public override void VisitForNode(ForNode c)
        {
            c.Begin.Visit(this);
            c.End.Visit(this);
            c.Counter.Visit(this);
            c.Stat.Visit(this);
        }

        public override void VisitIfNode(IfNode c)
        {
            c.Condition.Visit(this);
            c.Stat.Visit(this);
            if (c.ElseStat != null)
                c.ElseStat.Visit(this);
        }

        public override void VisitBlockNode(BlockNode bl)
        {
            for (var i = 0; i < bl.StList.Count; i++)
                bl.StList[i].Visit(this);
        }

    }
}
