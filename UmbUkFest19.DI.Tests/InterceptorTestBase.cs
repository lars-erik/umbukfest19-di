using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Composing;
using Umbraco.Tests.TestHelpers;
using UmbUkFest19.DI.Tests.Interception;

namespace UmbUkFest19.DI.Tests
{
    public class InterceptorTestBase : BaseWebTest
    {
        protected Dictionary<string, Action<IRegister>> Uniques;
        protected IInterceptingRegister Interceptor;

        protected override void Compose()
        {
            base.Compose();
            CreateInterceptor();
        }

        protected void CreateInterceptor()
        {
            var decoratingInterceptor = new DecoratingRegistryInterceptor(Composition);
            Interceptor = DecorateInterceptor(decoratingInterceptor);
            Uniques = ExtractUniques();
            InterceptUniques(Uniques, Interceptor);
        }

        private static void InterceptUniques(Dictionary<string, Action<IRegister>> uniques, IRegister interceptor)
        {
            var keys = uniques.Keys.ToArray();
            foreach (var key in keys)
            {
                var existingFactory = uniques[key];
                uniques[key] = r => existingFactory(interceptor);
            }
        }

        private Dictionary<string, Action<IRegister>> ExtractUniques()
        {
            return (Dictionary<string, Action<IRegister>>)Composition
                .GetType()
                .GetField("_uniques", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(Composition);
        }

        protected virtual IInterceptingRegister DecorateInterceptor(IInterceptingRegister register)
        {
            return register;
        }
    }
}