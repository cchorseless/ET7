using System;
using System.Collections.Generic;

namespace ET
{
    [Serializable]
    public struct FValueTuple<A, B>: IEquatable<FValueTuple<A, B>>
    {
        public A Item1;
        public B Item2;

        public FValueTuple(A a, B b)
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
            if (obj is FValueTuple<A, B>)
                flag = this.Equals((FValueTuple<A, B>)obj);
            return flag;
        }

        public bool Equals(FValueTuple<A, B> other)
        {
            return Item1.Equals(other.Item1) && Item2.Equals(other.Item2);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode();
        }
    }

    [Serializable]
    public struct FValueTuple<A, B, C>: IEquatable<FValueTuple<A, B, C>>
    {
        public A Item1;
        public B Item2;
        public C Item3;

        public FValueTuple(A a, B b, C c)
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
            if (obj is FValueTuple<A, B, C>)
                flag = this.Equals((FValueTuple<A, B, C>)obj);
            return flag;
        }

        public bool Equals(FValueTuple<A, B, C> other)
        {
            return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() + this.Item2.GetHashCode() + this.Item3.GetHashCode();
        }
    }

    [Serializable]
    public struct FValueTuple<A, B, C, D>: IEquatable<FValueTuple<A, B, C, D>>
    {
        public A Item1;
        public B Item2;
        public C Item3;
        public D Item4;

        public FValueTuple(A a, B b, C c, D d)
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
            if (obj is FValueTuple<A, B, C, D>)
                flag = this.Equals((FValueTuple<A, B, C, D>)obj);
            return flag;
        }

        public bool Equals(FValueTuple<A, B, C, D> other)
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

    [Serializable]
    public struct FItemInfo: IEquatable<FItemInfo>
    {
        public int ItemConfigId;
        public int ItemCount;

        public FItemInfo(int a, int b)
        {
            ItemConfigId = a;
            ItemCount = b;
        }

        public bool Equals(FItemInfo other)
        {
            return ItemConfigId == other.ItemConfigId && ItemCount == other.ItemCount;
        }

        public override string ToString()
        {
            return "{" + "\"ItemConfigId\":" + ItemConfigId + ","
                    + "\"ItemCount\":" + ItemCount + "}";
        }
    }

    public static class GameStruct
    {
        static GameStruct()
        {
            MongoHelper.RegisterStruct<FValueTuple<int, int>>();
            MongoHelper.RegisterStruct<FValueTuple<int, long>>();
            MongoHelper.RegisterStruct<FValueTuple<int, string, bool>>();
            MongoHelper.RegisterStruct<FValueTuple<long, long, long, string>>();
            MongoHelper.RegisterStruct<FItemInfo>();
            // 先注册所有的结构体，再初始化
            MongoHelper.Init();
        }

        public static string ToListString(this List<FItemInfo> self)
        {
            var str = "";
            foreach (var item in self)
            {
                str += item + ",";
            }

            str = str.Substring(0, str.Length - 1);
            str = "[" + str + "]";
            return str;
        }

        public static void Init()
        {
        }
    }
}