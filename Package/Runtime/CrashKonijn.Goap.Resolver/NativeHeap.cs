using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CrashKonijn.Goap.Resolver
{
    using static UnsafeUtility;

    public struct NativeHeapIndex
    {
        internal int TableIndex;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal int Version;
        internal int StructureId;
#endif
    }

    /// <summary>
    ///     This is a basic implementation of the MinHeap/MaxHeap data structure.  It allows you
    ///     to insert objects into the container with a O(log(n)) cost per item, and it allows you
    ///     to extract the min/max from the container with a O(log(n)) cost per item.
    ///     This implementation provides the ability to remove items from the middle of the container
    ///     as well.  This is a critical operation when implementing algorithms like a-star.  When an
    ///     item is added to the container, an index is returned which can be used to later remove
    ///     the item no matter where it is in the heap, for the same cost of removing it if it was
    ///     popped normally.
    ///     This container is parameterized with a comparator type that defines the ordering of the
    ///     container.  The default form of the comparator can be used, or you can specify your own.
    ///     The item that comes first in the ordering is the one that will be returned by the Pop
    ///     operation.  This allows you to use the comparator to parameterize this collection into a
    ///     MinHeap, MaxHeap, or other type of ordered heap using your own custom type.
    ///     For convenience, this library contains the Min and Max comparator, which provide
    ///     comparisons for all built in primitives.
    /// </summary>
    [NativeContainer]
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(NativeHeapDebugView<,>))]
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeHeap<T, U> : IDisposable
        where T : unmanaged
        where U : unmanaged, IComparer<T>
    {
        #region API

        public const int DEFAULT_CAPACITY = 128;

        /// <summary>
        ///     Returns the number of elements that this collection can hold before the internal structures
        ///     need to be reallocated.
        /// </summary>
        public int Capacity
        {
            get
            {
                unsafe
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
#endif
                    return this.Data->Capacity;
                }
            }
            set
            {
                unsafe
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
                    if (value < this.Data->Count)
                    {
                        throw new ArgumentException(
                            $"Capacity of {value} cannot be smaller than count of {this.Data->Count}.");
                    }
#endif
                    var newTable = (TableValue*) Malloc(SizeOf<TableValue>() * value, AlignOf<TableValue>(),
                        this.Allocator);
                    var newHeap = (HeapNode<T>*) Malloc(SizeOf<HeapNode<T>>() * value, AlignOf<HeapNode<T>>(),
                        this.Allocator);

                    var toCopy = this.Data->Capacity < value ? this.Data->Capacity : value;
                    MemCpy(newTable, this.Data->Table, toCopy * SizeOf<TableValue>());
                    MemCpy(newHeap, this.Data->Heap, toCopy * SizeOf<HeapNode<T>>());

                    for (var i = 0; i < value - this.Data->Capacity; i++)
                    {
                        //For each new heap node, make sure that it has a new unique index
                        newHeap[i + this.Data->Capacity] = new HeapNode<T>()
                        {
                            TableIndex = i + this.Data->Capacity,
                        };

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                        //For each new table value, make sure it has a specific version
                        newTable[i + this.Data->Capacity] = new TableValue()
                        {
                            Version = 1,
                        };
#endif
                    }

                    Free(this.Data->Table, this.Allocator);
                    Free(this.Data->Heap, this.Allocator);

                    this.Data->Table = newTable;
                    this.Data->Heap = newHeap;

                    this.Data->Capacity = value;
                }
            }
        }

        /// <summary>
        ///     Returns the number of elements currently contained inside this collection.
        /// </summary>
        public int Count
        {
            get
            {
                unsafe
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
#endif
                    return this.Data->Count;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the comparator used for this Heap. Note that you can only set the comparator
        ///     when the Heap is empty.
        /// </summary>
        public U Comparator
        {
            get
            {
                unsafe
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
#endif
                    return this.Data->Comparator;
                }
            }
            set
            {
                unsafe
                {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);

                    if (this.Data->Count != 0)
                    {
                        throw new InvalidOperationException(
                            "Can only change the comparator of a NativeHeap when it is empty.");
                    }
#endif
                    this.Data->Comparator = value;
                }
            }
        }

        /// <summary>
        ///     Constructs a new NativeHeap using the given Allocator.  You must call Dispose on this collection
        ///     when you are finished with it.
        /// </summary>
        /// <param name="allocator">
        ///     You must specify an allocator to use for the creation of the internal data structures.
        /// </param>
        /// <param name="initialCapacity">
        ///     You can optionally specify the default number of elements this collection can contain before the internal
        ///     data structures need to be re-allocated.
        /// </param>
        /// <param name="comparator">
        ///     You can optionally specify the comparator used to order the elements in this collection.  The Pop operation will
        ///     always return the smallest element according to the ordering specified by this comparator.
        /// </param>
        public NativeHeap(Allocator allocator, int initialCapacity = DEFAULT_CAPACITY, U comparator = default) :
            this(initialCapacity, comparator, allocator, disposeSentinelStackDepth: 1) { }

        /// <summary>
        ///     Disposes of this container and deallocates its memory immediately.
        ///     Any NativeHeapIndex structures obtained will be invalidated and cannot be used again.
        /// </summary>
        public void Dispose()
        {
            unsafe
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                DisposeSentinel.Dispose(ref this.m_Safety, ref this.m_DisposeSentinel);
#endif

                this.Data->Count = 0;
                this.Data->Capacity = 0;

                Free(this.Data->Heap, this.Allocator);
                Free(this.Data->Table, this.Allocator);
                Free(this.Data, this.Allocator);
            }
        }

        /// <summary>
        ///     Removes all elements from this container.  Any NativeHeapIndex structures obtained will be
        ///     invalidated and cannot be used again.
        /// </summary>
        public void Clear()
        {
            unsafe
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);

                for (var i = 0; i < this.Data->Count; i++)
                {
                    var node = this.Data->Heap[i];
                    this.Data->Table[node.TableIndex].Version++;
                }
