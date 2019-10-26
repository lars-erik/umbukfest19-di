using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using UmbUkFest19.DI.Core.Decorators;
using UmbUkFest19.DI.Core.Interception;

namespace UmbUkFest19.DI.Core
{
    public class DecoratingComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            var interceptor = new DecoratingRegistryInterceptor(composition);
            interceptor.Decorate<IContentService, ContentServiceDecorator>();
        }
    }
}
