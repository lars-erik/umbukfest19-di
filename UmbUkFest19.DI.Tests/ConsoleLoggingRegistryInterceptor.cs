using System;
using Umbraco.Core.Composing;

namespace UmbUkFest19.DI.Tests
{
    public class ConsoleLoggingRegistryInterceptor : IRegister
    {
        private IRegister inner;

        public ConsoleLoggingRegistryInterceptor(IRegister inner)
        {
            this.inner = inner;
        }

        public void Register(Type serviceType, Lifetime lifetime = Lifetime.Transient)
        {
            Console.WriteLine($"Register({serviceType.Name}, {lifetime})");
            inner.Register(serviceType, lifetime);
        }

        public void Register(Type serviceType, Type implementingType, Lifetime lifetime = Lifetime.Transient)
        {
            Console.WriteLine($"Register({serviceType.Name}, {implementingType.Name}, {lifetime})");
            inner.Register(serviceType, implementingType, lifetime);
        }

        public void Register<TService>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            Console.WriteLine($"Register<{typeof(TService).Name}>(Func<IFactory, {typeof(TService).Name}> {factory}, {lifetime})");
            inner.Register(factory, lifetime);
        }

        public void Register(Type serviceType, object instance)
        {
            Console.WriteLine($"Register({serviceType.Name}, instance of {instance.GetType().Name})");
            inner.Register(serviceType, instance);
        }

        public void RegisterFor<TService, TTarget>(Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            Console.WriteLine($"RegisterFor<{typeof(TService).Name}, {typeof(TTarget).Name}>({lifetime})");
            inner.RegisterFor<TService, TTarget>(lifetime);
        }

        public void RegisterFor<TService, TTarget>(Type implementingType, Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            Console.WriteLine($"RegisterFor<{typeof(TService).Name}, {typeof(TTarget).Name}>({implementingType.Name}, {lifetime})");
            inner.RegisterFor<TService, TTarget>(implementingType, lifetime);
        }

        public void RegisterFor<TService, TTarget>(Func<IFactory, TService> factory, Lifetime lifetime = Lifetime.Transient) where TService : class
        {
            Console.WriteLine($"RegisterFor<{typeof(TService).Name}, {typeof(TTarget).Name}>({factory}, {lifetime})");
            inner.RegisterFor<TService, TTarget>(factory, lifetime);
        }

        public void RegisterFor<TService, TTarget>(TService instance) where TService : class
        {
            Console.WriteLine($"RegisterFor<{typeof(TService).Name}, {typeof(TTarget).Name}>(instance of {instance.GetType().Name})");
            inner.RegisterFor<TService, TTarget>(instance);
        }

        public void RegisterAuto(Type serviceBaseType)
        {
            Console.WriteLine($"RegisterAuto({serviceBaseType.Name})");
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