#endif

                this.Data->Count = 0;
            }
        }

        /// <summary>
        ///     Returns whether or not the given NativeHeapIndex is a valid index for this container.  If true,
        ///     that index can be used to Remove the element tied to that index from the container.
        ///     This method will always return true if Unity safety checks is turned off.
        /// </summary>
        public bool IsValidIndex(NativeHeapIndex index)
        {
            var isValid = true;
            var errorCode = 0;
            this.IsValidIndexInternal(index, ref isValid, ref errorCode);
            return isValid;
        }

        /// <summary>
        ///     Throws an ArgumentException if the provided NativeHeapIndex is not valid for this container.
        ///     This method will never throw if Unity safety checks is turned off.
        /// </summary>
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public void AssertValidIndex(NativeHeapIndex index)
        {
            var isValid = true;
            var errorCode = 0;
            this.IsValidIndexInternal(index, ref isValid, ref errorCode);
            if (isValid)
            {
                return;
            }

            switch (errorCode)
            {
                case VALIDATION_ERROR_WRONG_INSTANCE:
                    throw new ArgumentException("The provided ItemHandle was not valid for this NativeHeap.  " +
                                                "It was taken from a different instance.");
                case VALIDATION_ERROR_INVALID:
                    throw new ArgumentException("The provided ItemHandle was not valid for this NativeHeap.");
                case VALIDATION_ERROR_REMOVED:
                    throw new ArgumentException("The provided ItemHandle was not valid for this NativeHeap.  " +
                                                "The item it pointed to might have already been removed.");
            }
        }

        /// <summary>
        ///     Returns the next element that would be obtained if Pop was called.  This is the first/smallest
        ///     item according to the ordering specified by the comparator.
        ///     This method is an O(1) operation.
        ///     This method will throw an InvalidOperationException if the collection is empty.
        /// </summary>
        public T Peek()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
