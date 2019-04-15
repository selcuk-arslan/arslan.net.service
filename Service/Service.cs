﻿using System;
using System.Collections.Generic;

namespace Arslan.Net
{
    public static class Service
    {
        internal static Func<Type, object> _resolver;
        internal static T Resolve<T>() => (T)_resolver(typeof(T));
        public static void Configure(Func<Type, object> resolver) => _resolver = resolver;
    }

    public struct Service<T> : IEquatable<Service<T>>, IEquatable<T>
    {
        private T _service;

        internal Service(T service = default) {
            _service = service;
        }

        public T Value {
            get {
                if (_service == null)
                    _service = Service.Resolve<T>();

                return _service;
            }
        }

        public override bool Equals(object other) {
            if (Value == null)
                return other == null;

            if (other == null)
                return false;

            if (other is Service<T> service)
                return Equals(service.Value, Value);

            if (other is T t)
                return Equals(t, Value);

            return false;
        }

        public bool Equals(Service<T> other) => Equals(other.Value);

        public bool Equals(T other) => EqualityComparer<T>.Default.Equals(Value, other);

        public override int GetHashCode() {
            var value = Value;
            return value == null ? 0 : value.GetHashCode();
        }

        public override string ToString() {
            var value = Value;
            return value == null ? default : value.ToString();
        }

        public static implicit operator Service<T>(T service) => new Service<T>(service);
        public static implicit operator T(Service<T> service) => service.Value;
    }
}
