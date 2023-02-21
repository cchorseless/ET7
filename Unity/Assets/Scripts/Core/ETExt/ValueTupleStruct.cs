
namespace ET
{

    [Serializable]
    public struct ValueTupleStruct<A, B> : IEquatable<ValueTupleStruct<A, B>>
    {
        public A Item1;
        public B Item2;

        public ValueTupleStruct(A a, B b)
        {
            Item1 = a;
            Item2 = b;
        }
        public override string ToString()
        {
            return (Item1, Item2).ToString();
        }
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is ValueTupleStruct<A, B>)
                flag = this.Equals((ValueTupleStruct<A, B>)obj);
            return flag;
        }

        public bool Equals(ValueTupleStruct<A, B> other)
        {
            return Item1.Equals(other.Item1) && Item2.Equals(other.Item2);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode();
        }

    }

    [Serializable]
    public struct ValueTupleStruct<A, B, C> : IEquatable<ValueTupleStruct<A, B, C>>
    {
        public A Item1;
        public B Item2;
        public C Item3;

        public ValueTupleStruct(A a, B b, C c)
        {
            Item1 = a;
            Item2 = b;
            Item3 = c;
        }
        public override string ToString()
        {
            return (Item1, Item2, Item3).ToString();
        }
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is ValueTupleStruct<A, B, C>)
                flag = this.Equals((ValueTupleStruct<A, B, C>)obj);
            return flag;
        }

        public bool Equals(ValueTupleStruct<A, B, C> other)
        {
            return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode() + this.Item3.GetHashCode();
        }

    }

    [Serializable]
    public struct ValueTupleStruct<A, B, C, D> : IEquatable<ValueTupleStruct<A, B, C, D>>
    {
        public A Item1;
        public B Item2;
        public C Item3;
        public D Item4;

        public ValueTupleStruct(A a, B b, C c, D d)
        {
            Item1 = a;
            Item2 = b;
            Item3 = c;
            Item4 = d;
        }
        public override string ToString()
        {
            return (Item1, Item2, Item3, Item4).ToString();
        }
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is ValueTupleStruct<A, B, C, D>)
                flag = this.Equals((ValueTupleStruct<A, B, C, D>)obj);
            return flag;
        }

        public bool Equals(ValueTupleStruct<A, B, C, D> other)
        {
            return Item1.Equals(other.Item1) && 
                Item2.Equals(other.Item2) && 
                Item3.Equals(other.Item3) && 
                Item4.Equals(other.Item4);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode() + this.Item3.GetHashCode() + this.Item4.GetHashCode();
        }

    }
}
