﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class PrettyPrintVisitor : Visitor
    {
        public string Text = "";
        private int Indent = 0;
        private bool isRoot = false;
        private bool IsDisableIndent = false;
        private bool IsInner = false;
        private bool isUnOp = false;

        public PrettyPrintVisitor(bool isRoot = false)
        {
            this.isRoot = isRoot;
        }

        private string IndentStr()
        {
            if (IsDisableIndent) return "";
            return new string(' ', Indent);
        }

        private void IndentPlus()
        {
            Indent += 2;
        }

        private void IndentMinus()
        {
            Indent -= 2;
        }

        public override void VisitIdNode(IdNode id)
        {
            Text += id.Name;
        }

        public override void VisitIntNumNode(IntNumNode num)
        {
            Text += num.Num.ToString();
        }

        public override void VisitBinExprNode(BinExprNode binexpr)
        {
            if (!isUnOp) Text += "(";
            binexpr.Left.Visit(this);
            Text += " " + binexpr.OpType + " ";
            binexpr.Right.Visit(this);
            Text += ")";
            isUnOp = false;
        }

        public override void VisitUnoExprNode(UnoExprNode unop)
        {

            if (unop.Expr is BinOpNode)
            {
                Text += "(";
                isUnOp = true;
            }

            Text += unop.Expr;
            unop.Expr.Visit(this);
        }
        public override void VisitAssignNode(AssignNode a)
        {
            bool prevInner = IsInner;
            if (prevInner)
                IndentPlus();
            Text += IndentStr();
            a.Id.Visit(this);
            Text += " = ";
            a.Expr.Visit(this);
            Text += ';';
            if (prevInner)
                IndentMinus();
            IsInner = prevInner;
        }
        public override void VisitWhileNode(WhileNode node)
        {
            bool prevInner = IsInner;

            if (prevInner)
                IndentPlus();

            Text += IndentStr() + "while (";
            IsDisableIndent = true;
            node.Condition.Visit(this);
            IsDisableIndent = false;
            Text += ")";
            Text += Environment.NewLine;
            IsInner = true;
            node.Stat.Visit(this);

            if (prevInner)
                IndentMinus();

            IsInner = prevInner;
        }

        public override void VisitForNode(ForNode node)
        {
            bool prevInner = IsInner;

            if (prevInner)
                IndentPlus();

            Text += IndentStr() + "for (";
            IsDisableIndent = true;
            node.Begin.Visit(this);
            node.End.Visit(this);
            Text += " to ";
            node.Counter.Visit(this);
            IsDisableIndent = false;
            Text += ")";
            Text += Environment.NewLine;
            IsInner = true;
            node.Stat.Visit(this);
            if (prevInner)
                IndentMinus();
            IsInner = prevInner;
        }

        public override void VisitIfNode(IfNode node)
        {
            bool prevInner = IsInner;
            if (prevInner)
                IndentPlus();
            IsInner = true;
            Text += IndentStr() + "if (";
            IsDisableIndent = true;
            node.Condition.Visit(this);
            IsDisableIndent = false;
            Text += ")";
            Text += Environment.NewLine;
            node.Stat.Visit(this);

            if (node.ElseStat != null)
            {
                Text += Environment.NewLine;
                Text += IndentStr() + "else ";
                Text += Environment.NewLine;
                node.ElseStat.Visit(this);
            }

            if (prevInner)
                IndentMinus();
            IsInner = prevInner;
        }

        public override void VisitBlockNode(BlockNode bl)
        {
            bool prevInner = IsInner;
            IsInner = false;
            var Count = bl.StList.Count;
            bool isFirstIndent = false;

            if (!isRoot)
            {
                Text += IndentStr() + "{" + Environment.NewLine;
                IndentPlus();
                isFirstIndent = true;
            }

            isRoot = false;

            if (Count > 0)
                bl.StList[0].Visit(this);

            for (var i = 1; i < Count; i++)
            {
                if (!(bl.StList[i] is EmptyNode))
                    Text += Environment.NewLine;
                bl.StList[i].Visit(this);
            }

            if (isFirstIndent)
            {
                IndentMinus();
                Text += Environment.NewLine + IndentStr() + "}";
            }

            IsInner = prevInner;
        }

        public override void VisitBoolNode(BoolValNode v) => Text += v.Val.ToString().ToLower();

        /*
        public override void VisitLabelNode(LabelNode l)
        {
            Text += l.ToString();
        }
        */

        public override void VisitGotoNode(GotoNode gt)
        {
            Text += gt.ToString();
        }

        public override void VisitLogicNotNode(LogicNotNode v)
        {
            Text += "!";
            v.LogExpr.Visit(this);
        }
    }
}
