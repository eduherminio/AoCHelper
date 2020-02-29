namespace AoCHelper.Model
{
    /// <summary>
    /// Simple node class, with equality operators overriden
    /// <See cref="TreeNode{TKey}"/> con TKey: string
    /// </summary>
    public class Node : TreeNode<string>
    {
        public Node(string id) : base(id)
        {
        }

        public Node(string id, Node child) : base(id, child)
        {
        }
    }
}
