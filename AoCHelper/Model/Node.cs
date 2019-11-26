using System;

namespace AoCHelper.Model
{
    /// <summary>
    /// Simple node class, with equals method and equality operators overriden
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class Node<TKey> : IEquatable<Node<TKey>>
    {
        public TKey ParentId { get; set; }

        public TKey Id { get; set; }

        public Node(TKey id)
        {
            Id = id;
        }

        #region Equals override
        // https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1815-override-equals-and-operator-equals-on-value-types?view=vs-2017

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Node<TKey>))
            {
                return false;
            }

            return Equals((Node<TKey>)obj);
        }

        public bool Equals(Node<TKey> other)
        {
            if (other == null)
            {
                return false;
            }

            if (Id is string _id && other is Node<string> _other)
            {
                return _id == _other.Id;
            }
            else if (Id is int _id2 && other is Node<int> _other2)
            {
                return _id2 == _other2.Id;
            }
            else
            {
                throw new Exception("Wrong TKey in Node<TKey>");
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Node<TKey> node1, Node<TKey> node2)
        {
            if (node1 is null)
            {
                return node2 is null;
            }

            return node1.Equals(node2);
        }

        public static bool operator !=(Node<TKey> node1, Node<TKey> node2)
        {
            if (node1 is null)
            {
                return node2 is object;
            }

            return !node1.Equals(node2);
        }

        #endregion
    }
}
