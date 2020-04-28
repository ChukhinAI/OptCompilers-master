using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public abstract class Visitor
    {
        public virtual void VisitIdNode(IdNode id) { }
        public virtual void VisitIntNumNode(IntNumNode num) { }
        public virtual void VisitBinExprNode(BinExprNode binexpr) { }
        public virtual void VisitAssignNode(AssignNode a) { }
        public virtual void VisitListExprNode(ListExprNode bl) { }
        public virtual void VisitWriteNode(WriteNode w) { }
        public virtual void VisitEmptyNode(EmptyNode w) { }
        public virtual void VisitFloatNumNode(FloatNumNode num) { }
        public virtual void VisitBoolNode(BoolValNode v) { }
        public virtual void VisitUnoExprNode(UnoExprNode u) { }
        public virtual void VisitForNode(ForNode f) { }
        public virtual void VisitWhileNode(WhileNode w) { }
        public virtual void VisitGotoNode(GotoNode gt) { }
        public virtual void VisitLabelNode(LabeledStatementNode gt) { }       
        public virtual void VisitIfNode(IfNode i) { }
        public virtual void VisitReadNode(ReadNode w) { }
        public virtual void VisitBlockNode(BlockNode bl) { }
        public virtual void VisitDeclarationNode(DeclarationNode bl) { }
        public virtual void VisitLogicNotNode(LogicNotNode lnot) { }
    }
}
