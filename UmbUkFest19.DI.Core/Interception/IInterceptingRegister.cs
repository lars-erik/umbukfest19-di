using Umbraco.Core.Composing;

namespace UmbUkFest19.DI.Core.Interception
{
    public interface IInterceptingRegister : IRegister
    {
        void Decorate<T, T1>();
    }
}