// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

namespace Microsoft.Diagnostics.Runtime {
	internal sealed class RefCountedFreeLibrary {
		private readonly IntPtr _library;
		private int _refCount;

		public RefCountedFreeLibrary(IntPtr library) {
			_library = library;
			_refCount = 1;
		}

		public int AddRef() {
			return Interlocked.Increment(ref _refCount);
		}

		public int Release() {
			int count = Interlocked.Decrement(ref _refCount);
			if (count == 0 && _library != IntPtr.Zero)
				DataTarget.PlatformFunctions.FreeLibrary(_library);

			return count;
		}
	}
}
