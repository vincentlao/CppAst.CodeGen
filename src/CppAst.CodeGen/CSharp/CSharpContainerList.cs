﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CppAst.CodeGen.CSharp
{
    /// <summary>
    /// A generic list of <see cref="CSharpElement"/> hold by a <see cref="CSharpElement"/>
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    [DebuggerTypeProxy(typeof(CSharpContainerListDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class CSharpContainerList<TElement> : IList<TElement> where TElement : CSharpElement
    {
        private readonly ICSharpContainer _container;
        private readonly List<TElement> _elements;

        public CSharpContainerList(ICSharpContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            if (!(container is CSharpElement)) throw new ArgumentException($"Container must inherit from {nameof(CSharpElement)}", nameof(container));
            _elements = new List<TElement>();
        }

        /// <summary>
        /// Gets the container this list is attached to.
        /// </summary>
        public ICSharpContainer Container => _container;

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _elements).GetEnumerator();
        }

        public void Add(TElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (ReferenceEquals(item, _container)) throw new ArgumentException("Cannot add item to itself as a container owner");
            _container.ValidateMember(item);
            if (item.Parent != null)
            {
                throw new ArgumentException("The item belongs already to a container");
            }
            item.Parent = (CSharpElement)_container;
            _elements.Add(item);
        }

        public void Clear()
        {
            foreach (var element in _elements)
            {
                element.Parent = null;
            }

            _elements.Clear();
        }

        public bool Contains(TElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return _elements.Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(TElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_elements.Remove(item))
            {
                item.Parent = null;
                return true;
            }
            return false;
        }

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public int IndexOf(TElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return _elements.IndexOf(item);
        }

        public void Insert(int index, TElement item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (ReferenceEquals(item, _container)) throw new ArgumentException("Cannot add item to itself as a container owner");
            _container.ValidateMember(item);
            if (item.Parent != null)
            {
                throw new ArgumentException("The item belongs already to a container");
            }

            item.Parent = (CSharpElement)_container;
            _elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            var element = _elements[index];
            element.Parent = null;
            _elements.RemoveAt(index);
        }

        public TElement this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }
    }

    class CSharpContainerListDebugView<T>
    {
        private readonly ICollection<T> _collection;

        public CSharpContainerListDebugView(ICollection<T> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] array = new T[this._collection.Count];
                this._collection.CopyTo(array, 0);
                return array;
            }
        }
    }
}