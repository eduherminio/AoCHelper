using System.Collections.Generic;
using System.Linq;

namespace AoCHelper.Model
{
    /// <summary>
    /// Tree: undirected graph in which any two vertices are connected by exactly one path,
    /// or equivalently a connected acyclic undirected graph.
    /// That's to say, it can be transversed recursively due its lack of cycles: a node only has one parent
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class TreeNode<TKey> : GenericNode<TKey>
    {
        public TKey ParentId { get; set; }

        /// <summary>
        /// Direct descendants
        /// </summary>
        public ICollection<TreeNode<TKey>> Children { get; set; } = new HashSet<TreeNode<TKey>>();

        public TreeNode(TKey id) : base(id)
        {
        }

        /// <summary>
        /// Initialize with one of its children
        /// </summary>
        /// <param name="id"></param>
        /// <param name="child">One of their descendants</param>
        public TreeNode(TKey id, TreeNode<TKey> child) : base(id)
        {
            Children.Add(child);
        }

        /// <summary>
        /// Number of descendants
        /// </summary>
        /// <returns></returns>
        public int DescendantsCount()
        {
            return Children.Count
                + Children.Select(child => child.DescendantsCount()).Sum();
        }

        /// <summary>
        /// Number of children of the children of (the children of) my children
        /// Equals to <see cref="DescendantsCount()"/>
        /// </summary>
        /// <returns></returns>
        public int GrandChildrenCount() => DescendantsCount();

        /// <summary>
        /// Number of relationships between this node and its descendants
        /// </summary>
        /// <returns></returns>
        public int RelationshipsCount()
        {
            return Children.Count
                   + Children.Select(child => child.DescendantsCount()).Sum()
                   + Children.Select(child => child.RelationshipsCount()).Sum();
        }

        /// <summary>
        /// Distance to childNode.
        /// int.MaxValue if childNode is not among this node descendants
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="initialDistance"></param>
        /// <returns></returns>
        public int DistanceTo(TreeNode<TKey> childNode, int initialDistance)
        {
            if (Children.Contains(childNode))
            {
                return ++initialDistance;
            }
            else
            {
                int existingDistance = Children.Any()
                    ? Children.Min(child =>
                        child.DistanceTo(childNode, initialDistance))
                    : int.MaxValue;

                return existingDistance == int.MaxValue
                    ? int.MaxValue
                    : ++existingDistance;
            }
        }
    }
}
