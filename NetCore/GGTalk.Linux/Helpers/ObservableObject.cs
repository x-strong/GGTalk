using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GGTalk
{
    internal class ObservableObject<T> : ObservableCollection<T> where T:class
    {        
        public ObservableObject() : base()
        {
        }

        public ObservableObject(List<T> list) : base(list)
        {

        }

        public ObservableObject(IEnumerable<T> collection) : base(collection)
        {

        }

        public bool Contains(Predicate<T> match)
        {
            foreach (T t in Items)
            {
                if (match(t))
                {
                    return true;
                }
            }
            return false;
        }

        public T Find(Predicate<T> match)
        {
            foreach (T t in Items)
            {
                if (match(t))
                {
                    return t;
                }
            }
            return default(T);
        }

        public ObservableCollection<T> FindAll(Predicate<T> match)
        {
            ObservableCollection<T> tempList = new ObservableCollection<T>();
            foreach (T t in Items)
            {
                if (match(t))
                {
                    tempList.Add(t);
                }
            }
            return tempList;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T t in collection)
            {
                Items.Add(t);
            }
        }


        /// <summary>
        /// 按定义的顺序插入到对应到位置
        /// </summary>
        /// <param name="entityObject">要比较到对象类型</param>
        /// <param name="comparison">表示比较同一类型到两个对象到方法</param>
        public void InsertObjectBySort(T entityObject, Comparison<T> comparison)
        {
            if (this.Count == 0)
            {
                Add(entityObject);
            }
            else
            {
                bool isInsret = false;
                for (int i = 0; i < this.Count; i++)
                {
                    if (comparison(this[i],entityObject)>=0)
                    {
                        InsertItem(i, entityObject);
                        isInsret = true;
                        break;
                    }
                }
                if (!isInsret)
                {
                    Add(entityObject);
                }
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            QuickSort(Items, 0, Items.Count - 1, comparison);
        }

        public static void QuickSort(IList<T> _array, int startIndex, int endIndex, Comparison<T> comparison)
        {
            if (startIndex < endIndex)
            {
                T tempT = _array[startIndex];//指定一位基准数
                int i = startIndex;
                int j = endIndex;
                while (i < j)
                {
                    while (i < j)
                    {
                        if (comparison(_array[j], tempT) <= 0)
                        {
                            _array[i] = _array[j];
                            i++;
                            break;
                        }
                        else
                        {
                            j--;
                        }
                    }
                    while (i < j)
                    {
                        if (comparison(_array[i], tempT) > 0)
                        {
                            _array[j] = _array[i];
                            j--;
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                _array[i] = tempT;
                ObservableObject<T>.QuickSort(_array, startIndex, i - 1, comparison);
                ObservableObject<T>.QuickSort(_array, i + 1, endIndex, comparison);
            }
        }

        

    }
}
