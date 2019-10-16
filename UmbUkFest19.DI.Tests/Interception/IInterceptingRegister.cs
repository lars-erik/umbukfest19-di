using System;
using Umbraco.Core.Composing;

namespace UmbUkFest19.DI.Tests.Interception
{
    public interface IInterceptingRegister : IRegister
    {
        void Decorate<T, T1>();
    }
}