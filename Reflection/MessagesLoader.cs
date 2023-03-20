using System.Reflection;
using Dtwo.API.DofusBase.Network.Messages;
namespace Dtwo.API.DofusBase.Reflection
{
    public abstract class MessagesLoader<T> where T : DofusMessage
    {
        protected IDictionary<string, Func<T>> Messages => m_messages;
        private readonly Dictionary<string, Func<T>> m_messages = new();

        protected abstract Assembly m_assembly { get; }

        public bool InitializeMessages(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                LogManager.LogError($"Error on Initialize messages : file {filePath} not found", 1);
                return false;
            }

            string content = File.ReadAllText(filePath);
            List<DofusMessageBinding> bindings;
            try
            {
                bindings = Json.JSonSerializer<List<DofusMessageBinding>>.DeSerialize(content);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex.ToString());
                return false;
            }

            var asmTypes = m_assembly.GetExportedTypes();
            for (int i = 0; i < asmTypes.Length; i++)
            {
                var type = asmTypes[i];

                if (type.IsClass && type.IsAbstract == false && type.IsSubclassOf(typeof(T)))
                {
                    var binding = GetBinding(type, bindings);

                    if (binding == null) continue;

                    if (m_messages.ContainsKey(binding.Identifier)) // error
                    {
                        LogManager.LogWarning($"Error on MessageLoader.LoadMessage : The key {binding.Identifier} aleady exist");
                        continue;
                    }

                    ConstructorInfo? ctor = type.GetConstructor(Type.EmptyTypes);
                    if (ctor == null)
                    {
                        LogManager.LogWarning($"Error on MessageLoader.LoadMessage : The key {binding.Identifier} has no constructor");
                        continue;
                    }

                    m_messages.Add(binding.Identifier, ctor.CreateDelegate<Func<T>>());
                }
            }

            return true;
        }

        public T GetMessage(string str)
        {
            bool b = m_messages.TryGetValue(str, out var func);
            if (b == false)
            {
                return null;
            }

            return func();
        }

        private static DofusMessageBinding? GetBinding(Type messageType, List<DofusMessageBinding> bindings)
        {
            string str = messageType.Name;

            for (int i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];

                if (binding.ClassName == str)
                {
                    return binding;
                }
            }

            return null;
        }
    }
}
