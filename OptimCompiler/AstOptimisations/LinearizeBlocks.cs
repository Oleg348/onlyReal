using ProgramTree;
using SimpleLang.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.AstOptimisations
{
    public class LinearizeBlocks : AutoApplyVisitorInterface
    {
        public override void VisitBlockNode(BlockNode bl)
        {
            for (int i = 0; i < bl.StList.Count; i++)
            {
                if (bl.StList[i] is BlockNode)
                {
                    BlockNode bl1= (BlockNode)bl.StList[i];
                    SetApply();

                    bl.StList.RemoveAt(i);
                    for (int j = 0; j < bl1.StList.Count; j++)
                    {
                        bl.StList.Insert(i+j, bl1.StList[j]);
                    }
                }
                if (bl.StList[i] != null)
                    bl.StList[i].Visit(this);
            }    
        }
    }
}
