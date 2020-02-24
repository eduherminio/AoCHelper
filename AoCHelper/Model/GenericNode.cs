using System;

namespace AoCHelper.Model
{
    /// <summary>
    /// Simple node class, with equality operators overriden
    /// <See cref="TreeNode{TKey}"/>
    /// </summary>
    /// <typeparam name="TKey">Generic key</typeparam>
    public class GenericNode<TKey> : IEquatable<GenericNode<TKey>>
    {
        public TKey Id { get; set; }

        public GenericNode(TKey id)
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

            if (!(obj is GenericNode<TKey>))
            {
                return false;
            }

            return Equals((GenericNode<TKey>)obj);
        }

        public bool Equals(GenericNode<TKey> other)
        {
            if (other == null)
            {
                return false;
            }

            if (Id.GetType() == typeof(TKey))
            {
                return Id.Equals(other.Id);
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

        public bool Equals(TKey x, TKey y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode(TKey obj)
        {
            return obj.GetHashCode();
        }

        public static bool operator ==(GenericNode<TKey> node1, GenericNode<TKey> node2)
        {
            if (node1 is null)
            {
                return node2 is null;
            }

            return node1.Equals(node2);
        }

        public static bool operator !=(GenericNode<TKey> node1, GenericNode<TKey> node2)
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
