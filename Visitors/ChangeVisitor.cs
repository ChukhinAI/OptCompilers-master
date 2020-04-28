using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ChangeVisitor : AutoVisitor
    {
        public void ReplaceExpr(ExprNode from, ExprNode to)
        {
            var p = from.Parent;
            to.Parent = p;
            if (p is AssignNode assn)
            {
                assn.Expr = to;
            }
            else if (p is BinExprNode binopn)
            {
                if (binopn.Left == from) // Поиск подузла в Parent
                    binopn.Left = to;
                else if (binopn.Right == from)
                    binopn.Right = to;
            }
            else if (p is BlockNode)
            {
                throw new Exception("Родительский узел не содержит выражений");
            }
            else if (p is WhileNode wnode)
            {
                wnode.Condition = to;
            }
            else if (p is ForNode fnode)
            {
                fnode.End = to;
            }
            else if (p is IfNode inode)
            {
                inode.Condition = to;
            }
        }

        public void ReplaceStatement(StatementNode from, StatementNode to)
        {
            var p = from.Parent;
            if (p is AssignNode || p is ExprNode)
            {
                throw new Exception("Родительский узел не содержит операторов");
            }
            to.Parent = p;
            if (p is BlockNode bln)
            {
                for (var i = 0; i < bln.StList.Count; ++i)
                {
                    if (bln.StList[i] == from)
                    {
                        bln.StList[i] = to;
                        break;
                    }
                }
            }
            /*else if (p is IfNode ifn)
            {
                if (ifn.Stat == from)
                {
                    ifn.Stat = new BlockNode(to);
                }
                else if (ifn.ElseStat == from)
                {
                    ifn.ElseStat = new BlockNode(to);
                }
            }*/
            else
            {
                throw new Exception("ReplaceStatement не определен для данного типа узла");
            }
        }

        public void ReplaceBlock(BlockNode from, BlockNode to)
        {
            var p = from.Parent;
            if (p is IfNode ifn)
            {
                if (ifn.Stat == from)
                {
                    ifn.Stat = to;
                }
                else if (ifn.ElseStat == from)
                {
                    ifn.ElseStat = to;
                }
            }
            else if (p is ForNode fn)
            {
                fn.Stat = to;
            }
            else if (p is WhileNode wn)
            {
                wn.Stat = to;
            }
            else
            {
                throw new Exception("BlockNode не определен для данного типа узла");
            }
        }

        // замена в самом главном блокноде
        public void ReplaceNode(Node from, Node to)
        {
            var p = from.Parent;
            if (p is BlockNode bn)
            {
                for (var i = 0; i < bn.StList.Count; ++i)
                {
                    if (bn.StList[i] == from)
                    {
                        bn.StList[i] = to as StatementNode;
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("BlockNode не определен для данного типа узла");
            }
        }
    }
}
