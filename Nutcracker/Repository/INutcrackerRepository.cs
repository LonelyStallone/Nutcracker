namespace Nutcracker
{
    internal interface INutcrackerRepository
    {
        bool TryGetValue(string key, out string data);

        void Set(string key, string data);
    }
}