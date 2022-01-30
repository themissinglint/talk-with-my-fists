using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;


/// <summary>This utility has some methods for scheduling method calls on specific threads.
/// This could be the 'Main' thread that interacts with the graphics pipeline, the 'UI' thread which is where any native view interaction should occur, 
/// or a 'Background' thread which is guaranteed to not be either of those threads and is where long IO or networks calls should be done.</summary>
public class ThreadingUtil {

	// The static instance of ThreadingUtil that will be used as the singleton.
	private int mainThreadID;
	
	// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
	//The singleton is available for getting as soon as necessary on any thread, even if two threads are trying to get it at the same time.
	static ThreadingUtil() {
	}

	// The behaviour, needed so that we can call code during the Update method.
	private ThreadingUtilBehaviour behaviour;

	#if !UNITY_WEBGL
	// A collection of timers that are waiting to expire before they will run the code they have been asked to run. Needed to ensure no timer is GC'd before it's run.
	private ArrayList timers;
	#endif

	public static ThreadingUtil Instance { get; } = new ThreadingUtil();

	private ThreadingUtil(){
		InitializeThreadingUtilBehaviour();
		#if !UNITY_WEBGL
		timers = ArrayList.Synchronized(new ArrayList()); //The use of this collection is so simple that this naiive implementation of synchronization is sufficient
		#endif
	}
	
	public bool IsMainThread { get { return Thread.CurrentThread.ManagedThreadId == mainThreadID; }}
	
	void InitializeThreadingUtilBehaviour() {
		// Is the app crashing on this line? You probably forgot to include an InitializationScript on your scene. (If not, adjust the script execution 
		// order so that the InitializationScript runs first.)
		behaviour = new GameObject("ThreadingUtilBehaviour").AddComponent<ThreadingUtilBehaviour>(); 
		mainThreadID = Thread.CurrentThread.ManagedThreadId;
	}
	
	[System.Diagnostics.ConditionalAttribute("DEVELOPMENT_BUILD")]
	private void CheckThread() {
		if (!Instance.IsMainThread)
			throw new InvalidOperationException("Attempting to start a coroutine from another thread");
	}
	
	private struct DelayedAction {
		public Action ToRun;
		public float Delay;
	}

	/// <summary>Runs some code on the main Unity thread. From what I can tell, a few things need to be run on the main thread
	/// These include calling JavaAndroidObject methods and also *getting a random number from UnityEngine.Random.Range*
	/// Since Unity doesn't have a built in way to add runnables to the main thread (that I could find), we will
	/// use a MonoBehaviour with DontDestroyOnLoad called on it and it will run these methods during overriden Update() method
	/// If you see the error 'JNI ERROR (app bug): accessed stale local global reference' in the logcat, 
	/// this is the JavaAndroidObject issue.</summary>
	/// <param name="runMethod">Code to run.</param>
	/// <param name="delaySeconds">Optional. Number of seconds to wait before running the method. Defaults to running as soon as possible. </param>
	public void RunOnUnityMainThread(Action runMethod, float delaySeconds = 0.0f){
		if (delaySeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(delaySeconds), "delaySeconds must not be negative");
		if (delaySeconds < 0.0001) // floating point equality to 0.0
			delaySeconds = -1; // Do not wait a frame. Run this action the same frame as we get it rather than scheduling a coroutine.
		behaviour.AddCodeToRunFromOffMainThread(runMethod, delaySeconds);
	}
	
	/// <summary>Runs some code on the unity main thread, optionally waiting for the code to finish running before moving on. This method
	/// is safe to call from any thread; in particular, this means that calling it from the main thread will not result in a deadlock.
	/// If this is the main Unity thread, then the code passed in will be run immediately, before returning, regardless of the value of
	/// waitForCompletion.</summary>
	/// <param name="runMethod">Code to be executed on the main thread.</param>
	/// <param name="waitForCompletion">If true, this method will not return until the code has finished running. If false, it may return 
	/// before or after the code is run.</param>
	public void RunOnUnityMainThread(Action runMethod, bool waitForCompletion)
	{
		if (runMethod == null)
			return;
		if (IsMainThread)
			runMethod();
		else if (waitForCompletion)
		{
			object monitor = new object();
			Monitor.Enter(monitor);
			RunOnUnityMainThread(()=>{
				runMethod();
				Monitor.Enter(monitor); // if not yet waited, this will block until the other thread waits.
				Monitor.Pulse(monitor);
				Monitor.Exit(monitor); 
			});
			Monitor.Wait(monitor);
			Monitor.Exit(monitor);
		} else {
			RunOnUnityMainThread(runMethod);
		}
	}

