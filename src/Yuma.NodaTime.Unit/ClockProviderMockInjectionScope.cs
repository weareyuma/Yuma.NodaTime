#region Copyright & License

// Copyright © 2024 - 2025 Yuma
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using Moq;

namespace Yuma;

[SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Does not release unmanaged resources but instead reinstates the default singleton after the test scope.")]
[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
public class ClockProviderMockInjectionScope : IDisposable
{
	public ClockProviderMockInjectionScope()
	{
		_clockProvider = ClockProvider.Instance;
		Mock = new Mock<ClockProvider> {
			CallBase = true
		}.As<IClockProvider>();
		ClockProvider.Instance = Mock.Object;
	}

	#region IDisposable Members

	[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
	public void Dispose()
	{
		ClockProvider.Instance = _clockProvider;
	}

	#endregion

	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
	public Mock<IClockProvider> Mock { get; }

	private readonly IClockProvider _clockProvider;
}
