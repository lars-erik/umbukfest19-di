using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Composing;
using Umbraco.Tests.TestHelpers;
using UmbUkFest19.DI.Core.Interception;
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
        }

        protected virtual IInterceptingRegister DecorateInterceptor(IInterceptingRegister register)
        {
            return register;
        }
    }
}