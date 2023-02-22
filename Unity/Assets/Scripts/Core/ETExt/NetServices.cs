namespace ET
{
    public partial class NetServices
    {
        // gaiguo
        public void RefreshTypeOpcode()
        {
            typeOpcode.Clear();
            HashSet<Type> types = EventSystem.Instance.GetTypes(typeof (MessageAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (MessageAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                MessageAttribute messageAttribute = attrs[0] as MessageAttribute;
                if (messageAttribute == null)
                {
                    continue;
                }
                typeOpcode.Add(type, messageAttribute.Opcode);
            }
        }
    }
}

