using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class UnityWebRequestAwaiter
{
	public static TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation requestOperation)
	{
		TaskCompletionSource<UnityWebRequest.Result> completionSource = new TaskCompletionSource<UnityWebRequest.Result>();
		requestOperation.completed += asyncOp => completionSource.TrySetResult(requestOperation.webRequest.result);
		if (requestOperation.isDone) { completionSource.TrySetResult(requestOperation.webRequest.result); }
		return completionSource.Task.GetAwaiter();
	}
}