using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class ContinuationManager
	{
		private class Job
		{
			public Func<bool> Completed
			{
				get;
				private set;
			}

			public Action ContinueWith
			{
				get;
				private set;
			}

			public Job(Func<bool> completed, Action continueWith)
			{
				Completed = completed;
				ContinueWith = continueWith;
			}
		}

		private static readonly List<Job> jobs = new List<Job>();

		public static void Add(Func<bool> completed, Action continueWith)
		{
			if (!jobs.Any())
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(Update));
			}
			jobs.Add(new Job(completed, continueWith));
		}

		private static void Update()
		{
			for (int num = 0; num >= 0; num--)
			{
				Job job = jobs[num];
				if (job.Completed())
				{
					job.ContinueWith();
					jobs.RemoveAt(num);
				}
			}
			if (!jobs.Any())
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(Update));
			}
		}
	}
}
