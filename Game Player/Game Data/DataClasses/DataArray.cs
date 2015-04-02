using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class DataArray<T>
    {
        private T[] data = { default(T) };

        public DataArray(T item)
        {
            this.data = new T[] { default(T), item };
        }

        public DataArray(T[] data)
        {
            this.data = data;
        }

        public DataArray() { }

        public int Length
        {
            get { return data.Length; }
            set { Resize(value); }
        }

        public T this[int index]
        {
            get 
            {
                if (index + 1 > data.Length || index < 0)
                    return default(T);
                return data[index]; 
            }
            set 
            {
                if (index + 1 > data.Length || index < 0)
                    return;
                data[index] = value; 
            }
        }

        public void Resize(int size)
        {
            Array.Resize<T>(ref data, size);
        }

        public void Add(T item)
        {
            this.Length++;
            this[Length - 1] = item;
        }

        public DataArray<T> Clone()
        {
            return new DataArray<T>((T[])data.Clone());
        }

        public DataArray<T> DeepClone()
        {
            try
            {
                DataArray<T> nData = new DataArray<T>();
                foreach (T t in data)
                    nData.Add((T)((ICloneable)t).Clone());
                return nData;
            }
            catch
            {
                return Clone();
            }
        }

        /// <summary>
        /// A method used so that foreach can enumerate through the list..
        /// </summary>
        /// <returns>The enumerator</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// A class used to aid the enumerator
        /// </summary>
        public class Enumerator
        {
            int nIndex;
            DataArray<T> data;

            public Enumerator(DataArray<T> data)
            {
                this.data = data;
                nIndex = 0;
            }

            /// <summary>
            /// Enumerates once.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                nIndex++;
                return (nIndex < data.data.Length);
            }

            public T Current
            {
                get
                {
                    return (data.data[nIndex]);
                }
            }
        }
    }
}
