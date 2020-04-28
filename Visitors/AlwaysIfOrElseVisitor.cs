using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class AlwaysIfOrElseVisitor : ChangeVisitor
    {
        public override void VisitIfNode(IfNode ifnode)
        {
            if (ifnode.Condition is BoolValNode expr)
            {
                if (expr.Val == true)
                {
                    ReplaceNode(ifnode, ifnode.Stat);
                }
                else
                {
                    ReplaceNode(ifnode, ifnode.ElseStat ?? new BlockNode(new EmptyNode()));
                    ifnode.Stat = null;
                }
                ifnode.Condition = null;
            }
            base.VisitIfNode(ifnode);           
        }

    }
}
