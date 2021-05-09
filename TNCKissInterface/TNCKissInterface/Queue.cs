using System;
using System.Collections;
using System.Threading;

namespace TNCKissInterface
{
    //
    // Blocking Queue routines.
    //
    // Copyright 2008-2011 - Peter R. Woods (N6PRW)
    //
    public class BlockingQueue
    {
        AutoResetEvent deQWait;
        Int32 qSize;
        Queue Q;
        Boolean waiting = false;
        Boolean _enabled = true;
        String n = "";

        //#region Constructors & public methods

        //
        // Constructor.  Queuesize defines the number
        // of objects the queue can hold.
        //
        public BlockingQueue(Int32 queueSize)
        {
            deQWait = new AutoResetEvent(false);
            Q = new Queue(queueSize);
            qSize = queueSize;
        }
        public BlockingQueue(Int32 queueSize, String name)
        {
            deQWait = new AutoResetEvent(false);
            Q = new Queue(queueSize);
            qSize = queueSize;
            n = name;
        }

        //
        // Add an object to the Queue
        // Returns false if Queue is full or disabled
        //
        public Boolean Enqueue(Object item)
        {
            if ((Q.Count == qSize) || !_enabled)
            {
                //
                // Queue is full or disabled
                //
                return false;
            }
            lock (Q.SyncRoot)
            {
                if ((Q.Count == 0) && waiting)
                {
                    //
                    // Someone was waiting for data arrive at the Queue, so send off 
                    // a signal so they will wake up and retreive the entry
                    //
                    deQWait.Set();
                }
                Q.Enqueue(item);
            }
            return true;
        }

        //
        // Remove an object from the Queue
        // Calling thread will wait if the queue is empty
        // Returns null when queue is disabled
        //
        public Object Dequeue()
        {
            if (!_enabled)
            {
                return null;
            }

            lock (Q.SyncRoot)
            {
                if (Q.Count == 0)
                {
                    //
                    // Set the wait flag while we still have the Queue locked.  We can then
                    // release the lock and wait for an item to be added to the Queue
                    waiting = true;
                }
            }

            if (waiting)
            {
                //
                // Wait for a singal if the queue is empty
                //
                deQWait.WaitOne();
                waiting = false;
            }

            lock (Q.SyncRoot)
            {
                if (!_enabled || (Q.Count == 0))
                {
                    //
                    // Nothing present.  This occurs when the Queue is stopped or we need to release
                    // the waiting thread.
                    //
                    return null;
                }

                return Q.Dequeue();
            }
        }

        public Object Peek()
        {
            //
            // Return the entry at the head of the queue w/o actually removing it
            //
            lock (Q.SyncRoot)
            {
                if (_enabled && (Q.Count > 0))
                {
                    return Q.Peek();
                }
                else
                {
                    return null;
                }
            }
        }

        public Boolean enabled
        {
            get { return _enabled; }
            set
            {
                //
                // Disable/enable the Queue
                //
                if (value && !_enabled)
                {
                    //
                    // Enable the queue
                    //
                    lock (Q.SyncRoot)
                    {
                        _enabled = true;
                    }
                }
                else if (!value && _enabled)
                {
                    //
                    // Disable the queue
                    //
                    lock (Q.SyncRoot)
                    {
                        _enabled = false;
                        Wakeup();
                        Q.Clear();
                    }
                }
            }
        }

        public void Wakeup()
        {
            //
            // Release any waiting threads.
            //
            if (waiting)
            {
                deQWait.Set();
            }
        }

        //#endregion
    }
}
