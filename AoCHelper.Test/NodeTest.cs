using AoCHelper.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace AoCHelper.Test
{
    public class NodeTest
    {
        private class CustomNode : TreeNode<DateTime>
        {
            public CustomNode(DateTime id) : base(id)
            {
            }

            public CustomNode(DateTime id, TreeNode<DateTime> child) : base(id, child)
            {
            }
        }

        [Fact]
        public void Equal()
        {
            DateTime now = DateTime.Now;

            CustomNode a = new CustomNode(now);
            CustomNode b = new CustomNode(now);
            CustomNode c = new CustomNode(default);
            CustomNode d = new CustomNode(now.AddSeconds(1));

            Assert.Equal(a, b);
            Assert.NotEqual(a, c);
            Assert.NotEqual(a, d);
            Assert.NotEqual(c, d);

            HashSet<CustomNode> set = new HashSet<CustomNode>() { a };
            Assert.False(set.Add(b));
            Assert.True(set.Add(c));
            Assert.True(set.Add(d));
        }
    }
}
