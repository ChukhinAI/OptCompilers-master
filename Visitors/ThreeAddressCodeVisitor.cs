using System;
using System.Collections.Generic;
using System.Linq;
using ProgramTree;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Visitors
{

   public class ThreeAddressCodeVisitor : AutoVisitor
    {

       public ThreeAddressCode TACodeContainer { get; }

        public ThreeAddressCodeVisitor()
        {
            TACodeContainer = new ThreeAddressCode();
        }

       private string ManageTrivialCases(ExprNode node)
        {
            switch (node)
            {
                case IdNode idNode:
                    return idNode.ToString();

                case IntNumNode intNumNode:
                    return intNumNode.ToString();

                case BoolValNode boolNode:
                    return boolNode.ToString();
                
                case FloatNumNode doubleNode:
                    return doubleNode.ToString();

                default:
                    
                   return GenerateThreeAddressLine(node);
            }
        }
       public override void VisitAssignNode(AssignNode a)
        {

            string rightPartExpression = null;
            
            rightPartExpression = ManageTrivialCases(a.Expr);
            TACodeContainer.PushNode(new TacAssignmentNode()
            {
                LeftPartIdentifier = a.Id.Name,
                FirstOperand = rightPartExpression
            });
        }

       private string GenerateThreeAddressLine(ExprNode expression)
        {
            string label = null;
            
           switch (expression)
            {
                
               case IdNode idNode:
                {
                    return TACodeContainer.CreateAndPushIdNode(idNode, label);
                }
                case IntNumNode intNumNode:
                {
                    return TACodeContainer.CreateAndPushIntNumNode(intNumNode, label);
                }
                case BoolValNode boolNode:
                {
                    return TACodeContainer.CreateAndPushBoolNode(boolNode, label);
                }
                case UnoExprNode unOpNode:
                {
                    var unaryExp = ManageTrivialCases(unOpNode.Expr);
                    
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    TACodeContainer.PushNode(new TacAssignmentNode()
                    {
                        Label = label,
                        LeftPartIdentifier = tmpName,
                        FirstOperand = null,
                        Operation = unOpNode.OpType.ToString(),
                        SecondOperand = unaryExp
                    });
                    return tmpName;
                }
                
               case BinExprNode binOpNode:
                {                    
                    var leftPart = ManageTrivialCases(binOpNode.Left);
                    var rightPart = ManageTrivialCases(binOpNode.Right);

                        /*var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                         TACodeContainer.PushNode(new TacAssignmentNode()
                         {
                             Label = label,
                             LeftPartIdentifier = tmpName,
                             FirstOperand = leftPart,
                             Operation = binOpNode.OpType.ToString(),
                             SecondOperand = rightPart
                         });
                         return tmpName;*/
                        return binOpNode.ToString(); 
                    }
                case LogicNotNode logicNotNode:
                {
                    var unaryExp = ManageTrivialCases(logicNotNode.LogExpr);
                    
                    var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
                    TACodeContainer.PushNode(new TacAssignmentNode()
                    {
                        Label = label,
                        LeftPartIdentifier = tmpName,
                        FirstOperand = null,
                        Operation = "!",
                        SecondOperand = unaryExp
                    });
                    return tmpName;
                }
            }
            
           return default(string);
        }

        public override void VisitIfNode(IfNode ifnode)
        {
            var lastNodeBeforeGeneration = TACodeContainer.Last;

           var conditionalExpression = GenerateThreeAddressLine(ifnode.Condition);

           var mainIfBlockStartLabel = TmpNameManager.Instance.GenerateLabel();
            var exitingLabel = TmpNameManager.Instance.GenerateLabel();

            TACodeContainer.PushNode(new TacIfGotoNode()
            {
                Condition = conditionalExpression,
                TargetLabel = mainIfBlockStartLabel
            });

           ifnode.ElseStat?.Visit(this);

           TACodeContainer.PushNode(new TacGotoNode()
            {
                TargetLabel = exitingLabel
            });

           TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = mainIfBlockStartLabel,
                IsUtility = true
            });

           ifnode.Stat.Visit(this);

           TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = exitingLabel,
                IsUtility = true
            });
        }

        public override void VisitWhileNode(WhileNode whilenode)
        {
            var lastNodeBeforeGeneration = TACodeContainer.Last;

           var conditionalCheckLabel = TmpNameManager.Instance.GenerateLabel();
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = conditionalCheckLabel,
                IsUtility = true
            });

           var conditionalExpression = GenerateThreeAddressLine(whilenode.Condition);

           var endOfWhileStatementLabel = TmpNameManager.Instance.GenerateLabel();
            var startOfWhileBodyLabel = TmpNameManager.Instance.GenerateLabel();

           TACodeContainer.PushNode(new TacIfGotoNode()
            {
                Condition = conditionalExpression,
                TargetLabel = startOfWhileBodyLabel
            });

           TACodeContainer.PushNode(new TacGotoNode()
            {
                TargetLabel = endOfWhileStatementLabel
            });

           TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfWhileBodyLabel,
                IsUtility = true
            });

           whilenode.Stat.Visit(this);

           TACodeContainer.PushNode(new TacGotoNode()
            {
                TargetLabel = conditionalCheckLabel
            });

           TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = endOfWhileStatementLabel,
                IsUtility = true
            });
        }

        public override void VisitForNode(ForNode fornode)
        {
            var lastNodeBeforeGeneration = TACodeContainer.Last;

           fornode.Counter.Visit(this); 
           string conditionalExpression;

           var startOfForStatementLabel = TmpNameManager.Instance.GenerateLabel();
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = startOfForStatementLabel,
                IsUtility = true
            });

           fornode.Stat.Visit(this);
            TACodeContainer.PushNode(new TacAssignmentNode()
            {
                LeftPartIdentifier = fornode.Counter.Name,
               FirstOperand = fornode.Counter.Name, 
               Operation = "+",
                SecondOperand = "1"
            });

           switch (fornode.Counter)
            {
                case IdNode idNode:
                    conditionalExpression = idNode.Name;
                    break;
                /*case IntNumNode intNumNode:
                    conditionalExpression = intNumNode.Num.ToString();
                    break;*/
                default:
                    conditionalExpression = GenerateThreeAddressLine(fornode.Counter);
                    break;
            }
            
           var conditionalExpressionId = TmpNameManager.Instance.GenerateTmpVariableName();
            
            TACodeContainer.PushNode(new TacAssignmentNode()
            {
                LeftPartIdentifier = conditionalExpressionId,
                FirstOperand = fornode.Counter.Name, 
               Operation = "<",
                SecondOperand = conditionalExpression
            });
            
           TACodeContainer.PushNode(new TacIfGotoNode()
            {
                Condition = conditionalExpressionId,
                TargetLabel = startOfForStatementLabel
            });
        }

        public override void VisitEmptyNode(EmptyNode w)
        {
            TACodeContainer.CreateAndPushEmptyNode(w);
        }

        public override void VisitGotoNode(GotoNode gt)
        {
            TACodeContainer.PushNode(new TacGotoNode()
            {
                IsUtility = false,
                TargetLabel = "l" + gt.Label
            });
        }

        public override void VisitLabelNode(LabeledStatementNode l)
        {
            TACodeContainer.PushNode(new TacEmptyNode()
            {
                Label = "l" + l.Label
            });
        }

        public void Postprocess()
        {
            var nodesToRemove = new List<TacNode>();
            var currentNode = TACodeContainer.First;
            while (currentNode != null)
            {
                var next = currentNode.Next;

                if (next == null)
                {
                    currentNode = next;
                    continue;
                }
                if (currentNode.Value is TacEmptyNode && !(next.Value is TacEmptyNode))
                {
                    if (currentNode.Value.Label != null)
                    {
                        next.Value.Label = currentNode.Value.Label;
                        nodesToRemove.Add(currentNode.Value);
                    }
                }
                currentNode = next;
            }
            TACodeContainer.RemoveNodes(nodesToRemove);
        }
        
        public override string ToString() => TACodeContainer.ToString();
    }
}
