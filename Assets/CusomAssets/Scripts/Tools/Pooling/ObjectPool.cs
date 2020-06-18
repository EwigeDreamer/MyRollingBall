using System;
using System.Collections.Concurrent;

namespace MyTools.Pooling
{
    public class ObjectPool<T> where T : class
    {
#pragma warning disable 649
        ConcurrentBag<T> objects;
        Func<ObjectPool<T>, T> generate;
        Action<T> active;
        Action<T> deactive;
#pragma warning restore 649

        public ObjectPool(Func<ObjectPool<T>, T> generate, Action<T> active, Action<T> deactive, int count = 0)
        {
            this.generate = generate ?? throw new ArgumentNullException(nameof(generate));
            this.active = active = active ?? delegate { };
            this.deactive = deactive = deactive ?? delegate { };
            var objects = this.objects = new ConcurrentBag<T>();
            for (int i = 0; i < count; ++i)
            {
                var obj = generate(this);
                deactive(obj);
                this.objects.Add(obj);
            }
        }

        public T Take(Action<T> setup = null)
        {
            if (!this.objects.TryTake(out var obj)) obj = this.generate(this);
            setup?.Invoke(obj);
            this.active(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (obj == null) return;
            this.deactive(obj);
            this.objects.Add(obj);
        }
    }
}