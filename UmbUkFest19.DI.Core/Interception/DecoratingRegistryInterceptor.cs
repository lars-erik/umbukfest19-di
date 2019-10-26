using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Composing;

namespace UmbUkFest19.DI.Core.Interception
{
    public class DecoratingRegistryInterceptor : IInterceptingRegister
    {
        private IRegister inner;
        private List<(Type serviceType, Type implType)> decorators = new List<(Type serviceType, Type implType)>();

        protected Dictionary<string, Action<IRegister>> Uniques;

        public DecoratingRegistryInterceptor(IRegister inner)
        {
            this.inner = inner;

            Uniques = ExtractUniques();
            InterceptUniques(Uniques, inner);
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
            return (Dictionary<string, Action<IRegister>>)inner
                .GetType()
                .GetField("_uniques", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(inner);
        }



        public void Decorate<TService, TImplementation>()
        {
            decorators.Add((typeof(TService), typeof(TImplementation)));
        }

        public void Register(Type serviceType, Lifetime lifetime = Lifetime.Transient)
        {
            inner.Register(serviceType, lifetime);
        }

        public void Register(Type serviceType, Type implementingType, Lifetime lifetime = Lifetime.Transient)
        {
            if (decorators.Any(x => x.serviceType == serviceType))
            {
                var decorator = decorators.First(x => x.serviceType == serviceType);
                var methodTemplate = typeof(IRegister).GetMethods()
                    .First(x => x.Name == "RegisterFor" && x.GetGenericArguments().Length == 2 && x.GetParameters().Length == 2 && x.GetParameters()[0].ParameterType == typeof(Type));
                var method = methodTemplate.MakeGenericMethod(implementingType, decorator.implType);
                method.Invoke(inner, new object[] {implementingType, lifetime});
                inner.Register(serviceType, decorator.implType, lifetime);
            }
            else
            {
                inner.Register(serviceType, implementingType, lifetime);
            }
        }

        public void Register<TService>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            inner.Register(factory, lifetime);
        }

        public void Register(Type serviceType, object instance)
        {
            inner.Register(serviceType, instance);
        }

        public void RegisterFor<TService, TTarget>(Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            inner.RegisterFor<TService, TTarget>(lifetime);
        }

        public void RegisterFor<TService, TTarget>(Type implementingType, Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            inner.RegisterFor<TService, TTarget>(implementingType, lifetime);
        }

        public void RegisterFor<TService, TTarget>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            inner.RegisterFor<TService, TTarget>(factory, lifetime);
        }

        public void RegisterFor<TService, TTarget>(TService instance) where TService : class
        {
            inner.RegisterFor<TService, TTarget>(instance);
        }

        public void RegisterAuto(Type serviceBaseType)
        {
            inner.RegisterAuto(serviceBaseType);
        }

        public void ConfigureForWeb()
        {
            inner.ConfigureForWeb();
        }

        public IFactory CreateFactory()
        {
            return inner.CreateFactory();
        }

        public object Concrete => inner.Concrete;
    }
}