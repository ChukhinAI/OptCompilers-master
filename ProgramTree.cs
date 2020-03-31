// fiit_mag_1.5_2019-2021_gg
using System.Collections.Generic;
using SimpleLang.Visitors;
using System.Text;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public enum BinOpType { Or, And, Plus, Minus, Prod, Div, Less, LessOrEqual, Greater, GreaterOrEqual, Equal, NotEqual };

    public enum UnoOpType { Minus, Not };

    public enum VariableType { Int, Float, Bool }

    public abstract class Node // базовый класс для всех узлов    
    {
        public abstract void Visit(Visitor v);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
    }


public abstract class BinOpNode : ExprNode
{

}

public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }

        public override void Visit(Visitor v)
        {
            v.VisitIdNode(this);
        }
}

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }

        public override void Visit(Visitor v)
        {
            v.VisitIntNumNode(this);
        }
    }

    public class FloatNumNode : ExprNode
    {
        public double Num { get; set; }
        public FloatNumNode(double num) { Num = num; }

        public override void Visit(Visitor v)
        {
            v.VisitFloatNumNode(this);
        }
        public override string ToString() => Num.ToString();
    }

    public class BoolValNode : ExprNode
    {
        public bool Val { get; set; }
        public BoolValNode(bool val) { Val = val; }

        public override void Visit(Visitor v)
        {
            v.VisitBoolNode(this);
        }
        public override string ToString() => Val.ToString();
    }

    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class ListExprNode : Node
    {
        public List<ExprNode> ExprList = new List<ExprNode>();
        public ListExprNode(ExprNode expr)
        {
            ExprList.Add(expr);
        }

        public void Add(ExprNode expr)
        {
            ExprList.Add(expr);
        }

        public override void Visit(Visitor v)
        {
            v.VisitListExprNode(this);
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var st in ExprList)
                s.Append(st.ToString() + ";\n");
            return s.ToString();
        }
    }

    public class BinExprNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public BinOpType OpType { get; set; }
        public BinExprNode(ExprNode left, ExprNode right, BinOpType type)
        {
            Left = left;
            Right = right;
            OpType = type;
        }

        public override void Visit(Visitor v)
        {
            v.VisitBinExprNode(this);
        }
    }

    public class UnoExprNode : ExprNode
    {
        public ExprNode Expr { get; set; }
        public UnoOpType OpType { get; set; }
        public UnoExprNode(ExprNode expr, UnoOpType type)
        {
            Expr = expr;
            OpType = type;
        }

        public override void Visit(Visitor v)
        {
            v.VisitUnoExprNode(this);
        }
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }

        public override void Visit(Visitor v)
        {
            v.VisitAssignNode(this);
        }
        public override string ToString() => Id + " = " + Expr;
    }

    public class ForNode : StatementNode
    {
        public IdNode Counter { get; set; }
        public ExprNode Begin { get; set; }
        public ExprNode End { get; set; }
        public BlockNode Stat { get; set; }
        public ForNode(IdNode counter, ExprNode begin, ExprNode end, BlockNode stat)
        {
            Counter = counter;
            Begin = begin;
            End = end;
            Stat = stat;
        }

        public override void Visit(Visitor v)
        {
            v.VisitForNode(this);
        }
        public override string ToString()
        {
            return "for " + Counter + " = " + Begin + " .. " + "end" + ")\n" + Stat; // вроде так
        }
    }

    public class WhileNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public BlockNode Stat { get; set; }
        public WhileNode(ExprNode condition, BlockNode stat)
        {
            Condition = condition;
            Stat = stat;
        }

        public override void Visit(Visitor v)
        {
            v.VisitWhileNode(this);
        }
        public override string ToString()
        {
            return "while(" + Condition + ")\n" + Stat;
        }
    }

    public class GotoNode : StatementNode
    {
        public int Label { get; set; }
        public GotoNode(int label)
        {
            Label = label;
        }

        public override void Visit(Visitor v)
        {
            v.VisitGotoNode(this);
        }
        public override string ToString()
        {
            return "goto " + Label;
        }
    }

    /*
    public class LabeledStatementNode : StatementNode
    {
        public int Label { get; set; }
        StatementNode Stat { get; set; }
        public LabeledStatementNode(int label, StatementNode stat)
        {
            Label = label;
            Stat = stat;
        }
    }
    */

    public class IfNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public BlockNode Stat { get; set; }
        public BlockNode ElseStat { get; set; }
        public IfNode(ExprNode condition, BlockNode stat)
        {
            Condition = condition;
            Stat = stat;
        }

        public IfNode(ExprNode condition, BlockNode stat, BlockNode elseStat)
        {
            Condition = condition;
            Stat = stat;
            ElseStat = elseStat;
        }

        public override void Visit(Visitor v)
        {
            v.VisitIfNode(this);
        }
        public override string ToString()
        {
            string str = "if(" + Condition + ")\n" + Stat;
            if (ElseStat != null)
                str += "\nelse\n" + ElseStat;
            return str;
        }
    }

    public class WriteNode : StatementNode
    {
        public ListExprNode ExprList;
        public bool WriteLine { get; set; }

        public WriteNode(ListExprNode exprList, bool writeLine)
        {
            ExprList = exprList;
            WriteLine = writeLine;
        }

        public override void Visit(Visitor v)
        {
            v.VisitWriteNode(this);
        }
    }

    public class ReadNode : StatementNode
    {
        public IdNode Ident { get; set; }
        public ReadNode(IdNode ident)
        {
            Ident = ident;
        }

        public override void Visit(Visitor v)
        {
            v.VisitReadNode(this);
        }
    }

    public class LogicNotNode : ExprNode
    {
        public ExprNode LogExpr { get; set; }
        public SimpleParser.Tokens Operation { get; set; }
        public LogicNotNode(ExprNode LogExpr)
        {
            this.LogExpr = LogExpr;
        }
        public override void Visit(Visitor v)
        {
            v.VisitLogicNotNode(this);
        }
        public override string ToString()
        {
            return "!" + LogExpr.ToString();
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public override void Visit(Visitor v)
        {
            v.VisitBlockNode(this);
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var st in StList)
                s.Append(st.ToString() + ";\n");
            return s.ToString();
        }
    }

    public class DeclarationNode : StatementNode
    {
        public List<IdNode> IdentList { get; set; }

        public VariableType Type { get; set; }

        public DeclarationNode(List<IdNode> identList, VariableType type)
        {
            IdentList = identList;
            Type = type;
        }

        public override void Visit(Visitor v)
        {
            v.VisitDeclarationNode(this);
        }
    }

    public class EmptyNode : StatementNode
    {
        public override void Visit(Visitor v)
        {
            v.VisitEmptyNode(this);
        }
        public override string ToString() => "";
    }
}
