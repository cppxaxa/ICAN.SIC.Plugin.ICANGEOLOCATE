using System;
using System.Threading.Tasks;

namespace GeoSharp.KDTree
{
    public class KDTree<T>
        where T : IKDComparator<T>
    {
        private KDNode<T> Root;

        public KDTree(T[] Items)
        {
            Root = CreateTree(Items, 0, Items.Length, 0);
        }

        public bool IsEmpty { get => Root == null? true: false; }

        public T FindNearest(T Search)
        {
            return FindNearest(Root, Search, 0).Location;
        }

        // Only ever goes to log2(items.length) depth so lack of tail recursion is a non-issue
        private KDNode<T> CreateTree(T[] Items, int Start, int End, int Depth)
        {
            if (Start >= End)
                return null;

            Array.Sort(Items, Start, End - Start, Items[0].Comparator((Axis)(Depth % 3)));
            int Index = Start + ((End - Start) / 2);

            KDNode<T> Left = null, Right = null;

            if (Depth < 4)
            {
                Parallel.For(0, 2, (i) =>
                {
                    if (i % 2 == 0)
                        Left = CreateTree(Items, Start, Index, Depth + 1);
                    else
                        Right = CreateTree(Items, Index + 1, End, Depth + 1);
                });
            }
            else
            {
                Left = CreateTree(Items, Start, Index, Depth + 1);
                Right = CreateTree(Items, Index + 1, End, Depth + 1);
            }

            return new KDNode<T>(Left, Right, Items[Index]);
        }

        private KDNode<T> FindNearest(KDNode<T> Node, T Search, int Depth)
        {
            int Direction = Search.Comparator((Axis)(Depth % 3)).Compare(Search, Node.Location);
            KDNode<T> Next = (Direction < 0) ? Node.Left : Node.Right;
            KDNode<T> Other = (Direction < 0) ? Node.Right : Node.Left;
            KDNode<T> Best = (Next == null) ? Node : FindNearest(Next, Search, Depth + 1);
            if (Node.Location.SquaredDistance(Search) < Best.Location.SquaredDistance(Search))
            {
                Best = Node;
            }

            if (Other != null)
            {
                if (Other.Location.AxisSquaredDistance(Search, (Axis)(Depth % 3)) < Best.Location.SquaredDistance(Search))
                {
                    KDNode<T> PossibleBest = FindNearest(Other, Search, Depth + 1);
                    if (PossibleBest.Location.SquaredDistance(Search) < Best.Location.SquaredDistance(Search))
                    {
                        Best = PossibleBest;
                    }
                }
            }

            return Best;
        }
    }
}
