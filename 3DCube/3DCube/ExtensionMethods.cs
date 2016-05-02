using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clifton.Core.ExtensionMethods
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Implements a ForEach for generic enumerators.
		/// </summary>
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
			{
				action(item);
			}
		}

		/// <summary>
		/// ForEach with an index.
		/// </summary>
		public static void ForEachWithIndex<T>(this IEnumerable<T> collection, Action<T, int> action)
		{
			int n = 0;

			foreach (var item in collection)
			{
				action(item, n++);
			}
		}

		/// <summary>
		/// Encapsulates testing for whether the event has been wired up.
		/// </summary>
		public static void Fire<TEventArgs>(this EventHandler<TEventArgs> theEvent, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			if (theEvent != null)
			{
				theEvent(sender, e);
			}
		}

		public static int to_i(this string s)
		{
			return Convert.ToInt32(s);
		}

		public static string to_s(this int i)
		{
			return i.ToString();
		}

		public static void Loop(this int i, Action action)
		{
			for (int n = 0; n < i; n++) action();
		}

		public static void LoopWithIndex(this int i, Action<int> action)
		{
			for (int n = 0; n < i; n++) action(n);
		}

		/// <summary>
		/// Returns true if the task is completed, canceled, or faulted.
		/// </summary>
		public static bool IsDone(this Task task)
		{
			return task.IsCanceled | task.IsCompleted | task.IsFaulted;
		}

		/// <summary>
		/// Asynchronous invoke on application thread.  Will return immediately unless invocation is not required.
		/// </summary>
		public static void BeginInvoke(this Control control, Action action)
		{
			if (control.InvokeRequired)
			{
				// We want a synchronous call here!!!!
				control.BeginInvoke((Delegate)action);
			}
			else
			{
				action();
			}
		}

		/// <summary>
		/// Synchronous invoke on application thread.  Will not return until action is completed.
		/// </summary>
		public static void Invoke(this Control control, Action action)
		{
			if (control.InvokeRequired)
			{
				// We want a synchronous call here!!!!
				control.Invoke((Delegate)action);
			}
			else
			{
				action();
			}
		}
	}
}