	/// <summary>Runs some code on the main Unity thread in a coroutine</summary>
	/// <param name="coroutine">The coroutine to start.</param>
	/// <param name="delaySeconds">Optional. Number of seconds to wait before starting the coroutine. Defaults to running as soon as possible. </param>
	public void RunOnUnityMainThread(IEnumerator coroutine, float delaySeconds = 0.0f){
		if (delaySeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(delaySeconds), "delaySeconds must not be negative");
		if (delaySeconds < 0.0001) // floating point equality to 0.0
			delaySeconds = -1; // Do not wait a frame. Run this action the same frame as we get it rather than scheduling a coroutine.
		behaviour.AddCodeToRunFromOffMainThread(()=>behaviour.StartCoroutine(coroutine), delaySeconds);
	}

	/// <summary>Runs a coroutine consisting solely of the AsyncOperations passed in</summary>
	/// <param name="operations">The operations to run, in order.</param>
	public void RunAsyncOperation(params AsyncOperation[] operations){
		CheckThread();
		behaviour.StartCoroutine(operations.GetEnumerator());
	}

	/// <summary>
	/// Runs some code later on the main Unity thread, assuming this is running on the main Unity thread. Do not use this method if
	/// not running on the main Unity thread. In that case, use <see cref="RunOnUnityMainThread(IEnumerator, float)"/> instead.
	/// </summary>
	/// <param name="runMethod">Code to run.</param>
	/// <param name="delaySeconds">Number of seconds to wait before running the method. Optional, defaults to on the next update if not given.</param>
	/// <returns>An object that can be passed to CancelRunLater to stop running it later (unless it's already run)</returns>
	public object RunLater(Action runMethod, float delaySeconds = 0.0f){
		CheckThread();
		if (delaySeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(delaySeconds), "delaySeconds must not be negative");
		return behaviour.RunAfterDelay(runMethod, delaySeconds, -1);
	}


	public object RunLaterRepeating(Action runMethod, float delaySeconds, float repeatDelaySeconds) {
		CheckThread();
		if (delaySeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(delaySeconds), "delaySeconds must not be negative");
		if (repeatDelaySeconds <= 0)
			throw new ArgumentOutOfRangeException(nameof(repeatDelaySeconds), "repeatDelaySeconds must be positive");
		return behaviour.RunAfterDelay(runMethod, delaySeconds, repeatDelaySeconds);
	}

	/// <summary>
	/// Begins a coroutine later on the main Unity thread, assuming this is running on the main Unity thread. Do not use this method if
	/// not running on the main Unity thread. In that case, use <see cref="RunOnUnityMainThread(IEnumerator, float)"/> instead, calling this method from there.
	/// </summary>
	/// <param name="coroutine">The coroutine to begin</param>
	/// <param name="delaySeconds">Number of seconds to wait before beginning the coroutine. Optional, defaults to immediately if not given.</param>
	/// <returns>An object that can be passed to CancelRunLater to stop running it later (unless it's already run)</returns>
	public object RunLater(IEnumerator coroutine, float delaySeconds = 0.0f) {
		CheckThread();
		if (delaySeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(delaySeconds), "delaySeconds must not be negative");
		
		if (delaySeconds < 0.0001)
		{
			behaviour.StartCoroutine(coroutine);
			return coroutine;
		}

		return behaviour.RunAfterDelay(()=>behaviour.StartCoroutine(coroutine), delaySeconds, -1);
	}

	/// <summary>
	/// Cancels an item previously scheduled using RunLater. If running on the main Unity thread, it is canceled immediately. If running on a 
	/// background thread, it may be canceled later.
	/// </summary>
	/// <param name="receipt">The object returned by the RunLater invocation you wish to cancel.</param>
	public void CancelRunLater(object receipt)
	{
		if (receipt is IEnumerator && behaviour) {
			if (IsMainThread) {
				behaviour.StopCoroutine((IEnumerator)receipt);
			} else {
				RunOnUnityMainThread(()=>{
					CancelRunLater(receipt);
				});
			}
		}
	}
	