#endif

            if (!this.TryPeek(out var t))
            {
                throw new InvalidOperationException("Cannot Peek NativeHeap when the count is zero.");
            }

            return t;
        }

        /// <summary>
        ///     Returns the next element that would be obtained if Pop was called.  This is the first/smallest
        ///     item according to the ordering specified by the comparator.
        ///     This method is an O(1) operation.
        ///     This method will return true if an element could be obtained, or false if the container is empty.
        /// </summary>
        public bool TryPeek(out T t)
        {
            unsafe
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
#endif

                if (this.Data->Count == 0)
                {
                    t = default;
                    return false;
                }
                else
                {
                    unsafe
                    {
                        t = this.Data->Heap[0].Item;
                        return true;
                    }
                }
            }
        }

        /// <summary>
        ///     Removes the first/smallest element from the container and returns it.
        ///     This method is an O(log(n)) operation.
        ///     This method will throw an InvalidOperationException if the collection is empty.
        /// </summary>
        public T Pop()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
#endif

            if (!this.TryPop(out var t))
            {
                throw new InvalidOperationException("Cannot Pop NativeHeap when the count is zero.");
            }

            return t;
        }

        /// <summary>
        ///     Removes the first/smallest element from the container and returns it.
        ///     This method is an O(log(n)) operation.
        ///     This method will return true if an element could be obtained, or false if the container is empty.
        /// </summary>
        public bool TryPop(out T t)
        {
            unsafe
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
#endif

                if (this.Data->Count == 0)
                {
                    t = default;
                    return false;
                }

                var rootNode = this.Data->Heap[0];

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                //Update version to invalidate all existing handles
                this.Data->Table[rootNode.TableIndex].Version++;
#endif

                //Grab the last node off the end and remove it
                var lastNodeIndex = --this.Data->Count;
                var lastNode = this.Data->Heap[lastNodeIndex];

                //Move the previous root to the end of the array to fill the space we just made
                this.Data->Heap[lastNodeIndex] = rootNode;

                //Finally insert the previously last node at the root and bubble it down
                this.InsertAndBubbleDown(lastNode, 0);

                t = rootNode.Item;
                return true;
            }
        }

        /// <summary>
        ///     Inserts the provided element into the container.  It may later be removed by a call to Pop,
        ///     TryPop, or Remove.
        ///     This method returns a NativeHeapIndex.  This index can later be used to Remove the item from
        ///     the collection.  Once the item is removed by any means, this NativeHeapIndex will become invalid.
        ///     If an item is re-added to the collection after it has been removed, Insert will return a NEW
        ///     index that is distinct from the previous index.  Each index can only be used exactly once to
        ///     remove a single item.
        ///     This method is an O(log(n)) operation.
        /// </summary>
        public NativeHeapIndex Insert(in T t)
        {
            unsafe
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
#endif

                if (this.Data->Count == this.Data->Capacity)
                {
                    this.Capacity *= 2;
                }

                var node = this.Data->Heap[this.Data->Count];
                node.Item = t;

                var insertIndex = this.Data->Count++;

                this.InsertAndBubbleUp(node, insertIndex);

                return new NativeHeapIndex()
                {
                    TableIndex = node.TableIndex,
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    Version = this.Data->Table[node.TableIndex].Version,
                    StructureId = this.Id,
#endif
                };
            }
        }

        /// <summary>
        ///     Removes the element tied to this NativeHeapIndex from the container.  The NativeHeapIndex must be
        ///     the result of a previous call to Insert on this container.  If the item has already been removed by
        ///     any means, this method will throw an ArgumentException.
        ///     This method will invalidate the provided index.  If you re-insert the removed object, you must use
        ///     the NEW index to remove it again.
        ///     This method is an O(log(n)) operation.
        /// </summary>
        public T Remove(NativeHeapIndex index)
        {
            unsafe
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);

                this.AssertValidIndex(index);
