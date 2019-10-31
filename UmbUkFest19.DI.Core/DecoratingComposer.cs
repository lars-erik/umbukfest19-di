using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using UmbUkFest19.DI.Core.Decorators;
using UmbUkFest19.DI.Core.Interception;

namespace UmbUkFest19.DI.Core
{
    public class FactoryComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<GoldCustomerStrategy>();
            composition.Register<SilverCustomerStrategy>();
            composition.Register<BronzeCustomerStrategy>();
            composition.Register<DefaultCustomerStrategy>();

            composition.Register<Func<string, ICustomerStrategy>>(factory => name => name switch
                {
                    "Gold" => factory.GetInstance<GoldCustomerStrategy>(),
                    "Silver" => factory.GetInstance<SilverCustomerStrategy>(),
                    "Bronze" => factory.GetInstance<BronzeCustomerStrategy>(),
                    _ => (ICustomerStrategy)factory.GetInstance<DefaultCustomerStrategy>()
                }
            );
        }

        private string typeCode;

        public ICustomerStrategy CreateStrategy(string name)
        {
            var strategyFactory = Current.Factory.GetInstance<Func<string, ICustomerStrategy>>();
            return strategyFactory(name);
        }

        public void BehaveAccordingToTypeCode()
        {
            var strategy = CreateStrategy(typeCode);
            strategy.Execute(this);
        }
    }

    public class DefaultCustomerStrategy : ICustomerStrategy
    {
        public void Execute(object thing)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICustomerStrategy
    {
        void Execute(object thing);
    }

    public class GoldCustomerStrategy : ICustomerStrategy
    {
        public void Execute(object thing)
        {
            throw new NotImplementedException();
        }
    }

    public class SilverCustomerStrategy : ICustomerStrategy
    {
        public void Execute(object thing)
        {
            throw new NotImplementedException();
        }
    }

    public class BronzeCustomerStrategy : ICustomerStrategy
    {
        public void Execute(object thing)
        {
            throw new NotImplementedException();
        }
    }
}