	/// <summary>
	/// Runs some code later on a background thread.
	/// </summary>
	/// <param name="runMethod">Code to run.</param>
	/// <param name="delaySeconds">Number of seconds to wait before running the method. Optional, defaults to running as soon as a 
	/// background thread is available if not given.</param>
	public void RunOnBackgroundThread(Action runMethod, float delaySeconds = 0.0f) {
		#if UNITY_WEBGL
		throw new System.InvalidOperationException("Background threading is not supported on this platform");
		#else
		if (delaySeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(delaySeconds), "delaySeconds must not be negative");
		if (delaySeconds < 0.0001) {
			ThreadPool.QueueUserWorkItem((state)=>runMethod(), null);
		} else {
			Timer timer = new Timer((state)=>{
				// The timer callback state argument is the timer itself if none is specified
				timers.Remove(state);

				// The timer callback is automatically handled by a thread pool, so we just call it
				runMethod();
			});
			timers.Add(timer);
			timer.Change((int)(delaySeconds * 1000), Timeout.Infinite);
		}
		#endif
	}
	
	/// <summary>
	/// Flattens out IEnumerators that return other IEnumerators into a single IEnumerator. Coroutines don't normally recurse into returned IEnumerators. 
	/// Wrap the coroutine using this method to make that happen.
	/// </summary>
	public static IEnumerator FlattenCoroutine(IEnumerator coroutine)
	{
		Stack<IEnumerator> coroutines = new Stack<IEnumerator>();
		IEnumerator current = coroutine;
		
		while (current != null)
		{
			if (current.MoveNext())
			{
				object value = current.Current;
				if (value is IEnumerator)
				{
					coroutines.Push(current);
					current = (IEnumerator)value;
					continue;
				}
				yield return current.Current;
			} else if (coroutines.Count > 0) {
				current = coroutines.Pop();
			} else
				current = null;
		}
	}

	// I had considered disabling the gameObject if no actions are scheduled (including started coroutines).
	// However, this introduces several problem that I don't want to deal with, involving otherwise unnecessary locking. 
	// We'll just let it stick around doing nothing.
	private class ThreadingUtilBehaviour : MonoBehaviour{
		private List<DelayedAction> toRunActions = new List<DelayedAction>();
		private List<DelayedAction> toRunActionsSwap = new List<DelayedAction>();

		public void AddCodeToRunFromOffMainThread(Action runMethod, float delaySeconds){
			lock(toRunActions) {
				toRunActions.Add(new DelayedAction{ToRun=runMethod, Delay=delaySeconds});
			}
		}

		void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		void Update(){
			if (toRunActions.Count > 0) {
				lock(toRunActions)
				{
					List<DelayedAction> temp = toRunActions;
					toRunActions = toRunActionsSwap;
					toRunActionsSwap = temp;
				}
				if (toRunActionsSwap.Count > 0) {
					foreach(var action in toRunActionsSwap)
					{
						//A delay of 0 will run next frame after Update has finished on all MonoBehaviours (after any yield return nulls)
						RunAfterDelay(action.ToRun, action.Delay, -1);
					}
					toRunActionsSwap.Clear();
				}
			}
		}
		
		void OnDestroy() {
			// Why!? Why did you destroy me? Don't do it!
			// (This does happen when you click "stop" in the editor.)
			#if !UNITY_EDITOR
			Debug.Log("ThreadingUtilBehaviour destroyed! Don't do that.");
			#endif
		}
		
		public object RunAfterDelay(Action action, float delay, float repeatDelay) {
			IEnumerator coroutine = RunDelayedAction(action, delay, repeatDelay);
			StartCoroutine(coroutine);
			return coroutine;
		}

		// TODO: Rename this? Call it "create" rather than "run"?
		IEnumerator RunDelayedAction(Action action, float delay, float repeatDelay = -1) {
			if (delay > 0.0001)
				yield return new WaitForSeconds(delay);
			else if (delay < 0.0001 && delay > -0.0001) // floating point equality to 0
				yield return null;
			// If delay < 0, then we run immediately.
			
			while (true) {
				try {
					action();
				} catch (Exception e){
					//TODO: It's not clear if we should be catching this exception. Do we want Unity to handle it?
					Debug.Log("Exception while trying to run code on the main Unity thread: " + e);
					throw;	//Rethrow it so Unity can handle it by default
				}
				
				if (repeatDelay < 0) {
					yield break;
				}

				yield return new WaitForSeconds(repeatDelay);
			}
		}
	}
	
	#region Testing

	public void ResetForTest() {
		InitializeThreadingUtilBehaviour();
	}
	
	#endregion
}