#endif
                var indexToRemove = this.Data->Table[index.TableIndex].HeapIndex;

                var toRemove = this.Data->Heap[indexToRemove];

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                this.Data->Table[toRemove.TableIndex].Version++;
#endif

                var lastNode = this.Data->Heap[--this.Data->Count];

                //First we move the node to remove to the end of the heap
                WriteArrayElement(this.Data->Heap, this.Data->Count, toRemove);

                if (indexToRemove != 0)
                {
                    var parentIndex = (indexToRemove - 1) / 2;
                    var parentNode = this.Data->Heap[parentIndex];
                    if (this.Data->Comparator.Compare(lastNode.Item, parentNode.Item) < 0)
                    {
                        this.InsertAndBubbleUp(lastNode, indexToRemove);
                        return toRemove.Item;
                    }
                }

                //If we couldn't bubble up, bubbling down instead
                this.InsertAndBubbleDown(lastNode, indexToRemove);

                return toRemove.Item;
            }
        }

        #endregion

        #region IMPLEMENTATION

        internal const int VALIDATION_ERROR_WRONG_INSTANCE = 1;
        internal const int VALIDATION_ERROR_INVALID = 2;
        internal const int VALIDATION_ERROR_REMOVED = 3;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal unsafe static int NextId = 1;
        internal unsafe int Id;

        internal unsafe AtomicSafetyHandle m_Safety;

        [NativeSetClassTypeToNullOnSchedule]
        internal unsafe DisposeSentinel m_DisposeSentinel;
#endif

        [NativeDisableUnsafePtrRestriction]
        internal unsafe HeapData<T, U>* Data;

        internal unsafe Allocator Allocator;

        internal unsafe NativeHeap(
            int initialCapacity, U comparator, Allocator allocator,
            int disposeSentinelStackDepth
        )
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (initialCapacity <= 0)
            {
                throw new ArgumentException(nameof(initialCapacity),
                    "Must provide an initial capacity that is greater than zero.");
            }

            if (allocator == Allocator.None ||
                allocator == Allocator.Invalid ||
                allocator == Allocator.AudioKernel)
            {
                throw new ArgumentException(nameof(allocator),
                    "Must provide an Allocator type of Temp, TempJob, or Persistent.");
            }

            DisposeSentinel.Create(out this.m_Safety, out this.m_DisposeSentinel, disposeSentinelStackDepth, allocator);
            this.Id = Interlocked.Increment(ref NextId);
#endif

            this.Data = (HeapData<T, U>*) Malloc(SizeOf<HeapData<T, U>>(), AlignOf<HeapData<T, U>>(), allocator);
            this.Data->Heap = (HeapNode<T>*) Malloc(SizeOf<HeapNode<T>>() * initialCapacity, AlignOf<HeapNode<T>>(),
                allocator);
            this.Data->Table =
                (TableValue*) Malloc(SizeOf<TableValue>() * initialCapacity, AlignOf<TableValue>(), allocator);

            this.Allocator = allocator;

            for (var i = 0; i < initialCapacity; i++)
            {
                this.Data->Heap[i] = new HeapNode<T>()
                {
                    TableIndex = i,
                };
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                this.Data->Table[i] = new TableValue()
                {
                    Version = 1,
                };
#endif
            }

            this.Data->Count = 0;
            this.Data->Capacity = initialCapacity;
            this.Data->Comparator = comparator;
        }

        internal unsafe void InsertAndBubbleDown(HeapNode<T> node, int insertIndex)
        {
            while (true)
            {
                var indexL = insertIndex * 2 + 1;
                var indexR = insertIndex * 2 + 2;

                //If the left index is off the end, we are finished
                if (indexL >= this.Data->Count)
                {
                    break;
                }

                if (indexR >= this.Data->Count || this.Data->Comparator.Compare(this.Data->Heap[indexL].Item,
                        this.Data->Heap[indexR].Item) <= 0)
                {
                    //left is smaller (or the only child)
                    var leftNode = this.Data->Heap[indexL];

                    if (this.Data->Comparator.Compare(node.Item, leftNode.Item) <= 0)
                    {
                        //Last is smaller or equal to left, we are done
                        break;
                    }

                    this.Data->Heap[insertIndex] = leftNode;
                    this.Data->Table[leftNode.TableIndex].HeapIndex = insertIndex;
                    insertIndex = indexL;
                }
                else
                {
                    //right is smaller
                    var rightNode = this.Data->Heap[indexR];

                    if (this.Data->Comparator.Compare(node.Item, rightNode.Item) <= 0)
                    {
                        //Last is smaller than or equal to right, we are done
                        break;
                    }

                    this.Data->Heap[insertIndex] = rightNode;
                    this.Data->Table[rightNode.TableIndex].HeapIndex = insertIndex;
                    insertIndex = indexR;
                }
            }

            this.Data->Heap[insertIndex] = node;
            this.Data->Table[node.TableIndex].HeapIndex = insertIndex;
        }

        internal unsafe void InsertAndBubbleUp(HeapNode<T> node, int insertIndex)
        {
            while (insertIndex != 0)
            {
                var parentIndex = (insertIndex - 1) / 2;
                var parentNode = this.Data->Heap[parentIndex];

                //If parent is actually less or equal to us, we are ok and can break out
                if (this.Data->Comparator.Compare(parentNode.Item, node.Item) <= 0)
                {
                    break;
                }

                //We need to swap parent down
                this.Data->Heap[insertIndex] = parentNode;
                //Update table to point to new heap index
                this.Data->Table[parentNode.TableIndex].HeapIndex = insertIndex;

                //Restart loop trying to insert at parent index
                insertIndex = parentIndex;
            }

            this.Data->Heap[insertIndex] = node;
            this.Data->Table[node.TableIndex].HeapIndex = insertIndex;
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        internal unsafe void IsValidIndexInternal(NativeHeapIndex index, ref bool result, ref int errorCode)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);

            if (index.StructureId != this.Id)
            {
                errorCode = VALIDATION_ERROR_WRONG_INSTANCE;
                result = false;
                return;
            }

            if (index.TableIndex >= this.Data->Capacity)
            {
                errorCode = VALIDATION_ERROR_INVALID;
                result = false;
                return;
            }

            var tableValue = this.Data->Table[index.TableIndex];
            if (tableValue.Version != index.Version)
            {
                errorCode = VALIDATION_ERROR_REMOVED;
                result = false;
                return;
            }
#endif
        }

        #endregion
    }

    internal unsafe class NativeHeapDebugView<T, U>
        where T : unmanaged
        where U : unmanaged, IComparer<T>
    {
        private NativeHeap<T, U> _heap;

        public NativeHeapDebugView(NativeHeap<T, U> heap)
        {
            this._heap = heap;
        }

        public int Count => this._heap.Count;
        public int Capacity => this._heap.Capacity;
        public U Comparator => this._heap.Comparator;

        public T[] Items
        {
            get
            {
                var items = new T[this._heap.Count];
                for (var i = 0; i < items.Length; i++)
                {
                    unsafe
                    {
                        items[i] = this._heap.Data->Heap[i].Item;
                    }
                }

                return items;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct TableValue
    {
        public int HeapIndex;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        public int Version;
#endif
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct HeapData<T, U>
        where T : unmanaged
    {
        public int Count;
        public int Capacity;
        public unsafe HeapNode<T>* Heap;
        public unsafe TableValue* Table;
        public U Comparator;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct HeapNode<T>
        where T : unmanaged
    {
        public T Item;
        public int TableIndex;
    }
}
